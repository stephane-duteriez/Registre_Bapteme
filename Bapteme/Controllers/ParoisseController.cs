using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Bapteme.Models;
using Bapteme.Data;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Bapteme.Controllers 
{
    [Route("paroisse")]
    public class ParoisseController : MyController
    {

        public ParoisseController(BaptemeDataContext db, UserManager<ApplicationUser> userManager) : base(db, userManager)
        {
        }

        [Route("")]
        public async Task<IActionResult> Index()
        {
			List<Paroisse> paroisses = new List<Paroisse>();
			ApplicationUser user = await GetCurrentUserAsync();
			if (User.Identity.IsAuthenticated)
			{
				if (User.IsInRole("Admin"))
				{
					paroisses = await _db.Paroisses.OrderBy(x => x.Name).ToListAsync();
				} else
				{
					paroisses = await _db.UserParoisse.Where(x => x.UserId == user.Id).Select(x => x.Paroisse).Distinct().ToListAsync();
				}
			}
            return View(paroisses.ToArray());
        }

        [Authorize]
        [HttpGet, Route("create")]
        public IActionResult Create()
        {
            return View();
        }

		[Authorize]
		[HttpPost, Route("create")]
        public IActionResult Create(Paroisse new_paroisse)
        {
            if (!ModelState.IsValid)
                return View();
            
            _db.Paroisses.Add(new_paroisse);
            _db.SaveChanges();

            return RedirectToAction("Post", "Paroisse", new
            {
                key = new_paroisse.Key
            });
        }

        [Route("{key}")]
        public async Task<IActionResult> Post(string key)
        {
			ApplicationUser user = await GetCurrentUserAsync();
            var paroisse = _db.Paroisses.Include("Clochers").FirstOrDefault(x=>x.Key==key);
			ViewBag.roles = await FindRole(user, paroisse.Id);
			ViewBag.Celebrations = await _db.Celebrations.Where(x => x.Clocher.ParoisseId == paroisse.Id).Where(x=>x.Date >= DateTime.Now.AddDays(-7)).OrderBy(x=>x.Date).ToListAsync();
			return View(paroisse);
        }
    }
}