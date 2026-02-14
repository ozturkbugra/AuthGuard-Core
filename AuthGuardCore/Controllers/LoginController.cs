using AuthGuardCore.Entities;
using AuthGuardCore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthGuardCore.Controllers
{
    public class LoginController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;

        public LoginController(SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginUserViewModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password,true,true);
            if (result.Succeeded)
            {
                return RedirectToAction("Profile", "MyProfile");
            }
            return View();
        }
    }
}
