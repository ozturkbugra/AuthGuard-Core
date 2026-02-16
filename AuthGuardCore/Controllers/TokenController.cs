using Microsoft.AspNetCore.Mvc;

namespace AuthGuardCore.Controllers
{
    public class TokenController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
