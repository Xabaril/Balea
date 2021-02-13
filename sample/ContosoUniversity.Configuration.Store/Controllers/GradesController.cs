using Balea.Authorization.Abac;
using ContosoUniversity.Configuration.Store.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContosoUniversity.Configuration.Store.Controllers
{
    public class GradesController : Controller
    {
        [Authorize(Policies.GradesRead)]
        public IActionResult Read()
        {
            return View();
        }

        [Authorize(Policies.GradesEdit)]
        public IActionResult Validate()
        {
            return View();
        }

        [HttpPost]
        [AbacAuthorize("Validate")]
        public IActionResult Validate([AbacParameter] int value)
        {
            return View();
        }

        [Authorize(Policies.GradesEdit)]
        public IActionResult Edit()
        {
            return View();
        }
    }
}
