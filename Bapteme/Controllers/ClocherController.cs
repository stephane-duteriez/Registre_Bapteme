using Bapteme.Data;
using Bapteme.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bapteme.Controllers
{
	[Route("clocher")]
	public class ClocherController : MyController
	{

		public ClocherController(BaptemeDataContext db, UserManager<ApplicationUser> userManager) : base(db, userManager)
		{
		}

		[Authorize()]
		[Authorize(Policy = "IsAdmin")]
		public async Task<IActionResult> Index()
		{
			List<Clocher> clochers = await _db.Clochers.Include("Paroisse").OrderBy(x => x.Name).ToListAsync();
			return View(clochers);
		}

		[Authorize]
		[HttpGet, Route("create")]
		public IActionResult Create()
		{
			ViewBag.ParoissesName = new SelectList(_db.Paroisses.OrderBy(x => x.Name).ToArray(), "Id", "Name");
			return View();
		}

		[HttpPost, Route("create")]
		public IActionResult Create(Clocher new_clocher)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}

			_db.Clochers.Add(new_clocher);
			_db.SaveChanges();

			return RedirectToAction("Post", "Clocher", new { key = new_clocher.Key });
		}

		[Route("{keyParoisse}/{keyClocher}")]
		public IActionResult Post(string keyParoisse, string keyClocher)
		{
			Clocher clocher = _db.Clochers.Include(c => c.Paroisse).Include(c=>c.Celebrations).Include(c=>c.Permanences).FirstOrDefault(x => x.Key == keyClocher && x.Paroisse.Key==keyParoisse);
			ViewBag.roles = FindRole(GetCurrentUserAsync().Result, clocher.ParoisseId).Result;
			return View(clocher);
		}

		[Route("edit/{keyParoisse}/{keyClocher}")]
		[Authorize]
		[HttpGet]
		public async Task<IActionResult> Edit(string keyParoisse, string keyClocher)
		{
			Clocher clocher = _db.Clochers.Include(c => c.Paroisse).Include(c=>c.Permanences).FirstOrDefault(x => x.Key == keyClocher && x.Paroisse.Key == keyParoisse);
			List<role> myRoles = await FindRole(await GetCurrentUserAsync(), clocher.ParoisseId);

			if (myRoles.Contains(role.Administrateur))
			{
				ViewBag.roles = myRoles;
				return View(clocher);
			}
			return View("notAllowed");
		}
	}
}
