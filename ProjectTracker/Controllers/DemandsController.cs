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
    public class DemandsController : Controller
    {
        private readonly AppDbContext _context;

        public DemandsController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var demandList = await _context.Demands.
                Where(d => d.Status == 1 && d.Situation == false).
                Include(d => d.Project).
                Include(d => d.User).
                OrderByDescending(x=> x.CreateDate).
                ToListAsync();
            return View(demandList);


        }
        [Authorize]
        public async Task<IActionResult> Index2()
        {
            var demandList =
                await _context.Demands.
                Include(d => d.Project).Include(d => d.User).
                Where(d => d.Status == 0).
                 OrderByDescending(x => x.CreateDate).
                ToListAsync();
            return View(demandList);


        }
        [Authorize]
        public async Task<IActionResult> TalepRed()
        {
            var demandList =
                await _context.Demands.
                Include(d => d.Project).
                Include(d => d.User).
                Where(d => d.Status == 2).
                OrderByDescending(x => x.CreateDate).
                ToListAsync();
            // await getUsers(_context, demandList);
            return View(demandList);


        }
        [Authorize]
        public async Task<IActionResult> TalepTamamlanan()
        {
            var demandList =
                await _context.Demands.
                Include(d => d.Project).
                Include(d => d.User).
               
                Where(d => d.Situation == true && d.Status == 1).
                 OrderByDescending(x => x.CreateDate).
                ToListAsync();
            // await getUsers(_context, demandList);
            return View(demandList);


        }

        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {


            if (id == null)
            {
                return NotFound();
            }

            var demand = await _context.Demands.Include(d => d.User).Include(d => d.Project).FirstOrDefaultAsync(m => m.Id == id);

            if (demand == null)
            {
                return NotFound();
            }

            return View(demand);
        }

        // GET: Demands/Create
        [Authorize]
        public IActionResult Create(int id)
        {
            List<Project> Projects = _context.Projects.ToList();
            {

            };
            ViewBag.Projects = new SelectList(Projects, "Id", "ProjectName");

            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Name");
            return View();
        }

        // POST: Demands/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CompanyAbout,CreateDate,Demander,Execution,FinishDate,Data,Situation,ProjectId,User")] Demand demand)
        {

            if (ModelState.IsValid)
            {
                //ViewBag.Projects = _context.Demands.Where(x => x.Project.Id == demand.Project.Id).FirstOrDefault();
                //ViewBag.Projects = demand.Project;
                demand.User = _context.Users.FirstOrDefault(u => u.UserId == demand.User.UserId);
                _context.Add(demand);
                demand.Status = 0;
                demand.CreateDate = DateTime.Now;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Name", demand.User);
            return View(demand);
        }

        // GET: Demands/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            List<SelectListItem> statusList = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Bekliyor", Value = "0" },
                new SelectListItem { Text = "Onaylandı", Value = "1" },
                new SelectListItem { Text = "Reddedildi", Value = "2" }



            };




            if (id == null)
            {
                return NotFound();
            }

            var demand = await _context.Demands.FindAsync(id);

            if (demand == null)
            {
                return NotFound();
            }
            ViewBag.statusList = statusList;
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Name", demand.User);

            return View(demand);
        }

        // POST: Demands/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CompanyAbout,CreateDate,Demander,Execution,FinishDate,Data,Situation,ProjectId,User,Status")] Demand demand)
        {
            List<SelectListItem> statusList = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Bekliyor", Value = "0" },
                new SelectListItem { Text = "Onaylandı", Value = "1" },
                new SelectListItem { Text = "Reddedildi", Value = "2" }


            };
            ViewBag.statusList = statusList;

            if (id != demand.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    demand.Status = 1;
                    demand.User = _context.Users.FirstOrDefault(u => u.UserId == demand.User.UserId);
                    _context.Update(demand);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DemandExists(demand.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Demands");
            }

            ViewBag.UserId = new SelectList(_context.Users, "UserId", "Name", demand.User);
            return View(demand);
        }
        [Authorize]
        public async Task<IActionResult> Edit2(int? id)
        {
            List<SelectListItem> statusList = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Bekliyor", Value = "0" },
                new SelectListItem { Text = "Onaylandı", Value = "1" },
                new SelectListItem { Text = "Reddedildi", Value = "2" }


            };





            if (id == null)
            {
                return NotFound();
            }

            var demand = await _context.Demands.FindAsync(id);

            var Bakim = _context.Projects.Where(x => x.Id == demand.ProjectId).FirstOrDefault();

            
            var date = Convert.ToDateTime(DateTime.Now.ToString("yyyy-dd-MMM"));
            

            ViewBag.MaintenanceAgreementFinishDate = Bakim.MaintenanceAgreementFinishDate - date;


            






            if (demand == null)
            {
                return NotFound();
            }
            ViewBag.statusList = statusList;
            ViewBag.UserId = new SelectList(_context.Users, "UserId", "Name", demand.User);

            return View(demand);
        }

        // POST: Demands/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit2(int id, [Bind("Id,CompanyAbout,CreateDate,Demander,Execution,FinishDate,Data,Situation,ProjectId,User,Status")] Demand demand)
        {
            List<SelectListItem> statusList = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Bekliyor", Value = "0" },
                new SelectListItem { Text = "Onaylandı", Value = "1" },
                new SelectListItem { Text = "Reddedildi", Value = "2" }


            };
            ViewBag.statusList = statusList;

            if (id != demand.Id)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                try
                {
                    demand.User = _context.Users.FirstOrDefault(u => u.UserId == demand.User.UserId);
                    _context.Update(demand);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DemandExists(demand.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index2", "Demands");
            }

            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Name", demand.User);
            return View(demand);
        }

        // GET: Demands/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var demand = await _context.Demands
                .Include(d => d.User).Include(x => x.Project)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (demand == null)
            {
                return NotFound();
            }

            return View(demand);
        }

        // POST: Demands/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var demand = await _context.Demands.FindAsync(id);
            _context.Demands.Remove(demand);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete2(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var demand = await _context.Demands
                .Include(d => d.User).Include(x => x.Project)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (demand == null)
            {
                return NotFound();
            }

            return View(demand);
        }

        // POST: Demands/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete2")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete2Confirmed(int id)
        {
            var demand = await _context.Demands.FindAsync(id);
            _context.Demands.Remove(demand);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index2));
        }

        private bool DemandExists(int id)
        {
            return _context.Demands.Any(e => e.Id == id);
        }

    }

}
