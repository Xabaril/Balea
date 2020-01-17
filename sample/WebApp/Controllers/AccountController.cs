using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Volvoreta;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            switch (model.UserName)
            {
                case "john":
                    identity.AddClaim(new Claim(Claims.Subject, "1"));
                    identity.AddClaim(new Claim(ClaimTypes.Name, "john"));
                    identity.AddClaim(new Claim(ClaimTypes.Role, "teacher"));
                    break;
                case "marie":
                    identity.AddClaim(new Claim(Claims.Subject, "2"));
                    identity.AddClaim(new Claim(ClaimTypes.Name, "john"));
                    identity.AddClaim(new Claim(ClaimTypes.Role, "teacher"));
                    break;
                case "mark":
                    identity.AddClaim(new Claim(Claims.Subject, "3"));
                    identity.AddClaim(new Claim(ClaimTypes.Name, "mark"));
                    identity.AddClaim(new Claim(ClaimTypes.Role, "custodian"));
                    break;
                default:
                    identity.AddClaim(new Claim(Claims.Subject, "4"));
                    identity.AddClaim(new Claim(ClaimTypes.Name, model.UserName));
                    identity.AddClaim(new Claim(ClaimTypes.Role, "graduate"));
                    break;
            }

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity),
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(10)
                });

            return RedirectToAction(nameof(HomeController.Private), "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
