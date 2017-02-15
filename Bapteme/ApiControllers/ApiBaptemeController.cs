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
	[Route("api/bapteme/")]
    public class ApiBaptemeController : MyController
    {
				 
		public ApiBaptemeController(BaptemeDataContext db, UserManager<ApplicationUser> userManager) : base(db, userManager)
		{
		}

		[Route("")]
		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Post([FromForm] AddBaptemeViewModel new_bapteme)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			Adresse new_adress = new Adresse()
			{
				Numero = new_bapteme.Numero,
				Rue = new_bapteme.Rue,
				TelephoneFix = new_bapteme.TelephoneFix,
				CP = new_bapteme.CP,
				Ville = new_bapteme.Ville,
				Complement = new_bapteme.Complement,
			};
			await _db.AddAsync(new_adress);

			ApplicationUser new_user = new ApplicationUser() {
				FirstName = new_bapteme.FirstName,
				LastName = new_bapteme.LastName,
				BirthDate = new_bapteme.BirthDate,
				UserName = Guid.NewGuid().ToString(),
				CelebrationId = new_bapteme.CelebrationId,
				TelephoneMobile = new_bapteme.TelephoneMobile,
				Email = new_bapteme.Email,
				IdAdress = new_adress.Id
			};
			await _userManager.CreateAsync(new_user);
			Celebration celebration = await _db.Celebrations.FindAsync(new_bapteme.CelebrationId);
			Clocher clocher = await _db.Clochers.Where(x => x.Id == celebration.ClocherId).FirstAsync();
			UserParoisse uParoisse = new UserParoisse() { UserId = new_user.Id, ParoisseId = clocher.ParoisseId, Role=role.Contact };
			await _db.AddAsync(uParoisse);
			await _db.SaveChangesAsync();
			return Ok(new_user.Id);
		}

		[Route("")]
		[Authorize]
		[HttpDelete]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(string userId)
		{
			ApplicationUser user = await _userManager.FindByIdAsync(userId);
			user.CelebrationId = Guid.Empty;
			await _userManager.UpdateAsync(user);
			return Ok();
		}

		[Route("GetBaptemeForCelebration")]
		[Authorize]
		[HttpGet]
		public async Task<IActionResult> GetBaptemeForCelebration(Guid CelebrationId)
		{
			List<ApplicationUser> list_baptemes = await _userManager.Users.Where(x => x.CelebrationId == CelebrationId).ToListAsync();
			return PartialView("_indexBaptemes", list_baptemes);
		}

	}
}
