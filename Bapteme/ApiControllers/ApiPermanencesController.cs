using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bapteme.Data;
using Bapteme.Models;
using Microsoft.AspNetCore.Authorization;
using Bapteme.Controllers;
using Microsoft.AspNetCore.Identity;

namespace Bapteme.ApiControllers
{
    [Produces("application/json")]
    [Route("api/ApiPermanences")]
    public class ApiPermanencesController : MyController
    {
        public ApiPermanencesController(BaptemeDataContext db, UserManager<ApplicationUser> userManager) : base(db, userManager)
		{
		}

		// GET: api/ApiPermanences
		[HttpGet]
        public IEnumerable<Permanence> GetPermences()
        {
            return _db.Permences;
        }

        // GET: api/ApiPermanences/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPermanence([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var permanence = await _db.Permences.SingleOrDefaultAsync(m => m.Id == id);

            if (permanence == null)
            {
                return NotFound();
            }

            return Ok(permanence);
        }

        // PUT: api/ApiPermanences/5
        [HttpPut("{id}")]
		[Authorize]
		public async Task<IActionResult> PutPermanence([FromRoute] Guid id, [FromBody] Permanence permanence)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != permanence.Id)
            {
                return BadRequest();
            }

            _db.Entry(permanence).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PermanenceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

		// POST: api/ApiPermanences
		[Route("")]
		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> PostPermanence([FromForm] Permanence permanence, string DebutHours, string DebutMinutes, string FinHours, string FinMinutes)
        {
			permanence.Debut = new TimeSpan(Int32.Parse(DebutHours), Int32.Parse(DebutMinutes), 0);
			permanence.Fin = new TimeSpan(Int32.Parse(FinHours), Int32.Parse(FinMinutes), 0);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
			
            _db.Permences.Add(permanence);
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PermanenceExists(permanence.Id))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }
			List<Permanence> l_permanence = await _db.Permences.Include(x=>x.Clocher).Where(x => x.ClocherId == permanence.ClocherId).ToListAsync();
			ViewBag.roles = await FindRole(l_permanence[0].Clocher.ParoisseId);
			return PartialView("_indexPermanence", l_permanence);
        }

        // DELETE: api/ApiPermanences/5
        [HttpDelete]
		[Authorize]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeletePermanence(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var permanence = await _db.Permences.SingleOrDefaultAsync(m => m.Id == id);
            if (permanence == null)
            {
                return NotFound();
            }

            _db.Permences.Remove(permanence);
            await _db.SaveChangesAsync();

            return Ok();
        }

        private bool PermanenceExists(Guid id)
        {
            return _db.Permences.Any(e => e.Id == id);
        }
    }
}