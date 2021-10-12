using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectTracker.Database;
using ProjectTracker.Database.Concrete;

namespace ProjectTracker.Controllers
{
    public class SupportsController : Controller
    {
        private readonly AppDbContext _context;

        public SupportsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Supports
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var supportList = await _context.Supports.Include(d => d.Project).Include(d => d.User).OrderByDescending(x=> x.CreateDate).ToListAsync();
            
            return View(supportList);
        }

        // GET: Supports/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var support = await _context.Supports
                .Include(s => s.User)
                .Include(s=> s.Project)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (support == null)
            {
                return NotFound();
            }

            return View(support);
        }

        // GET: Supports/Create
        [Authorize]
        public IActionResult Create()
        {
            List<Project> Projects = _context.Projects.ToList();
            {

            };
            ViewBag.Projects = new SelectList(Projects, "Id", "ProjectName");
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Name");
            return View();
        }

        // POST: Supports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProjectId,User,Demander,Execution,Situation")] Support support)
        {
            if (ModelState.IsValid)
            {
                support.CreateDate = DateTime.Now;
                support.User = _context.Users.FirstOrDefault(u => u.UserId == support.User.UserId);
                _context.Add(support);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Name", support.User);
            return View(support);
        }

        // GET: Supports/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var support = await _context.Supports.FindAsync(id);
            if (support == null)
            {
                return NotFound();
            }

            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Name", support.User);
            return View(support);
        }

        // POST: Supports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProjectId,User,User.UserName,Demander,Execution,Situation,CreateDate")] Support support)
        {
            if (id != support.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    support.User = _context.Users.FirstOrDefault(u => u.UserId == support.User.UserId);
                    _context.Update(support);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupportExists(support.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Name", support.User);
            return View(support);
        }

        // GET: Supports/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var support = await _context.Supports
                .Include(s => s.User)
                .Include(s=> s.Project)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (support == null)
            {
                return NotFound();
            }

            return View(support);
        }

        // POST: Supports/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var support = await _context.Supports.FindAsync(id);
            _context.Supports.Remove(support);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SupportExists(int id)
        {
            return _context.Supports.Any(e => e.Id == id);
        }
    }
}
