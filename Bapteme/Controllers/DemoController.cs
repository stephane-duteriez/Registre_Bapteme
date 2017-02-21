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
		private int nbr_weeks;
		private string indice_unique;

		public DemoController(BaptemeDataContext db, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) : base(db, userManager)
		{
			_signInManager = signInManager;
		}

		public async Task<IActionResult> Start()
        {
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
			Random random = new Random();

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

			await creatCelebrations(list_clochers);

			await _db.SaveChangesAsync();
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