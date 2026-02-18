using AuthGuardCore.Context;
using AuthGuardCore.Entities;
using AuthGuardCore.Models;
using AuthGuardCore.Models.JwtModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthGuardCore.Controllers
{
    public class LoginController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly AuthGuardCoreContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly JwtSettingsModel _jwtSettingsModel;


        public LoginController(SignInManager<AppUser> signInManager, AuthGuardCoreContext context, UserManager<AppUser> userManager, IOptions<JwtSettingsModel> jwtSettingsModel)
        {
            _signInManager = signInManager;
            _context = context;
            _userManager = userManager;
            _jwtSettingsModel = jwtSettingsModel.Value;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginUserViewModel model)
        {   
            var value = _context.Users.FirstOrDefault(x=> x.UserName == model.UserName);

            SimpleUserViewModel simpleUserViewModel = new()
            {
                City = value.City,
                Name = value.Name,
                Id = value.Id,
                SurName = value.Surname,
                Username = value.UserName
            };

            if(value == null)
            {
                return View();

            }

            if (value.EmailConfirmed == true)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, true, true);
                if (result.Succeeded)
                {
                    var token = Generate(simpleUserViewModel);
                    Response.Cookies.Append("jwtToken", token , new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTime.UtcNow.AddMinutes(_jwtSettingsModel.ExpireMinutes)
                    });

                    return RedirectToAction("Inbox", "Message");
                }
                return View();
            }
            return View();
        }

        public string Generate(SimpleUserViewModel model)
        {
            // Token içine gömülecek kullanıcı bilgileri (CLAIMS)
            var claims = new[]
            {
                new Claim("name",model.Name),
                new Claim("surname",model.SurName),
                new Claim("username",model.Username),
                new Claim("city",model.City),
                new Claim(ClaimTypes.NameIdentifier,model.Id),
                // Token'a benzersiz ID verir (her token farklı olur)
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            };

            // Gizli anahtar oluşturuyoruz (Key)
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtSettingsModel.Key));

            // İmzalama algoritması belirliyoruz
            var creds = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256);

            // Token nesnesini oluşturuyoruz
            var token = new JwtSecurityToken(
                issuer: _jwtSettingsModel.Issuer,       // Token'ı kim üretti
                audience: _jwtSettingsModel.Audience,   // Token kime ait
                claims: claims,                         // İçerdiği bilgiler
                expires: DateTime.UtcNow.AddMinutes(_jwtSettingsModel.ExpireMinutes), // Süre
                signingCredentials: creds);             // İmza bilgisi

            // Token'ı string hale çeviriyoruz
            return new JwtSecurityTokenHandler().WriteToken(token);

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
