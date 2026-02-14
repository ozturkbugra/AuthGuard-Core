using Microsoft.AspNetCore.Mvc;

namespace AuthGuardCore.Controllers
{
    public class MessageController : Controller
    {
        public IActionResult Inbox()
        {
            return View();
        }
    }
}
