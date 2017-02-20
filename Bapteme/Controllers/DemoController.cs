using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bapteme.Data;
using Microsoft.AspNetCore.Identity;
using Bapteme.Models;

namespace Bapteme.Controllers
{
    public class DemoController : MyController
    {
		private readonly SignInManager<ApplicationUser> _signInManager;

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
			var user = new ApplicationUser();
			user.UserName = "demo_manager";
			user.Email = "default@default.com";
			user.FirstName = "Demo";
			user.LastName = "Manager";

			string userPWD = "A@Z200711a";

			IdentityResult chkUser = await _userManager.CreateAsync(user, userPWD);

			Paroisse demo_paroisse = new Paroisse() { Name = "Demo" , Demo = isDemo.True};
			_db.Add(demo_paroisse);
			_db.SaveChanges();

			UserParoisse uParoisse = new UserParoisse() { ParoisseId = demo_paroisse.Id, UserId = user.Id, Role = role.Manager };
			_db.Add(uParoisse);
			_db.SaveChanges();

			return user;
		}

		private async Task _removeDemoAsync()
		{
			List<Paroisse> list_demoParishe = _db.Paroisses.Where(x => x.Demo == isDemo.True).ToList();
			foreach (Paroisse paroisse in list_demoParishe)
			{
				List<UserParoisse> list_users_in_paroisse = _db.UserParoisse.Where(x => x.ParoisseId == paroisse.Id).ToList();
				foreach (UserParoisse uParoisse in list_users_in_paroisse)
				{
					ApplicationUser user = await _userManager.FindByIdAsync(uParoisse.UserId);
					_userManager.DeleteAsync(user);
					_db.Remove(uParoisse);
				}
				_db.Remove(paroisse);
				_db.SaveChanges();
			}
		}
	}
}