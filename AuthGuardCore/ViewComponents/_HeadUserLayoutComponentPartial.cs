using Microsoft.AspNetCore.Mvc;

namespace AuthGuardCore.ViewComponents
{
    public class _HeadUserLayoutComponentPartial : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
