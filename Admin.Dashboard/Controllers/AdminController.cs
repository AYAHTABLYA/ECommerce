using ECommerce.Domain.Entities.IdentityModule;
using ECommerce.Shared.DTOS.IdentityDtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Dashboard.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AdminController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
                return View(loginDTO);

            var user = await _userManager.FindByEmailAsync(loginDTO.Email);

            if (user is null)
            {
                ModelState.AddModelError("", "Invalid login attempt");
                return View(loginDTO);
            }

            var result = await _signInManager.PasswordSignInAsync(
                user,
                loginDTO.Password,
                isPersistent: false,
                lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Invalid login attempt");
                return View(loginDTO);
            }

            return RedirectToAction(nameof(Index), "Home");
        }

        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}
