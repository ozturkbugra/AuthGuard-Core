using Microsoft.AspNetCore.Mvc;

namespace AuthGuardCore.ViewComponents.MessageViewComponents
{
    public class _MessageSidebarComponentPartial : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
