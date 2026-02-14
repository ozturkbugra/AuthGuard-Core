using Microsoft.AspNetCore.Mvc;

namespace AuthGuardCore.ViewComponents
{
    public class _FooterUserLayoutComponentPartial : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
