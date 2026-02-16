using AuthGuardCore.Entities;
using AuthGuardCore.Interfaces;
using AuthGuardCore.Models.ForgetPasswordModels;
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
                "Password",
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
    }
}
