using Microsoft.AspNetCore.Mvc;
using ProjectTracker.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTracker.Controllers
{
    public class ReportController : Controller
    {
        public IActionResult Index()
        {
            return View();
            
        }
    }
}
