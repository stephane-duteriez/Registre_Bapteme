using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bapteme.Data;
using Microsoft.AspNetCore.Identity;
using Bapteme.Models;
using Microsoft.EntityFrameworkCore;

namespace Bapteme.Controllers
{
    public class DemoController : MyController
    {
		private readonly SignInManager<ApplicationUser> _signInManager;
		private ApplicationDbContext _dbUsers;

		private Random random;

		private int nbr_weeks;
		private string indice_unique;

		public DemoController(BaptemeDataContext db, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext dbUsers) : base(db, userManager)
		{
			_dbUsers = dbUsers;
			_signInManager = signInManager;
		}

		public async Task<IActionResult> Start()
        {
			random = new Random();
			ApplicationUser user = await _newDemoAsync();
			await _signInManager.SignInAsync(user, false);
			return RedirectToAction("Index", "Home", "");
		}

		public async Task<IActionResult> End()
		{
			await _removeDemoAsync();
			await _signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home", "");
		}

		private async Task<ApplicationUser> _newDemoAsync()
		{
			nbr_weeks = get_nbr_weeks_since_begining_2017();
			indice_unique = random.Next(1, 999999).ToString();

			ApplicationUser user = await creatDemoUser();

			Paroisse demo_paroisse = new Paroisse() { Name = "Ma Paroisse" , Demo = isDemo.True, Key = "maparoisse" + indice_unique};
			_db.Add(demo_paroisse);

			List<UserParoisse> list_uParoisse = new List<UserParoisse>() {
				new UserParoisse() {ParoisseId = demo_paroisse.Id, UserId = user.Id, Role = role.Administrateur },
				new UserParoisse() {ParoisseId = demo_paroisse.Id, UserId = user.Id, Role = role.Manager }
			};

			await _db.AddRangeAsync(list_uParoisse);

			List<Clocher> list_clochers = await creatClochers(demo_paroisse);

			await creatPermanences(list_clochers);

			List<Celebration> list_celebration = await creatCelebrations(list_clochers);

			await creatUserInCelebration(list_celebration, demo_paroisse.Id);

			await _db.SaveChangesAsync();
			await _dbUsers.SaveChangesAsync();
			return user;
		}

		private async Task _removeDemoAsync()
		{
			List<Paroisse> list_demoParishe = _db.Paroisses.Where(x => x.Demo == isDemo.True).ToList();
			foreach (Paroisse paroisse in list_demoParishe)
			{
				List<Clocher> list_clochers = await _db.Clochers.
					Include(x=>x.Permanences).
					Include(x=>x.Celebrations).
					Where(x => x.ParoisseId == paroisse.Id).
					ToListAsync();

				_db.RemoveRange(list_clochers);

				List<UserParoisse> list_users_in_paroisse = await _db.UserParoisse.Where(x => x.ParoisseId == paroisse.Id).ToListAsync();
				foreach (UserParoisse uParoisse in list_users_in_paroisse)
				{
					ApplicationUser user = await _userManager.FindByIdAsync(uParoisse.UserId);
					if (user != null)
					{
						_userManager.DeleteAsync(user);
					}
					_db.Remove(uParoisse);
				}
				_db.Remove(paroisse);
				_db.SaveChanges();
			}
		}

		private async Task<ApplicationUser> creatDemoUser()
		{
			
			var user = new ApplicationUser();
			user.UserName = "demo_manager" + indice_unique;
			user.Email = "default@default.com";
			user.FirstName = "Demo";
			user.LastName = "Manager";

			string userPWD = "A@Z200711a";

			IdentityResult chkUser = await _userManager.CreateAsync(user, userPWD);

			return user;
		}

		private async Task<List<Clocher>> creatClochers(Paroisse demo_paroisse)
		{
			List<Clocher> list_clocher = new List<Clocher>() {
				new Clocher() { Name = "St Martin", ParoisseId = demo_paroisse.Id } ,
				new Clocher() { Name = "St Maurice", ParoisseId = demo_paroisse.Id  },
				new Clocher() { Name = "St Mathieux", ParoisseId = demo_paroisse.Id  }
			};

			await _db.AddRangeAsync(list_clocher);
			return list_clocher;
		}

		private async Task creatPermanences(List<Clocher> list_clochers)
		{
			List<Permanence> list_permanences = new List<Permanence>()
			{
				new Permanence() {ClocherId = list_clochers[0].Id, Jour=JourSemaine.Lundi, Debut= new TimeSpan(9, 30, 00), Fin= new TimeSpan(11, 00, 00) },
				new Permanence() {ClocherId = list_clochers[0].Id, Jour=JourSemaine.Samedi, Debut= new TimeSpan(10, 00, 00), Fin= new TimeSpan(12, 00, 00)},
				new Permanence() {ClocherId = list_clochers[1].Id, Jour=JourSemaine.Mardi, Debut = new TimeSpan(14, 00, 00), Fin= new TimeSpan(16, 30, 00)},
				new Permanence() {ClocherId = list_clochers[1].Id, Jour=JourSemaine.Vendredi, Debut = new TimeSpan(9, 00, 00), Fin = new TimeSpan(11, 30, 00)},
				new Permanence() {ClocherId = list_clochers[2].Id, Jour=JourSemaine.Mercredi, Debut = new TimeSpan(9, 15, 00), Fin = new TimeSpan(11, 45, 00)},
				new Permanence() {ClocherId = list_clochers[2].Id, Jour=JourSemaine.Mercredi, Debut = new TimeSpan(13, 30, 00), Fin = new TimeSpan(17, 15, 00)}
			};

			await _db.AddRangeAsync(list_permanences);
		}

