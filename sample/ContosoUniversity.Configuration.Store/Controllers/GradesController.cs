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

        [AbacAuthorize("Example")]
        public IActionResult Complex([AbacParameter]string tenant)
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
