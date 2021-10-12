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
    public class UsersController : Controller
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }
        [Authorize]
        [HttpPost]
        public ActionResult DeleteUser(int UserId)
        {
           
            var user = _context.Users.FirstOrDefault(x => x.UserId == UserId);
            var project =  _context.Projects.FirstOrDefault();
            ViewBag.Hata = null;
            bool result = false;

            try
            {
                if (UserId != project.UserId)
                {
                    _context.Users.Remove(user);
                    _context.SaveChanges();
                    result = true;
                }

            }
            catch(Exception Hata)
            {
                ViewBag.Hata=Hata;
            }
            
           
            return Redirect("Index");
        }
        [Authorize]
        [HttpPost]
        public ActionResult SaveUser(User user)
        {
            user.Role = "User";
            _context.Users.Add(user);
            _context.SaveChanges();
            return RedirectToAction("Index");


        }
        // GET: Users
        //[Authorize(Roles = "Admin,User")]
        [Authorize]
        public async Task<IActionResult> Index()
        {

            ViewBag.Message = null;
            var users = await _context.Users.ToListAsync();
            ViewBag.Users = users;

            return View();
        }

        // GET: Users/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Name,Surname,UserName,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {

            List<SelectListItem> roleList = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Admin", Value = "Admin" },
                new SelectListItem { Text = "User", Value = "User" }



            };
            ViewBag.Role = roleList;
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Name,Surname,UserName,Password,Role")] User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
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
            return View(user);
        }

        // GET: Users/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            var project = _context.Projects.FirstOrDefault();
            if (id == project.UserId)
            {
                return NotFound();
            }


            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
