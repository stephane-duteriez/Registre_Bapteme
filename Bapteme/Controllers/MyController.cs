using Bapteme.Data;
using Bapteme.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bapteme.Controllers
{
    public class MyController : Controller
    {
		protected readonly BaptemeDataContext _db;
		protected readonly UserManager<ApplicationUser> _userManager;

		public MyController(BaptemeDataContext db, UserManager<ApplicationUser> userManager)
		{
			_db = db;
			_userManager = userManager;
		}

		protected async Task<ApplicationUser> GetCurrentUserAsync()
		{
			return await _userManager.GetUserAsync(HttpContext.User);
		}

		protected async Task<List<role>> FindRole(ApplicationUser user, Paroisse paroisse)
		{
			if (user != null)
			{
				return await _db.UserParoisse.Where(x => x.UserId == user.Id).Where(x => x.ParoisseId == paroisse.Id).Select(x=>x.Role).ToListAsync();
			} else
			{
				return new List<role>() { role.Viewer };
			}
		}
	}
}
