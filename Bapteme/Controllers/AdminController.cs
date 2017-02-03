using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Bapteme.Models;
using Bapteme.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Bapteme.Models.AdminViewModels;

namespace Bapteme.Controllers
{
	[Route("admin")]
	[Authorize(Policy = "IsAdmin")]
    public class AdminController:Controller
    {
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly BaptemeDataContext _dbBapteme;
		public AdminController(BaptemeDataContext dbBapteme, UserManager<ApplicationUser> userManager)
		{
			_dbBapteme = dbBapteme;
			_userManager = userManager;
		}

		[Route("")]
		public async Task<IActionResult> Index()
		{
			IQueryable<ApplicationUser> list_users = _userManager.Users;
			List<UserParoisse> users = await _dbBapteme.UserParoisse.ToListAsync();
			return View(list_users);
		}

		[Route("edite/{UserId}")]
		public async Task<IActionResult> Edit(string UserId)
		{
			EditUserViewModel user = new EditUserViewModel();
			user.user = await _userManager.FindByIdAsync(UserId);
			user.userParoises = await _dbBapteme.UserParoisse.Where(x => x.UserId == user.user.Id).ToListAsync();
			ViewBag.Paroisses = await _dbBapteme.Paroisses.ToListAsync();
			return View(user);
		}
    }
}