		private async Task<List<Celebration>> creatCelebrations(List<Clocher> list_clochers)
		{
			List<DateTime> list_default_dates = new List<DateTime>()
			{
				new DateTime(2017, 1, 7, 14, 00, 00),
				new DateTime(2017, 1, 14, 11, 30, 00),
				new DateTime(2017, 1, 15, 10, 30, 00),
				new DateTime(2017, 1, 21, 15, 15, 00),
				new DateTime(2017, 1, 28, 11, 30, 00)
			};

			List<Celebration> list_celebrations = new List<Celebration>()
			{
				new Celebration() { ClocherId = list_clochers[0].Id, Date = add_week(list_default_dates[0], nbr_weeks) },
				new Celebration() { ClocherId = list_clochers[0].Id, Date = add_week(list_default_dates[1], nbr_weeks) },
				new Celebration() { ClocherId = list_clochers[1].Id, Date = add_week(list_default_dates[2], nbr_weeks) },
				new Celebration() { ClocherId = list_clochers[1].Id, Date = add_week(list_default_dates[3], nbr_weeks) },
				new Celebration() { ClocherId = list_clochers[2].Id, Date = add_week(list_default_dates[4], nbr_weeks) },
			};

			await _db.AddRangeAsync(list_celebrations);

			return list_celebrations;
		}

		private async Task<List<ApplicationUser>> creatUserInCelebration(List<Celebration> list_celebration, Guid paroisseId)
		{
			int nbr = 15;
			List<DateTime> list_default_dates = generateDates(nbr);
			List<Adresse> list_adresses = generateAdresses(nbr);
			List<string> list_FirstNames = generateFirstName(nbr);
			List<string> list_lastNames = generateLastname(nbr);
			List<ApplicationUser> list_users = new List<ApplicationUser>();
			for (int i = 0; i < nbr; i++)
			{

				list_users.Add(new ApplicationUser()
				{
					FirstName = list_FirstNames[i],
					LastName = list_lastNames[i],
					IdAdress = list_adresses[i].Id,
					BirthDate = list_default_dates[i],
					CelebrationId = list_celebration[random.Next(0, list_celebration.Count())].Id					
				});
			}
			await _dbUsers.AddRangeAsync(list_users);
			List<UserParoisse> list_uParoisses = new List<UserParoisse>();
			for (int i = 0; i<nbr; i++)
			{
				list_uParoisses.Add(new UserParoisse() { ParoisseId = paroisseId, UserId = list_users[i].Id, Role=role.Contact });
			}
			await _db.AddRangeAsync(list_uParoisses);
			return list_users;
		}

		private List<DateTime> generateDates(int nbr)
		{
			List<DateTime> list_date = new List<DateTime>();
			for (int i = 0; i<nbr; i++)
			{
				list_date.Add(new DateTime(2016, random.Next(11, 12), random.Next(1, 30)));
			}
			return list_date;
		}

		private List<Adresse> generateAdresses(int nbr)
		{
			List<Adresse> list_adresses = new List<Adresse>();
			List<Ville> list_villes = new List<Ville>()
			{
				new Ville() { Name = "Carpin", CP = "46587"},
				new Ville() { Name = "Liberty", CP= "78451"},
				new Ville() { Name = "Balty", CP="94564"},
				new Ville() { Name = "Arvin", CP="12567"},
				new Ville() { Name = "Farière", CP="82463"}
			};
			List<string> list_rues = new List<string>() { "rue Lavender", "rue de la Liberté", "rue de la mairie", "rue de l'Église", "rue de Paris", "rue des Moineaux" };
			for (int i=0; i<nbr; i++)
			{
				int indice_ville = random.Next(0, list_villes.Count());
				list_adresses.Add(new Adresse()
				{
					Numero = random.Next(1, 300).ToString(),
					CP = list_villes[indice_ville].CP,
					Ville = list_villes[indice_ville].Name,
					Rue = list_rues[random.Next(0, list_rues.Count())]
				});
			}
			_db.AddRangeAsync(list_adresses);
			return list_adresses;
		}

		private List<string> generateFirstName(int nbr)
		{
			List<string> init_firstNames = new List<string>() { "Céline", "Christophe","Alexandra", "Olivier", "Marc", "Mathieu", "Veronique", "Rose", "Alexandre", "Gérémie", "Lucie", "Daniel", "Émily"};
			List<string> list_firstNames = new List<string>();
			for (int i=0; i<nbr; i++)
			{
				list_firstNames.Add(init_firstNames[random.Next(0, init_firstNames.Count())]);
			}
			return list_firstNames;
		}

		private List<string> generateLastname(int nbr)
		{
			List<string> init_lastname = new List<string>() { "Majory", "Perreault", "Loiseau", "Busson", "Desnoyer", "Bler", "Pelchat", "Michel", "Migneault", "Souplet", "Lapierre", "Giguère", "Leroux" };
			List<string> list_firstNames = new List<string>();
			for (int i=0; i<nbr; i++)
			{
				list_firstNames.Add(init_lastname[random.Next(0, init_lastname.Count())]);
			}
			return list_firstNames;
		}

		private int get_nbr_weeks_since_begining_2017()
		{
			TimeSpan time_elapse = DateTime.Now.Subtract(new DateTime(2017, 1, 1));
			return time_elapse.Days / 7;
		}

		private DateTime add_week(DateTime intitial_date, int nbr_weeks)
		{
			return intitial_date.AddDays(7 * nbr_weeks);
		}
	}
}