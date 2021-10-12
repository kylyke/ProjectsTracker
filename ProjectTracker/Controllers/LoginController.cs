using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjectTracker.Database;
using ProjectTracker.Models;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Collections.Generic;
using ProjectTracker.Database.Concrete;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace ProjectTracker.Controllers
{
    public class LoginController : Controller
    {

        private readonly AppDbContext _context;

        public LoginController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet] 
        public IActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(w=> w.UserName.Equals(username) && w.Password.Equals(password));
            if (user!=null)
            {
                HttpContext.Session.SetInt32("id",user.UserId);
                HttpContext.Session.SetString("fullname",user.Name+" "+user.Surname);
                HttpContext.Session.SetString("fullname2", user.Name);
                HttpContext.Session.SetString("Role", user.Role);
            
                var claims = new List<Claim>
                {
                  new Claim(ClaimTypes.Name,username)
                };
                var claimsIdentity = new ClaimsIdentity(claims, "Login");
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                return RedirectToAction("Index","Home");
            }

            return RedirectToAction("Index", "Home");
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Login");
        }
    }
}
