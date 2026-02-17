using AuthGuardCore.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthGuardCore.Controllers
{
    public class CommentController : Controller
    {
        private readonly AuthGuardCoreContext _context;

        public CommentController(AuthGuardCoreContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> UserComments()
        {
            var values = await _context.Comments.Include(x => x.AppUser).ToListAsync(); 
            return View(values);
        }

        public async Task<IActionResult> UserCommentList()
        {
            var values = await _context.Comments.Include(x => x.AppUser).ToListAsync(); 
            return View(values);
        }
    }
}
