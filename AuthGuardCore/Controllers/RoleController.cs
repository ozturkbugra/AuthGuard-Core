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


        public async Task<IActionResult> UserList()
        {
            var values = await _userManager.Users.ToListAsync();
            return View(values);
        }

        public async Task<IActionResult> AssignRole(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            var roles = await _roleManager.Roles.ToListAsync();
            var userRoles = await _userManager.GetRolesAsync(user);

            var model = new UserRoleAssignViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = roles.Select(r => new RoleAssignItemViewModel
                {
                    RoleID = r.Id,
                    RoleName = r.Name,
                    RoleExist = userRoles.Contains(r.Name)
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(UserRoleAssignViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null)
                return NotFound();

            foreach (var role in model.Roles)
            {
                if (role.RoleExist)
                {
                    if (!await _userManager.IsInRoleAsync(user, role.RoleName))
                        await _userManager.AddToRoleAsync(user, role.RoleName);
                }
                else
                {
                    if (await _userManager.IsInRoleAsync(user, role.RoleName))
                        await _userManager.RemoveFromRoleAsync(user, role.RoleName);
                }
            }

            return RedirectToAction("UserList");
        }

    }
}
