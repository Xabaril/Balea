using Balea.Authorization.Abac;
using ContosoUniversity.EntityFrameworkCore.Store.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContosoUniversity.Configuration.Store.Controllers
{
    public class GradesController : Controller
    {
        [Authorize(Permissions.GradesRead)]
        public IActionResult Read()
        {
            return View();
        }

        [Authorize(Permissions.GradesEdit)]
        public IActionResult Edit()
        {
            return View();
        }
    }
}
