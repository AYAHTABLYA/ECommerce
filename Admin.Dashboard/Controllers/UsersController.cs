using Admin.Dashboard.Models.Roles;
using Admin.Dashboard.Models.Users;
using ECommerce.Domain.Entities.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Admin.Dashboard.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var userEntities = await _userManager.Users
                .AsNoTracking()
                .ToListAsync();

            var users = new List<UserViewModel>(userEntities.Count);

            foreach (var user in userEntities)
            {
                var roles = await _userManager.GetRolesAsync(user);

                users.Add(new UserViewModel
                {
                    Id = user.Id,
                    DisplayName = user.DisplayName,
                    Email = user.Email,
                    Username = user.UserName,
                    Roles = roles.ToList() // or roles (depends on your property type)
                });
            }

            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            var roles = await _roleManager.Roles.ToListAsync();

            var userModel = new EditUserRoleViewModel
            {
                UserId = id,
                Username = user.UserName!,
                Roles = roles.Select(r => new UpdateRoleViewModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    IsSelected = _userManager.IsInRoleAsync(user, r.Name).Result
                }).ToList()
            };

            return View(userModel);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(EditUserRoleViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            var rolesForUser = await _userManager.GetRolesAsync(user);

            // Role was granted -> uncheck for the role (Remove this role)
            // Role was not granted -> check for the role (Add this role)

            foreach (var role in model.Roles) // All roles in the system
            {
                if (rolesForUser.Any(r => r == role.Name) && !role.IsSelected)
                    await _userManager.RemoveFromRoleAsync(user, role.Name);

                if (!rolesForUser.Any(r => r == role.Name) && role.IsSelected)
                    await _userManager.AddToRoleAsync(user, role.Name);

            }

            return RedirectToAction(nameof(Index));
        }
    }
}
