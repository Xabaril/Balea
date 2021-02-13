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

        [AbacAuthorize("Validate")]
        [HttpPost]
        public IActionResult Validate([AbacParameter] string value)
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
