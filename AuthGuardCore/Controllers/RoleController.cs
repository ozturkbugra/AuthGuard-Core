using AuthGuardCore.Entities;
using AuthGuardCore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthGuardCore.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> RoleList()
        {
            var values = await _roleManager.Roles.ToListAsync();
            return View(values);
        }


        public async Task<IActionResult> DeleteRole(string id)
        {
            var values = await _roleManager.Roles.FirstOrDefaultAsync(x=> x.Id == id);
            await _roleManager.DeleteAsync(values);
            return RedirectToAction("RoleList");
        }

        public async Task<IActionResult> UpdateRole(string id)
        {
            var values = await _roleManager.Roles.FirstOrDefaultAsync(x=> x.Id == id);
            UpdateRoleViewModel model = new()
            {
                RoleID = values.Id,
                RoleName = values.Name
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRole(UpdateRoleViewModel model)
        {
            var values = await _roleManager.Roles.FirstOrDefaultAsync(x => x.Id == model.RoleID);

            values.Name = model.RoleName;
            
            await _roleManager.UpdateAsync(values);
            return RedirectToAction("RoleList");
        }



        public async Task<IActionResult> CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            IdentityRole role = new()
            {
                Name = model.RoleName
            };

            await _roleManager.CreateAsync(role);
            return RedirectToAction("RoleList");
        }
    }
}
