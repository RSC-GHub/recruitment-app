using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Recruitment.Application.Interfaces.Services.CoreBusiness;
using Recruitment.Domain.Entities.UserManagement;
using Recruitment.Web.ViewModels.UserManagement.Account;

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

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null && user.IsActive)
            {
                var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
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

        // GET: /Account/Register
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            var departments = await _departmentService.GetAllAsync();

            var model = new RegisterViewModel
            {
                Departments = departments.Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Name
                })
            };

            return View(model);
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var departments = await _departmentService.GetAllAsync();
                model.Departments = departments.Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Name
                });
                return View(model);
            }

            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                DepartmentId = model.DepartmentId,
                FullName = model.FullName,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                if (await _roleManager.RoleExistsAsync("User"))
                    await _userManager.AddToRoleAsync(user, "User");

                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            var allDepartments = await _departmentService.GetAllAsync();
            model.Departments = allDepartments.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.Name
            });

            return View(model);
        }


    }
}
