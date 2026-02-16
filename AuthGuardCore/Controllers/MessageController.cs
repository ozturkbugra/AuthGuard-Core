using AuthGuardCore.Context;
using AuthGuardCore.Entities;
using AuthGuardCore.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AuthGuardCore.Controllers
{
    [Authorize]
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

            if (User.Identity.Name == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByNameAsync(User.Identity.Name);

           
            if (user == null)
                return RedirectToAction("Login", "Account");

            var values = await _context.Messages
                .Where(m => m.RecieverEmail == user.Email)
                .ProjectTo<MessageWithSenderInfoViewModel>(_mapper.ConfigurationProvider)
                .OrderByDescending(x => x.SendDate).ThenByDescending(x => x.MessageID)
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
                .OrderByDescending(x=> x.SendDate).ThenByDescending(x=> x.MessageID)
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
            var model = new CreateMessageViewModel();

            model.Categories = await _context.Categories
                .Where(x => x.Status) // aktif kategoriler
                .Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.CategoryID.ToString()
                }).ToListAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(CreateMessageViewModel model)
        {
            var sender = await _userManager.GetUserAsync(User);

            if (sender == null)
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
            {
                // Kategorileri tekrar doldurma kodu
                model.Categories = await _context.Categories
                    .Where(x => x.Status)
                    .Select(c => new SelectListItem
                    {
                        Text = c.Name,
                        Value = c.CategoryID.ToString()
                    }).ToListAsync();

                return View(model);
            }

            // Alıcı var mı kontrolü
            var receiver = await _userManager.FindByEmailAsync(model.ReceiverEmail);

            if (receiver == null)
            {
                ModelState.AddModelError("ReceiverEmail", "Bu mail adresine ait kullanıcı bulunamadı.");

                model.Categories = await _context.Categories
                    .Where(x => x.Status)
                    .Select(c => new SelectListItem
                    {
                        Text = c.Name,
                        Value = c.CategoryID.ToString()
                    }).ToListAsync();

                return View(model);
            }

            var message = new Message
            {
                SenderEmail = sender.Email,
                RecieverEmail = receiver.Email,
                Subject = model.Subject,
                MessageDetail = model.MessageDetail,
                CategoryID = model.CategoryID,
                SendDate = DateTime.Now,
                IsRead = false
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return RedirectToAction("SendBox");
        }



    }
}
