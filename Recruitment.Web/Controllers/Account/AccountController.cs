using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Recruitment.Application.Interfaces.Services.CoreBusiness;
using Recruitment.Domain.Entities.UserManagement;
using Recruitment.Web.ViewModels.UserManagement.Account;
using System.Security.Claims;

namespace Recruitment.Web.Controllers.Account
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IDepartmentService _departmentService;

        public AccountController(UserManager<User> userManager,
                                 SignInManager<User> signInManager,
                                 RoleManager<Role> roleManager, IDepartmentService departmentService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _departmentService = departmentService;
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null && user.IsActive)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

                if (result.Succeeded)
                {
                    // Get roles
                    var roles = await _userManager.GetRolesAsync(user);

                    var claims = new List<Claim>();

                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    // Get permissions
                    var permissions = await GetUserPermissions(user.Id);

                    claims.AddRange(permissions.Select(p => new Claim("Permission", p)));

                    await _signInManager.SignInWithClaimsAsync(user, model.RememberMe, claims);

                    // Update LastLogin
                    user.LastLogin = DateTime.UtcNow;
                    await _userManager.UpdateAsync(user);

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);

                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt or inactive user.");
            return View(model);
        }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            TempData["ShowAccessDenied"] = true;
            return RedirectToAction("Index", "Home");
        }

        private async Task<List<string>> GetUserPermissions(int userId)
        {
            var roles = await _userManager.GetRolesAsync(await _userManager.FindByIdAsync(userId.ToString()));

            var permissions = new List<string>();

            foreach (var role in roles)
            {
                var roleEntity = await _roleManager.FindByNameAsync(role);

                var rolePermissions = _roleManager.Roles
                    .Where(r => r.Id == roleEntity.Id)
                    .SelectMany(r => r.RolePermissions)
                    .Select(rp => rp.Permission.Resource + "." + rp.Permission.Action)
                    .ToList();

                permissions.AddRange(rolePermissions);
            }

            return permissions.Distinct().ToList();
        }
    }
}
