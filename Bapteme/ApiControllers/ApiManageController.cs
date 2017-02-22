using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Bapteme.Controllers;
using Bapteme.Data;
using Microsoft.AspNetCore.Identity;
using Bapteme.Models;
using Microsoft.AspNetCore.Authorization;
using Bapteme.Models.ManageViewModels;
using Microsoft.EntityFrameworkCore;

namespace Bapteme.ApiControllers
{
    [Produces("application/json")]
    [Route("api/ApiManager")]
	[Authorize]
	public class ApiManageController : MyController
    {
		public ApiManageController(BaptemeDataContext db, UserManager<ApplicationUser> userManager) : base(db, userManager)
		{
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Put([FromRoute] string id, [FromForm] IndexViewModel profile)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if (id != profile.Id)
			{
				return BadRequest();
			}

			ApplicationUser connectedUser = await _userManager.GetUserAsync(HttpContext.User);
			if (connectedUser.Id == profile.Id || verifyUserRightOnProfile(connectedUser.Id, profile.Id))
			{
				ApplicationUser user = await _userManager.FindByIdAsync(profile.Id);
				user.FirstName = profile.FirstName;
				user.LastName = profile.LastName;
				user.TelephoneMobile = profile.TelephonMobile;
				user.BirthName = profile.BirthName;
				user.BirthDate = profile.BirthDate;

				await _userManager.UpdateAsync(user);
			}

			return NoContent();
		}

		private bool verifyUserRightOnProfile(string connectedUserId, string profilId)
		{
			List<Guid> list_paroisseIdUserIsAutorize = _db.UserParoisse.Where(x => x.UserId == connectedUserId && x.Role == role.Manager).Select(x => x.ParoisseId).ToList();
			List<Guid> list_paroisseIdProfilIsContact = _db.UserParoisse.Where(x => x.UserId == profilId).Select(x => x.ParoisseId).ToList();

			return list_paroisseIdProfilIsContact.Intersect(list_paroisseIdUserIsAutorize).Count() > 0;
		}
	}
}