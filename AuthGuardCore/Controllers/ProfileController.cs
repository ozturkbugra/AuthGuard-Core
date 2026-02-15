using Microsoft.AspNetCore.Mvc;

namespace AuthGuardCore.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult EditProfile()
        {
            return View();
        }
    }
}
