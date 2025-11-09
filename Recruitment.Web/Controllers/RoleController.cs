using Microsoft.AspNetCore.Mvc;
using Recruitment.Application.DTOs.UserManagement.Role;
using Recruitment.Application.Interfaces.Services.UserManagement;
using Recruitment.Web.ViewModels.UserManagement;

namespace Recruitment.Web.Controllers
{
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly IPermissionService _permissionService;

        public RoleController(IRoleService roleService, IPermissionService permissionService)
        {
            _roleService = roleService;
            _permissionService = permissionService;
        }

        // GET: Role
        public async Task<IActionResult> Index()
        {
            var roles = await _roleService.GetAllWithPermissionsAsync();

            var viewModels = roles.Select(r => new RoleListViewModel
            {
                Id = r.Id,
                RoleName = r.RoleName,
                Description = r.Description,
                IsActive = r.IsActive,
                Permissions = r.Permissions?.Select(p => new PermissionItemViewModel
                {
                    Id = p.Id,
                    PermissionName = p.PermissionName,
                    Description = p.Description,
                    Resource = p.Resource,
                    Action = p.Action
                }).ToList()
            });

            return View(viewModels);
        }

        // GET: Role/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var role = await _roleService.GetByIdWithPermissionsAsync(id);
            if (role == null) return NotFound();

            var viewModel = new RoleDetailsViewModel
            {
                Id = role.Id,
                RoleName = role.RoleName,
                Description = role.Description,
                IsActive = role.IsActive,
                Permissions = role.Permissions?.Select(p => new PermissionItemViewModel
                {
                    Id = p.Id,
                    PermissionName = p.PermissionName,
                    Description = p.Description,
                    Resource = p.Resource,
                    Action = p.Action
                }).ToList()
            };

            return View(viewModel);
        }

        // GET: Role/Create
        public async Task<IActionResult> Create()
        {
            var permissions = await _permissionService.GetAllAsync();

            var viewModel = new RoleCreateViewModel
            {
                AllPermissions = permissions.Select(p => new PermissionItemViewModel
                {
                    Id = p.Id,
                    PermissionName = p.PermissionName,
                    Description = p.Description,
                    Resource = p.Resource,
                    Action = p.Action
                }).ToList()
            };

            return View(viewModel);
        }

        // POST: Role/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoleCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var permissions = await _permissionService.GetAllAsync();
                model.AllPermissions = permissions.Select(p => new PermissionItemViewModel
                {
                    Id = p.Id,
                    PermissionName = p.PermissionName,
                    Resource = p.Resource,
                    Action = p.Action
                }).ToList();
                return View(model);
            }

            var dto = new RoleCreateDto
            {
                RoleName = model.RoleName,
                Description = model.Description,
                IsActive = model.IsActive,
                PermissionIds = model.SelectedPermissionIds
            };

            await _roleService.AddAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        // GET: Role/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var role = await _roleService.GetByIdWithPermissionsAsync(id);
            if (role == null) return NotFound();

            var permissions = await _permissionService.GetAllAsync();

            var viewModel = new RoleEditViewModel
            {
                Id = role.Id,
                RoleName = role.RoleName,
                Description = role.Description,
                IsActive = role.IsActive,
                SelectedPermissionIds = role.Permissions?.Select(p => p.Id).ToList(),
                AllPermissions = permissions.Select(p => new PermissionItemViewModel
                {
                    Id = p.Id,
                    PermissionName = p.PermissionName,
                    Resource = p.Resource,
                    Action = p.Action,
                    IsSelected = role.Permissions?.Any(rp => rp.Id == p.Id) ?? false
                }).ToList()
            };

            return View(viewModel);
        }

        // POST: Role/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RoleEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var permissions = await _permissionService.GetAllAsync();
                model.AllPermissions = permissions.Select(p => new PermissionItemViewModel
                {
                    Id = p.Id,
                    PermissionName = p.PermissionName,
                    Resource = p.Resource,
                    Action = p.Action
                }).ToList();
                return View(model);
            }

            var dto = new RoleUpdateDto
            {
                Id = model.Id,
                RoleName = model.RoleName,
                Description = model.Description,
                IsActive = model.IsActive,
                PermissionIds = model.SelectedPermissionIds
            };

            await _roleService.UpdateAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        // GET: Role/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var role = await _roleService.GetByIdWithPermissionsAsync(id);
            if (role == null) return NotFound();

            var viewModel = new RoleDetailsViewModel
            {
                Id = role.Id,
                RoleName = role.RoleName,
                Description = role.Description,
                IsActive = role.IsActive,
                Permissions = role.Permissions?.Select(p => new PermissionItemViewModel
                {
                    Id = p.Id,
                    PermissionName = p.PermissionName,
                    Resource = p.Resource,
                    Action = p.Action
                }).ToList()
            };

            return View(viewModel);
        }

        // POST: Role/DeleteConfirmed
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _roleService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
