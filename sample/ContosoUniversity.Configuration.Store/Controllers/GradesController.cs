using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ContosoUniversity.Configuration.Store.Models;
using Balea.Authorization;
using Balea.DSL.AspNetCore;

namespace ContosoUniversity.Configuration.Store.Controllers
{
    public class GradesController : Controller
    {
        [Authorize(Policies.GradesRead)]
        public IActionResult Read()
        {
            return View();
        }

        [AbacPolicy("Example")]
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
