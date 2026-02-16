using AuthGuardCore.Models.JwtModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthGuardCore.Controllers
{
    public class TokenController : Controller
    {
        private readonly JwtSettingsModel _jwtSettingsModel;

        public TokenController(IOptions<JwtSettingsModel> JwtSettingsModel)
        {
            _jwtSettingsModel = JwtSettingsModel.Value;
        }

        public IActionResult Generate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Generate(SimpleUserViewModel model)
        {
            // Token içine gömülecek kullanıcı bilgileri (CLAIMS)
            var claims = new[]
            {
                new Claim("name",model.Name),
                new Claim("surname",model.SurName),
                new Claim("username",model.Username),
                new Claim("city",model.City),

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
            model.Token = new JwtSecurityTokenHandler().WriteToken(token);

            return View(model);
        }
    }
}
