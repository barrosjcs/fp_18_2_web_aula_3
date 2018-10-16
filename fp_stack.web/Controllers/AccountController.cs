using fp_stack.web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace fp_stack.web.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Perguntas");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            if (model.UserName == "jefferson" && model.Password == "123")
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.UserName)
                };
                var id = new ClaimsIdentity(claims, "password");
                var principal = new ClaimsPrincipal(id);

                await HttpContext.SignInAsync("app", principal, 
                    new AuthenticationProperties() { IsPersistent = model.IsPersistent });

                return RedirectToAction("Index", "Perguntas");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logoff()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}