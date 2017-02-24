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
	[Route("api/Relations/")]
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

		[Route("")]
		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Post([FromForm] AddContactViewModel new_contact)
		{
			Guid adresseId;

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if (!new_contact.useSameAdresse)
			{
				Adresse new_adresse = new Adresse()
				{
					Numero = new_contact.Numero,
					Rue = new_contact.Rue,
					TelephoneFix = new_contact.TelephoneFix,
					CP = new_contact.CP,
					Ville = new_contact.Ville,
					Complement = new_contact.Complement,
				};
				await _db.AddAsync(new_adresse);
				adresseId = new_adresse.Id;
			} else
			{
				ApplicationUser user_bapteme = await _userManager.FindByIdAsync(new_contact.ChildId);
				adresseId = user_bapteme.IdAdress;
			}

			ApplicationUser new_user = new ApplicationUser()
			{
				FirstName = new_contact.FirstName,
				LastName = new_contact.LastName,
				BirthDate = new_contact.BirthDate,
				UserName = Guid.NewGuid().ToString(),
				TelephoneMobile = new_contact.TelephoneMobile,
				Email = new_contact.Email,
				IdAdress = adresseId
			};
			await _userManager.CreateAsync(new_user);

			Relation new_relation = new Relation
			{
				ChildId = new_contact.ChildId,
				ParentId = new_user.Id
			};
			await _dbUsers.AddAsync(new_relation);

			UserParoisse userParoisse = await _db.UserParoisse.Where(x => x.UserId == new_contact.ChildId).FirstAsync();

			UserParoisse uParoisse = new UserParoisse() { UserId = new_user.Id, ParoisseId = userParoisse.ParoisseId, Role = role.Contact };
			await _db.AddAsync(uParoisse);
			await _db.SaveChangesAsync();
			await _dbUsers.SaveChangesAsync();

			List<ApplicationUser> list_contacts = await _dbUsers.Relations.Where(x => x.ChildId == new_contact.ChildId).Select(x => x.Parent).ToListAsync();
			return PartialView("_listContacts", list_contacts);
		}

		[Route("")]
		[Authorize]
		[ValidateAntiForgeryToken]
		[HttpDelete]
		public async Task<IActionResult> Delete(string ContactId, string RelationId)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			Relation relation = await _dbUsers.Relations.SingleOrDefaultAsync(m => m.ChildId == ContactId && m.ParentId==RelationId);
			if (relation == null)
			{
				return NotFound();
			}

			_dbUsers.Remove(relation);
			await _dbUsers.SaveChangesAsync();

			return Ok();
		}
	}
}
