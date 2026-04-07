using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Recruitment.Application.Interfaces.Persistence;
using Recruitment.Application.Interfaces.Services.CoreBusiness;
using Recruitment.Application.Interfaces.Services.UserManagement;
using Recruitment.Domain.Entities.UserManagement;
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
        private readonly IUserProjectService _userProjectService;
        private readonly IUnitOfWork _unitOfWork;

        public UserManagementController(UserManager<User> userManager,
                                        RoleManager<Role> roleManager,
                                        IDepartmentService departmentService,
                                        SignInManager<User> signInManager,
                                        IUnitOfWork unitOfWork,
                                        IUserProjectService userProjectService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userProjectService = userProjectService;
            _departmentService = departmentService;
            _unitOfWork = unitOfWork;
            _userProjectService = userProjectService;
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
        public async Task<IActionResult> Profile(int id)
        {
            var user = await _userManager.Users
                .Include(u => u.Department)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return NotFound();

            // ===== Roles =====
            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = await _roleManager.Roles
                .Where(r => r.IsActive)
                .ToListAsync();
            var userRoleIds = allRoles
                .Where(r => userRoles.Contains(r.Name))
                .Select(r => r.Id)
                .ToList();

            // ===== Departments =====
            var departments = await _departmentService.GetAllAsync();

            // ===== Projects =====
            var allProjects = await _unitOfWork.Projects.GetAllAsync();
            var userProjectIds = await _userProjectService.GetUserProjectsAsync(user.Id);

            var userProjects = allProjects
                .Where(p => userProjectIds.Contains(p.Id))
                .ToList();

            var model = new UserProfileViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email!,
                DepartmentId = user.DepartmentId,
                DepartmentName = user.Department?.Name ?? "—",
                IsActive = user.IsActive,

                // ===== Roles =====
                Roles = userRoles.ToList(),
                SelectedRoleIds = userRoleIds,
                AvailableRoles = allRoles.Select(r => new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = r.Name,
                    Selected = userRoles.Contains(r.Name)
                }),

                // ===== Departments =====
                Departments = departments.Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Name
                }),

                // ===== Projects =====
                SelectedProjectIds = userProjectIds,
                AvailableProjects = allProjects.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.ProjectName,
                    Selected = userProjectIds.Contains(p.Id)
                }),
                Projects = userProjects
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProjects(int id, List<int> selectedProjectIds)
        {
            if (id == 0)
                return BadRequest();

            // Example: update user projects
            await _userProjectService.UpdateUserProjectsAsync(id, selectedProjectIds);

            TempData["Success"] = "Projects updated successfully!";
            return RedirectToAction("Profile", new { id });
        }

        // POST: Remove project from user
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveProject(int userId, int projectId)
        {
            if (userId == 0 || projectId == 0)
                return BadRequest();

            try
            {
                await _userProjectService.RemoveProjectFromUserAsync(userId, projectId);
                TempData["Success"] = "Project removed successfully!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Failed to remove project: {ex.Message}";
            }

            return RedirectToAction("Profile", new { id = userId });
        }
        // POST: Remove role
        [HttpPost]
        [ValidateAntiForgeryToken]
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

            // Trim email and full name
            var email = model.Email?.Trim();
            var fullName = model.FullName?.Trim();

            // Check if user with the same email already exists
            var existingUserByEmail = await _userManager.FindByEmailAsync(email);
            if (existingUserByEmail != null)
            {
                TempData["ErrorMessage"] = "This email is already registered.";
                var departments = await _departmentService.GetAllAsync();
                model.Departments = departments.Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Name
                });
                return View(model);
            }

            // Check if username already exists (in case username != email in your system)
            var existingUserByUsername = await _userManager.FindByNameAsync(email);
            if (existingUserByUsername != null)
            {
                TempData["ErrorMessage"] = "This username is already taken.";
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
                UserName = email,
                Email = email,
                DepartmentId = model.DepartmentId,
                FullName = fullName,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                // Add default "User" role if exists
                if (await _roleManager.RoleExistsAsync("User"))
                    await _userManager.AddToRoleAsync(user, "User");

                // Sign in the new user
                await _signInManager.SignInAsync(user, isPersistent: false);

                TempData["SuccessMessage"] = "User created successfully!";
                return RedirectToAction("Index", "Home");
            }

            // If creation failed, show errors
            TempData["ErrorMessage"] = string.Join(", ", result.Errors.Select(e => e.Description));

            // Re-populate departments for the view
            var allDepartments = await _departmentService.GetAllAsync();
            model.Departments = allDepartments.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.Name
            });

            return View(model);
        }

        [HttpGet]
        public IActionResult ChangePassword(int userId)
        {
            var model = new UpdatePasswordViewModel
            {
                UserId = userId
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(UpdatePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByIdAsync(model.UserId.ToString());
            if (user == null)
                return NotFound();

            var passwordCheck = await _userManager.CheckPasswordAsync(user, model.CurrentPassword);
            if (!passwordCheck)
            {
                ModelState.AddModelError(string.Empty, "Current password is incorrect.");
                return View(model);
            }

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (result.Succeeded)
            {
                TempData["Success"] = "Password updated successfully.";
                return RedirectToAction("Profile", new { id = model.UserId });
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(model);
        }
    }
}
