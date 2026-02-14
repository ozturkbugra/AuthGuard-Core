using Microsoft.AspNetCore.Mvc;

namespace AuthGuardCore.ViewComponents
{
    public class _BreadCombUserLayoutComponentPartial : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
