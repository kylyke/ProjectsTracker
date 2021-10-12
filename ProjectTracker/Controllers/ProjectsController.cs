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
    public class ProjectsController : Controller
    {
        private readonly AppDbContext _context;

        public ProjectsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Projects
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var appDbContext = await _context.Projects.Include(p => p.User).OrderByDescending(x=> x.StartDate).ToListAsync();
            return View(appDbContext.ToList());
        }

        // GET: Projects/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.Include(p => p.User).FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // GET: Projects/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewBag.UserId = new SelectList(_context.Users, "UserId", "Name");
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProjectName,StartDate,UpdateDate,FinishDate,UserId,CompanyAbout,ProjectData,PhoneNumber,UpdateAgreement,UpdateAgreementStartDate,UpdateAgreementFinisDate,MaintenanceAgreement,MaintenanceAgreementStartDate,MaintenanceAgreementFinishDate,ConnectionAdress")] Project project)
        {
            if (ModelState.IsValid)
            {

                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.UserId = new SelectList(_context.Users, "UserId", "Name", project.UserId);
            return View(project);
        }

        // GET: Projects/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            List<SelectListItem> boolList = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Evet", Value = "True" },
                new SelectListItem { Text = "Hayır", Value = "False" }



            };




            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            ViewBag.boolList = boolList;
            ViewBag.UserId = new SelectList(_context.Users, "UserId", "Name", project.UserId);
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProjectName,StartDate,UpdateDate,FinishDate,UserId,CompanyAbout,ProjectData,PhoneNumber,UpdateAgreement,UpdateAgreementStartDate,UpdateAgreementFinisDate,MaintenanceAgreement,MaintenanceAgreementStartDate,MaintenanceAgreementFinishDate,ConnectionAdress")] Project project)
        {
            if (id != project.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(project);
                    project.UpdateDate = DateTime.Now;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.Id))
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
            ViewBag.UserId = new SelectList(_context.Users, "UserId", "Name", project.UserId);
            return View(project);
        }

        // GET: Projects/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {

            var check = _context.Demands.Where(x=> x.ProjectId==id).FirstOrDefault();
            var check2 = _context.Supports.Where(x => x.ProjectId == id).FirstOrDefault();



            try
            {
                if (check != null  && !String.IsNullOrEmpty(check.ProjectId.ToString()) && check.ProjectId == id)
                {
                    return RedirectToAction(nameof(Index));
                }
                else if(check2 != null && !String.IsNullOrEmpty(check2.ProjectId.ToString()) && check2.ProjectId == id)
                {
                    


                    

                }
                else{
                    var project = await _context.Projects
                                     .Include(p => p.User)
                                     .FirstOrDefaultAsync(m => m.Id == id);
                    return View(project);
                }
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));



        }

        // POST: Projects/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }
        //public IActionResult IndexDemand()
        //{
        //    var IndexDemand = _context.Projects.ToList();


        //    return View(IndexDemand);




        //}
    }
}
