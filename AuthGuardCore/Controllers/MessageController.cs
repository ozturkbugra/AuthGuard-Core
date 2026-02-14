using AuthGuardCore.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthGuardCore.Controllers
{
    public class MessageController : Controller
    {
        private readonly AuthGuardCoreContext _context;

        public MessageController(AuthGuardCoreContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Inbox()
        {
            var values = await _context.Messages.Where(x => x.RecieverEmail == "ztrk1212@gmail.com").ToListAsync();
            return View(values);
        }


        public async Task<IActionResult> Sendbox()
        {
            var values = await _context.Messages.Where(x => x.SenderEmail == "ztrk1212@gmail.com").ToListAsync();
            return View(values);
        }
    }
}
