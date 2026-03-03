using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Recruitment.Application.Interfaces.Services.CoreBusiness;
using Recruitment.Domain.Entities.UserManagement;
using Recruitment.Web.Authorization;
using Recruitment.Web.ViewModels.UserManagement.Account;
using Recruitment.Web.ViewModels.UserManagement.User;

namespace Recruitment.Web.Controllers
{
    public class UserManagementController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IDepartmentService _departmentService;

        public UserManagementController(UserManager<User> userManager,
                                        RoleManager<Role> roleManager,
                                        IDepartmentService departmentService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _departmentService = departmentService;
        }

        // GET: Users
        //[HasPermission("User", "View")]
        public IActionResult Index()
        {
            var users = _userManager.Users
                .Include(u => u.Department)
                .Select(u => new UserWithRoleViewModel
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    DepartmentName = u.Department != null ? u.Department.Name : "—",
                    IsActive = u.IsActive
                })
                .ToList();

            return View(users);
        }

        // GET: User Profile
        [HttpGet]
        //[HasPermission("User", "Edit")]
        public async Task<IActionResult> Profile(int id)
        {
            var user = await _userManager.Users
                .Include(u => u.Department)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);

            var allRoles = await _roleManager.Roles
                .Where(r => r.IsActive)
                .ToListAsync();

            var departments = await _departmentService.GetAllAsync();

            var userRoleIds = allRoles
                .Where(r => userRoles.Contains(r.Name))
                .Select(r => r.Id)
                .ToList();

            var model = new UserProfileViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email!,
                DepartmentId = user.DepartmentId,
                DepartmentName = user.Department?.Name ?? "—",
                IsActive = user.IsActive,
                Roles = userRoles.ToList(),
                SelectedRoleIds = userRoleIds,
                AvailableRoles = allRoles.Select(r => new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = r.Name,
                    Selected = userRoles.Contains(r.Name)
                }),
                Departments = departments.Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Name
                })
            };

            return View(model);
        }

        // POST: Edit user info
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[HasPermission("User", "Edit")]
        public async Task<IActionResult> EditUser(UserProfileViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id.ToString());
            if (user == null)
                return NotFound();

            user.FullName = model.FullName;
            user.Email = model.Email;
            user.IsActive = model.IsActive;
            user.DepartmentId = model.DepartmentId;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
                TempData["Success"] = "User information updated successfully.";
            else
                TempData["Error"] = string.Join(", ", result.Errors.Select(e => e.Description));

            return RedirectToAction("Index");
        }

        // POST: Update roles
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[HasPermission("Role", "Edit")]
        public async Task<IActionResult> UpdateRoles(UserProfileViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id.ToString());
            if (user == null)
                return NotFound();

            var currentRoles = await _userManager.GetRolesAsync(user);

            var selectedRoleIds = model.SelectedRoleIds ?? new List<int>();

            var allRoles = await _roleManager.Roles
                .Where(r => r.IsActive)
                .ToListAsync();

            var selectedRoleNames = allRoles
                .Where(r => selectedRoleIds.Contains(r.Id))
                .Select(r => r.Name)
                .ToList();

            var rolesToRemove = currentRoles.Except(selectedRoleNames).ToList();
            var rolesToAdd = selectedRoleNames.Except(currentRoles).ToList();

            if (rolesToRemove.Any())
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                if (!removeResult.Succeeded)
                {
                    TempData["Error"] = string.Join(", ", removeResult.Errors.Select(e => e.Description));
                    return RedirectToAction("Profile", new { id = model.Id });
                }
            }

            if (rolesToAdd.Any())
            {
                var addResult = await _userManager.AddToRolesAsync(user, rolesToAdd);
                if (!addResult.Succeeded)
                {
                    TempData["Error"] = string.Join(", ", addResult.Errors.Select(e => e.Description));
                    return RedirectToAction("Profile", new { id = model.Id });
                }
            }

            TempData["Success"] = "User roles updated successfully.";
            return RedirectToAction("Profile", new { id = model.Id });
        }

        // POST: Remove role
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[HasPermission("User", "Edit")]
        public async Task<IActionResult> RemoveRole(int userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return NotFound();

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);

            if (result.Succeeded)
                TempData["Success"] = $"Role '{roleName}' removed successfully.";
            else
                TempData["Error"] = string.Join(", ", result.Errors.Select(e => e.Description));

            return RedirectToAction("Profile", new { id = userId });
        }

        // GET: UserManagement/Register
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

        // POST: UserManagement/Register
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
