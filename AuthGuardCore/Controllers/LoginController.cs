using AuthGuardCore.Context;
using AuthGuardCore.Entities;
using AuthGuardCore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthGuardCore.Controllers
{
    public class LoginController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly AuthGuardCoreContext _context;
        private readonly UserManager<AppUser> _userManager;

        public LoginController(SignInManager<AppUser> signInManager, AuthGuardCoreContext context, UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _context = context;
            _userManager = userManager;
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

        public IActionResult ExternalLogin(string provider, string? returnUrl)
        {
            var redirectUrl = Url.Action("ExternalLoginCallBack", "Login", new
            {
                returnUrl
            });

            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return Challenge(properties, provider);
        }

        public async Task<IActionResult> ExternalLoginCallBack(string? returnUrl, string? remoteError)
        {
            returnUrl ??= Url.Content("~");

            if (remoteError != null)
            {
                ModelState.AddModelError("", $"External Provider Error: {remoteError}");
                return RedirectToAction("Index");
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction("Index");

            }

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);

            if (result.Succeeded)
            {
                return RedirectToAction("Inbox", "Message");
            }
            else
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);

                // Email ile kullanıcı var mı kontrol et
                var existingUser = await _userManager.FindByEmailAsync(email);

                if (existingUser != null)
                {
                    // Google login'i bu kullanıcıya bağla
                    await _userManager.AddLoginAsync(existingUser, info);
                    await _signInManager.SignInAsync(existingUser, isPersistent: false);

                    return RedirectToAction("Inbox", "Message");
                }

                // Kullanıcı yoksa yeni oluştur
                var user = new AppUser
                {
                    UserName = email,
                    Email = email,
                    Name = email,
                    Surname = email,
                    EmailConfirmed = true
                };

                var identityResult = await _userManager.CreateAsync(user);

                if (identityResult.Succeeded)
                {
                    await _userManager.AddLoginAsync(user, info);
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("Inbox", "Message");
                }

                return RedirectToAction("Index");
            }
        }
    }
}
