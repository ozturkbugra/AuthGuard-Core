using AuthGuardCore.Context;
using AuthGuardCore.Entities;
using AuthGuardCore.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthGuardCore.Controllers
{
    public class MessageController : Controller
    {
        private readonly AuthGuardCoreContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;


        public MessageController(AuthGuardCoreContext context, UserManager<AppUser> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IActionResult> Inbox()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user == null)
                return RedirectToAction("Login", "Account");

            var values = await _context.Messages
                .Where(m => m.RecieverEmail == user.Email)
                .ProjectTo<MessageWithSenderInfoViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return View(values);
        }


        public async Task<IActionResult> Sendbox()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user == null)
                return RedirectToAction("Login", "Account");

            var values = await _context.Messages
                .Where(m => m.SenderEmail == user.Email)
                .ProjectTo<MessageWithReceiverInfoViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return View(values);
        }

        public async Task<IActionResult> MessageDetail(int id)
        {
            var value = await _context.Messages.FirstOrDefaultAsync(x=> x.MessageID == id);
            return View(value);
        }

        public async Task<IActionResult> CreateMessage()
        {
            return View();
        }
    }
}
