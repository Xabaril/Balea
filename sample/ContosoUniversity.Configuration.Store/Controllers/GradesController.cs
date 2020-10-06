using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ContosoUniversity.Configuration.Store.Models;

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
        public IActionResult Edit()
        {
            return View();
        }
    }
}
