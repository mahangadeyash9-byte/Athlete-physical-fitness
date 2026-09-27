using Microsoft.AspNetCore.Mvc;
using AthleteFitnessApp.Data;
using AthleteFitnessApp.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace AthleteFitnessApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context; 

        public AccountController(ApplicationDbContext context) => _context = context;

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (_context.Users.Any(u => u.Email == model.Email))
                {
                    ModelState.AddModelError("", "Email already exists!");
                    return View(model);
                }

                var user = new User
                {
                    Username = model.Username,
                    Email = model.Email,
                    Password = model.Password
                };

                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Login");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);
            if (user != null)
            {
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("UserEmail", user.Email);
                return RedirectToAction("Dashboard", "Workout");
            }

            ModelState.AddModelError("", "Invalid credentials!");
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}