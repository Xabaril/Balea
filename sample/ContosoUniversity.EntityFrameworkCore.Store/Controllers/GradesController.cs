﻿using Balea.Authorization.Abac;
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

        [HttpGet]
        [Authorize(Permissions.GradesEdit)]
        public IActionResult Validate()
        {
            return View();
        }

        [HttpPost]
        [AbacAuthorize(Policies.ValidateGrades)]
        public IActionResult Validate([AbacParameter(Name = "Value")] int value)
        {
            return View();
        }
    }
}
