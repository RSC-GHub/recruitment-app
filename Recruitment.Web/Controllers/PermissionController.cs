using Microsoft.AspNetCore.Mvc;
using Recruitment.Application.DTOs.UserManagement.Permission;
using Recruitment.Application.Interfaces.Services.UserManagement;
using Recruitment.Web.ViewModels.UserManagement;

namespace Recruitment.Web.Controllers
{
    public class PermissionController : Controller
    {
        private readonly IPermissionService _permissionService;

        public PermissionController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        // GET: Permission
        public async Task<IActionResult> Index()
        {
            var permissions = await _permissionService.GetAllAsync();
            return View(permissions);
        }

        [HttpGet("setup")]
        public IActionResult Setup()
        {
            var vm = new PermissionSetupViewModel
            {
                Resources = new List<string> { "User", "Role", "Permission", "Country", "Location", "Department", "Title", "Vacancy", "Project" },
                Actions = new List<string> { "View", "Manage" }
            };
            return View(vm);
        }


        // GET: Permission/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Permission/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePermissionDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            await _permissionService.AddAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GeneratePermissions(string SelectedPermissions)
        {
            if (string.IsNullOrWhiteSpace(SelectedPermissions))
                return RedirectToAction(nameof(Setup));

            var permissions = SelectedPermissions.Split(',', StringSplitOptions.RemoveEmptyEntries);

            foreach (var item in permissions)
            {
                var parts = item.Split('_');
                var resource = parts[0];
                var action = parts[1];

                if (action == "Manage")
                {
                    var manageActions = new[] { "Create", "Edit", "Delete" };
                    foreach (var act in manageActions)
                    {
                        await _permissionService.AddAsync(new CreatePermissionDto
                        {
                            PermissionName = $"{resource}.{act}",
                            Description = $"{act} access for {resource}",
                            Resource = resource,
                            Action = act
                        });
                    }
                }
                else
                {
                    await _permissionService.AddAsync(new CreatePermissionDto
                    {
                        PermissionName = $"{resource}.{action}",
                        Description = $"{action} access for {resource}",
                        Resource = resource,
                        Action = action
                    });
                }
            }

            TempData["Success"] = "Permissions generated successfully.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Permission/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var permission = await _permissionService.GetByIdAsync(id);
            if (permission == null)
                return NotFound();

            var model = new UpdatePermissionDto
            {
                Id = permission.Id,
                PermissionName = permission.PermissionName,
                Description = permission.Description,
                Resource = permission.Resource,
                Action = permission.Action
            };

            return View(model);
        }

        // POST: Permission/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdatePermissionDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            await _permissionService.UpdateAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        // GET: Permission/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var permission = await _permissionService.GetByIdAsync(id);
            if (permission == null)
                return NotFound();

            return View(permission);
        }

        // POST: Permission/DeleteConfirmed
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _permissionService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
