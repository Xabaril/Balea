using System.Diagnostics;
using ContosoUniversity.EntityFrameworkCore.Store.Infrastructure;
using ContosoUniversity.EntityFrameworkCore.Store.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContosoUniversity.Configuration.Store.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Blog()
        {
            return View();
        }

        public IActionResult Teachers()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [Authorize]
        public IActionResult Profile()
        {
            return View();
        }

        [AuthorizeRoles(nameof(Roles.Teacher), nameof(Roles.Substitute), nameof(Roles.Student))]
        public IActionResult Courses()
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
