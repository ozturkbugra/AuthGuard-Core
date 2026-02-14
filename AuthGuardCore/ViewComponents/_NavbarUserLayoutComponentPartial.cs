using Microsoft.AspNetCore.Mvc;

namespace AuthGuardCore.ViewComponents
{
    public class _NavbarUserLayoutComponentPartial : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
