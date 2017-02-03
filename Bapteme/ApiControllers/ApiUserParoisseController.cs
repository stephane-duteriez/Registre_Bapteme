using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bapteme.Data;
using Microsoft.AspNetCore.Authorization;
using Bapteme.Models;
using Microsoft.EntityFrameworkCore;
using Bapteme.Controllers;
using Microsoft.AspNetCore.Identity;

namespace Bapteme.ApiControllers
{
	[Route("api/userparoisse/")]
    public class ApiUserParoisseController : MyController
    {
		 
		public ApiUserParoisseController(BaptemeDataContext db, UserManager<ApplicationUser> userManager) : base(db, userManager)
		{
		}

		[Route("")]
		[Authorize(Policy = "IsAdmin")]
		[HttpGet]
		public List<UserParoisse> GetAll()
		{
			return _db.UserParoisse.ToList();
		}

		[Route("")]
		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Post([FromForm] UserParoisse new_userParoisse)
		{
			//if (!ModelState.IsValid)
			//{
			//	return BadRequest(ModelState);
			//}
			await _db.UserParoisse.AddAsync(new_userParoisse);
			await _db.SaveChangesAsync();
			List<UserParoisse> l_userParoisse = await _db.UserParoisse.Include("Paroisse").Where(p => p.UserId == new_userParoisse.UserId).ToListAsync();
			return PartialView("_indexUserParoisse", l_userParoisse);
		}

		[Route("")]
		[Authorize]
		[HttpDelete]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(Guid uParoisseId)
		{
			UserParoisse uParoisse_to_delete = await _db.UserParoisse.FindAsync(uParoisseId);
			_db.UserParoisse.Remove(uParoisse_to_delete);
			await _db.SaveChangesAsync();
			List<UserParoisse> l_userParoisse = await _db.UserParoisse.Include("Paroisse").Where(p => p.UserId == uParoisse_to_delete.UserId).ToListAsync();
			return PartialView("_indexUserParoisse", l_userParoisse);
		}

	}
}
