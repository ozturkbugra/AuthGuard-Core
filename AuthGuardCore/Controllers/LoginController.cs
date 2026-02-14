using AuthGuardCore.Models;
using Microsoft.AspNetCore.Mvc;

namespace AuthGuardCore.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginUserViewModel model)
        {
            return View();
        }
    }
}
