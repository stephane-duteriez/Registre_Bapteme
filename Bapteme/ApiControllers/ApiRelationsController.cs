using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bapteme.Data;
using Microsoft.AspNetCore.Authorization;
using Bapteme.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Bapteme.Controllers;
using Bapteme.Models.CustomViewModels;

namespace Bapteme.ApiControllers
{
    public class ApiRelationsController : MyController
    {
		protected readonly ApplicationDbContext _dbUsers;

		public ApiRelationsController(BaptemeDataContext db, UserManager<ApplicationUser> userManager, ApplicationDbContext dbUsers) : base(db, userManager)
		{
			_dbUsers = dbUsers;
		}

		[Route("")]
		[Authorize]
		[HttpGet]
		public async Task<IActionResult> Get(string ChildId)
		{
			List<ApplicationUser> list_contacts = await _dbUsers.Relations.Where(x => x.ChildId == ChildId).Select(x => x.Parent).ToListAsync();
			return PartialView("_listContacts", list_contacts);
		}
	}
}
