using Microsoft.AspNetCore.Mvc;

namespace AuthGuardCore.Controllers
{
    public class ErrorPageController : Controller
    {
        public IActionResult Page404()
        {
            return View();
        }
    }
}
