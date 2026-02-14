using AuthGuardCore.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthGuardCore.ViewComponents.MessageViewComponents
{
    public class _MessageCategoryListSidebarComponentPartial : ViewComponent
    {
        private readonly AuthGuardCoreContext _context;

        public _MessageCategoryListSidebarComponentPartial(AuthGuardCoreContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _context.Categories.Where(x=> x.Status == true).OrderBy(x=> x.Name).ToListAsync();
            return View(values);
        }

    }
}
