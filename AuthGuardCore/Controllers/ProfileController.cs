using AuthGuardCore.Entities;
using AuthGuardCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace AuthGuardCore.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public ProfileController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> EditProfile()
        {
            var values = await _userManager.FindByNameAsync(User.Identity.Name);
            UserEditViewModel model = new();
            model.Surname = values.Surname;
            model.Name = values.Name;
            model.PhoneNumber = values.PhoneNumber;
            model.ImageUrl = values.ImageUrl;
            model.City = values.City;
            model.UserName = values.UserName;
            model.Email = values.Email;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(UserEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user == null)
                return RedirectToAction("Index", "Login");

            user.Surname = model.Surname;
            user.Name = model.Name;
            user.PhoneNumber = model.PhoneNumber;
            user.ImageUrl = model.ImageUrl;
            user.City = model.City;
            user.UserName = model.UserName;
            user.Email = model.Email;

            // Şifre doluysa değiştir
            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                if (model.Password != model.PasswordConfirm)
                {
                    ModelState.AddModelError("", "Şifreler uyuşmuyor.");
                    return View(model);
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, model.Password);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("", error.Description);

                    return View(model);
                }
            }

            await _userManager.UpdateAsync(user);

            return RedirectToAction("EditProfile");
        }



    }
}
