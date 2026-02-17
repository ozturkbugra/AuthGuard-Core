using AuthGuardCore.Context;
using AuthGuardCore.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthGuardCore.ViewComponents.NavbarHeaderViewComponents
{
    public class _MessageListOnNavbarHeaderComponentPartial : ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AuthGuardCoreContext _context;

        public _MessageListOnNavbarHeaderComponentPartial(UserManager<AppUser> userManager, AuthGuardCoreContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var values = await _context.Messages.Where(x => x.RecieverEmail == user.Email && x.IsRead == false).ToListAsync();
            return View(values);
        }
    }
}
