using AuthGuardCore.Entities;
using AuthGuardCore.Interfaces;
using AuthGuardCore.Models.ForgetPasswordModels;
using AuthGuardCore.Models.ResetPasswordModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace AuthGuardCore.Controllers
{
    public class PasswordChangeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;


        public PasswordChangeController(UserManager<AppUser> userManager, IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Böyle bir kullanıcı bulunamadı.");
                return View(model);
            }

            var passwordResetToken =
                await _userManager.GeneratePasswordResetTokenAsync(user);

            var passwordResetTokenLink = Url.Action(
                "ResetPassword",
                "PasswordChange",
                new
                {
                    userId = user.Id,
                    token = passwordResetToken
                },
                HttpContext.Request.Scheme);

            string body = $"Şifrenizi yenilemek için aşağıdaki linke tıklayın:\n\n{passwordResetTokenLink}";

            await _emailService.SendEmailAsync(
                user.Email,
                "AuthGuard Şifre Yenileme",
                body);

            ViewBag.Message = "Şifre yenileme linki e-posta adresinize gönderildi.";
            return View();
        }

        public IActionResult ResetPassword(string userId, string token)
        {
            if (userId == null || token == null)
                return RedirectToAction("Index", "PasswordChange");

            var model = new ResetPasswordViewModel
            {
                UserId = userId,
                Token = token
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("", "Şifreler uyuşmuyor.");
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null)
            {
                ModelState.AddModelError("", "Kullanıcı bulunamadı.");
                return View(model);
            }

            var result = await _userManager.ResetPasswordAsync(
                user,
                model.Token,
                model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

                return View(model);
            }

            return RedirectToAction("Index", "Login");
        }




    }
}
