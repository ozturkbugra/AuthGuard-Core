using AuthGuardCore.Context;
using AuthGuardCore.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthGuardCore.Controllers
{
    public class CommentController : Controller
    {
        private readonly AuthGuardCoreContext _context;
        private readonly UserManager<AppUser> _userManager;

        public CommentController(AuthGuardCoreContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> UserComments()
        {
            var values = await _context.Comments.Include(x => x.AppUser).ToListAsync(); 
            return View(values);
        }

        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> UserCommentList()
        {
            var values = await _context.Comments.Include(x => x.AppUser).ToListAsync(); 
            return View(values);
        }


        public PartialViewResult CreateComment()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment(Comment comment)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            comment.AppUserId = user.Id;
            comment.Date = DateTime.UtcNow;
            comment.Status = "Onay Bekliyor";
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction("UserCommentList");
        }

    }
}
