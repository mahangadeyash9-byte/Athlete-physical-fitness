using AthleteFitnessApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AthleteFitnessApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly AthleteFitnessApp.Data.ApplicationDbContext _context;

        public HomeController(AthleteFitnessApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
