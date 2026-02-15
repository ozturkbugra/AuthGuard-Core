using AuthGuardCore.Context;
using AuthGuardCore.Models;
using Microsoft.AspNetCore.Mvc;

namespace AuthGuardCore.Controllers
{
    public class ActivationController : Controller
    {
        private readonly AuthGuardCoreContext _context;

        public ActivationController(AuthGuardCoreContext context)
        {
            _context = context;
        }

        public IActionResult UserActivation()
        {
            if (TempData["ConfirmEmail"] is not string email)
                return RedirectToAction("Index", "Register");

            var model = new ConfirmEmailViewModel
            {
                Email = email
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult UserActivation(ConfirmEmailViewModel model)
        {
            var user = _context.Users
                .FirstOrDefault(x => x.Email == model.Email);

            if (user != null && user.ActivationCode == model.UserCode)
            {
                user.EmailConfirmed = true;
                _context.SaveChanges();

                return RedirectToAction("Index", "Login");
            }

            ModelState.AddModelError("", "Kod yanlış.");
            return View(model);
        }
    }
}
