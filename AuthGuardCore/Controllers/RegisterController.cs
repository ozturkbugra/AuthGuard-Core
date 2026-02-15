using AuthGuardCore.Entities;
using AuthGuardCore.Interfaces;
using AuthGuardCore.Models;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace AuthGuardCore.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;


        public RegisterController(UserManager<AppUser> userManager, IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(RegisterUserViewModel model)
        {
            Random random = new();
            int code = random.Next(100000, 1000000);

            AppUser appUser = new()
            {
                Name = model.Name,
                Email = model.Email,
                Surname = model.SurName,
                UserName = model.UserName,
                ActivationCode = code,
            };


            var result = await _userManager.CreateAsync(appUser, model.Password);

            if (result.Succeeded)
            {
                await _emailService.SendEmailAsync(
                    model.Email,
                    "AuthGuard Aktivasyon Kodu",
                    $"Hesabınızı doğrulamak için aktivasyon kodu: {code}"
                );

                return RedirectToAction("UserActivation", "Activation");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }

            return View();
        }
    }
}
