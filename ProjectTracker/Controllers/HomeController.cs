using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjectTracker.Database;
using ProjectTracker.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Dapper;
using System;


namespace ProjectTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public void Chart()
        {


            string conStr = "Server=91.93.0.23,14333;Database=PROJECT_TRACKER; Persist Security Info=True; User ID=sa;Password=Trbarkod34*";
            using (var sqlConnection = new SqlConnection(conStr))
            {
                try
                {

                    string selectDemandCount = "select DAY(CreateDate) as 'Day', COUNT(DAY(CreateDate)) as 'Count' from Demands where DATEPART(MONTH,CreateDate)=DATEPART(MONTH,GETDATE()) group by DAY(CreateDate)";
                    string selectSupportCount = "select DAY(CreateDate) as 'Day', COUNT(DAY(CreateDate)) as 'Count' from Supports where DATEPART(MONTH,CreateDate)=DATEPART(MONTH,GETDATE()) group by DAY(CreateDate)";
                    
                    List<ChartModel> results = sqlConnection.Query<ChartModel>(selectDemandCount).ToList();
                    List<ChartModel> results2 = sqlConnection.Query<ChartModel>(selectSupportCount).ToList();
                    List<ChartModel> missingResults = new List<ChartModel>();
                    List<ChartModel> missingResults2 = new List<ChartModel>();

                    DateTime currentDate = DateTime.Now;
                    int howManyDaysInThatMonth = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);

                    for (int i = 1; i <= howManyDaysInThatMonth; i++)
                    {
                        if (!results.Any(x => x.Day.Equals(i)))
                            missingResults.Add(new ChartModel() { Day = i, Count = 0 });
                    }


                    for (int i = 1; i <= howManyDaysInThatMonth; i++)
                    {
                        if (!results2.Any(x => x.Day.Equals(i)))
                            missingResults2.Add(new ChartModel() { Day = i, Count = 0 });
                    }

                    results.AddRange(missingResults);
                    results2.AddRange(missingResults2);

                    results = results.OrderBy(x => x.Day).ToList();
                    results2 = results2.OrderBy(x => x.Day).ToList();



                    ViewBag.DemandCount = results;
                    ViewBag.SupportCount = results2;

                   
                    ViewBag.bekleyendemandCount = _context.Demands.Where(x => x.Status == 0).Count();
                    ViewBag.bitendemandCount = _context.Demands.Where(x => x.Status==1 && x.Situation==true ).Count();
                    ViewBag.totalSupport = _context.Supports.Count();
                    ViewBag.totalDemand = _context.Demands.Where(x=> x.Status!=2).Count();



                }
                catch
                {
                    
                }
            }




        }

       

        [Authorize]
        public IActionResult Index()
        {
            Chart();
            return View();
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }
        [Authorize]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
