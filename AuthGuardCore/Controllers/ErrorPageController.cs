using Microsoft.AspNetCore.Mvc;

namespace AuthGuardCore.Controllers
{
    public class ErrorPageController : Controller
    {
        [Route("Error/404")]
        public IActionResult Page404()
        {
            return View();
        }
        public IActionResult Page401()
        {
            return View();
        }

        public IActionResult Page403()
        {
            return View();
        }

        [Route("Error/{statusCode}")]
        public IActionResult HandleError(int statusCode)
        {
            if(statusCode == 404)
            {
                return RedirectToAction("Page404");
            }else if(statusCode == 401)
            {
                return RedirectToAction("Page401");
            }else if(statusCode == 403)
            {
                return RedirectToAction("Page403");
            }
            return View(statusCode);
        }

    }
}
