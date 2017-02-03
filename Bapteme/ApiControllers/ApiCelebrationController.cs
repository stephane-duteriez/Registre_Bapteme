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

namespace Bapteme.ApiControllers
{
	[Route("api/celebration/")]
    public class ApiCelebrationController : MyController
    {
				 
		public ApiCelebrationController(BaptemeDataContext db, UserManager<ApplicationUser> userManager) : base(db, userManager)
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
		public async Task<IActionResult> Post([FromForm] Celebration celebration)
		{
			//if (!ModelState.IsValid)
			//{
			//	return BadRequest(ModelState);
			//}
			await _db.Celebrations.AddAsync(celebration);
			await _db.SaveChangesAsync();
			Clocher clocher = await _db.Clochers.Include("Paroisse").Where(x => x.Id == celebration.ClocherId).FirstAsync();
			List<Celebration> l_celebration = await _db.Celebrations.Include("Clocher").Where(x => x.Clocher.ParoisseId == clocher.ParoisseId).Where(x=>x.Date >= DateTime.Now.AddDays(-7)).OrderBy(x=>x.Date).ToListAsync();
			return PartialView("_indexCelebrations", l_celebration);
		}

		[Route("")]
		[Authorize]
		[HttpDelete]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(Guid CelebrationId)
		{
			Celebration celebration_to_delete = await _db.Celebrations.FindAsync(CelebrationId);
			_db.Celebrations.Remove(celebration_to_delete);
			await _db.SaveChangesAsync();
			return Ok();
		}

	}
}
