using AuthGuardCore.Context;
using AuthGuardCore.Entities;
using AuthGuardCore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthGuardCore.Controllers
{
    public class LoginController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly AuthGuardCoreContext _context;

        public LoginController(SignInManager<AppUser> signInManager, AuthGuardCoreContext context)
        {
            _signInManager = signInManager;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginUserViewModel model)
        {   
            var value = _context.Users.FirstOrDefault(x=> x.UserName == model.UserName);

            if(value == null)
            {
                return View();

            }

            if (value.EmailConfirmed == true)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, true, true);
                if (result.Succeeded)
                {
                    return RedirectToAction("Inbox", "Message");
                }
                return View();
            }
            return View();
        }
    }
}
