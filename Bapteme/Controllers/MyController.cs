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

		protected async Task<List<role>> FindRole(Guid paroisseId)
		{
			string userId = null;
			if (HttpContext.User.FindFirst("UserId") != null)
			{
				userId = HttpContext.User.FindFirst("UserId").Value;
			}
			if (userId != null)
			{
				return await _db.UserParoisse.Where(x => x.UserId == userId).Where(x => x.ParoisseId == paroisseId).Select(x=>x.Role).ToListAsync();
			} else
			{
				return new List<role>() { role.Viewer };
			}
		}
	}
}
