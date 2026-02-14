using Microsoft.AspNetCore.Mvc;

namespace AuthGuardCore.ViewComponents
{
    public class _ScriptUserLayoutComponentPartial : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
