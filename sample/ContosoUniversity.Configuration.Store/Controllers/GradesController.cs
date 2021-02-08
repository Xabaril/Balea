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

<<<<<<< HEAD
        [AbacAuthorize("Example")]
        public IActionResult Complex([AbacParameter]int value)
=======
        [AbacAuthorize("ValidateGrades")]
        public IActionResult Validate([AbacParameter]int value)
>>>>>>> 2aed65376472316ab1db2f3765da7e6fce2e0ea1
        {
            return View("Read");
        }

        [Authorize(Policies.GradesEdit)]
        public IActionResult Edit()
        {
            return View();
        }
    }
}
