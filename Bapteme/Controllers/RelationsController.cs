using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bapteme.Data;
using Bapteme.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Bapteme.Controllers
{
	[Authorize()]
	[Authorize(Policy = "IsAdmin")]
	public class RelationsController : MyController
    {
		protected readonly ApplicationDbContext _dbUsers;

		public RelationsController(BaptemeDataContext db, UserManager<ApplicationUser> userManager, ApplicationDbContext dbUsers) : base(db, userManager)
		{
			_dbUsers = dbUsers;
		}

        // GET: Relations
        public async Task<IActionResult> Index()
        {
            return View(await _dbUsers.Relations.ToListAsync());
        }

        // GET: Relations/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var relation = await _dbUsers.Relations
                .SingleOrDefaultAsync(m => m.Id == id);
            if (relation == null)
            {
                return NotFound();
            }

            return View(relation);
        }

        // GET: Relations/Create
        public IActionResult Create()
        {
			ViewBag.ListUsers = new SelectList(_userManager.Users.ToArray(), "Id", "DisplayName");
            return View();
        }

        // POST: Relations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ParentId,ChildId,RelationType,DateCreated,DateModified")] Relation relation)
        {
            if (ModelState.IsValid)
            {
                relation.Id = Guid.NewGuid();
                _dbUsers.Add(relation);
                await _dbUsers.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(relation);
        }

        // GET: Relations/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var relation = await _dbUsers.Relations.SingleOrDefaultAsync(m => m.Id == id);
            if (relation == null)
            {
                return NotFound();
            }
			ViewBag.ListUsers = new SelectList(_userManager.Users.ToArray(), "Id", "DisplayName");
			return View(relation);
        }

        // POST: Relations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,ParentId,ChildId,RelationType,DateCreated,DateModified")] Relation relation)
        {
            if (id != relation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dbUsers.Update(relation);
                    await _dbUsers.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RelationExists(relation.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(relation);
        }

        // GET: Relations/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var relation = await _dbUsers.Relations
                .SingleOrDefaultAsync(m => m.Id == id);
            if (relation == null)
            {
                return NotFound();
            }

            return View(relation);
        }

        // POST: Relations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var relation = await _dbUsers.Relations.SingleOrDefaultAsync(m => m.Id == id);
            _dbUsers.Relations.Remove(relation);
            await _dbUsers.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool RelationExists(Guid id)
        {
            return _dbUsers.Relations.Any(e => e.Id == id);
        }
    }
}
