using AuthGuardCore.Context;
using AuthGuardCore.Entities;
using AuthGuardCore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthGuardCore.ViewComponents.MessageViewComponents
{
    public class _MessageSidebarComponentPartial : ViewComponent
    {
        private readonly AuthGuardCoreContext _context;
        private readonly UserManager<AppUser> _userManager;


        public _MessageSidebarComponentPartial(AuthGuardCoreContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            SendAndInboxMessageCountViewModel model = new();

            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            model.SendboxCount = _context.Messages.Count(x => x.SenderEmail == user.Email);
            model.InboxCount = _context.Messages.Count(x => x.RecieverEmail == user.Email);
            return View(model);
        }
    }
}
