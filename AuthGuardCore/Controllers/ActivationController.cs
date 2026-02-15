using Microsoft.AspNetCore.Mvc;

namespace AuthGuardCore.Controllers
{
    public class ActivationController : Controller
    {
        public IActionResult UserActivation()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UserActivation(string q)
        {
            return RedirectToAction("Index", "Login");
        }

        //ohhy wyjd hpbe oewl
    }
}
