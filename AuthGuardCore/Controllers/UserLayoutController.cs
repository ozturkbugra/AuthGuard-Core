using Microsoft.AspNetCore.Mvc;

namespace AuthGuardCore.Controllers
{
    public class UserLayoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
