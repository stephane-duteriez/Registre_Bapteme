using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bapteme.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Bapteme.Models;

namespace Bapteme.Controllers
{
    public class HomeController : MyController
    {
		protected readonly ApplicationDbContext _dbUsers;

		public HomeController(BaptemeDataContext db, UserManager<ApplicationUser> userManager, ApplicationDbContext dbUsers) : base(db, userManager)
		{
			_dbUsers = dbUsers;
        }
		public async Task<IActionResult> Index()
		{
			List<Paroisse> list_paroisse;
			if (User.Identity.IsAuthenticated)
			{
				ApplicationUser user = await GetCurrentUserAsync();
				list_paroisse = await _db.UserParoisse.Where(x => x.UserId == user.Id).Select(x => x.Paroisse).Distinct().ToListAsync();
				
				if (list_paroisse.Count > 1)
				{
					return View("~/Views/Paroisse/Index.cshtml", list_paroisse);
				}
				else if(list_paroisse.Count==1)
				{
					return RedirectToAction("Post", "Paroisse", new { key = list_paroisse[0].Key });
				}
			}
			list_paroisse = _db.Paroisses.ToList();
			return View(list_paroisse);
		}

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult InitDatabase(string returnUrl = null)
        {
            _db.Database.Migrate();
			_dbUsers.Database.Migrate();
            return RedirectToAction("Index", "Home");
        }
    }
}
