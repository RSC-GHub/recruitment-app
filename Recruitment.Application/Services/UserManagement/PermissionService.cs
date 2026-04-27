using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Recruitment.Application.DTOs.UserManagement.Permission;
using Recruitment.Application.Interfaces.Persistence;
using Recruitment.Application.Interfaces.Services.UserManagement;
using Recruitment.Domain.Entities.UserManagement;
using System.Security.Claims;

namespace Recruitment.Application.Services.UserManagement
{
    public class PermissionService : IPermissionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;


        public PermissionService(IUnitOfWork unitOfWork, 
            IHttpContextAccessor httpContextAccessor,
            UserManager<User> userManager,
            RoleManager<Role> roleManager)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IEnumerable<PermissionDto>> GetAllAsync()
        {
            var permissions = await _unitOfWork.Permissions.GetAllAsync();
            return permissions.Select(p => new PermissionDto
            {
                Id = p.Id,
                PermissionName = p.PermissionName,
                Description = p.Description,
                Resource = p.Resource,
                Action = p.Action
            });
        }

        public async Task<PermissionDto?> GetByIdAsync(int id)
        {
            var permission = await _unitOfWork.Permissions.GetByIdAsync(id);
            if (permission == null)
                return null;

            return new PermissionDto
            {
                Id = permission.Id,
                PermissionName = permission.PermissionName,
                Description = permission.Description,
                Resource = permission.Resource,
                Action = permission.Action
            };
        }

        public async Task AddAsync(CreatePermissionDto dto)
        {
            var permission = new Permission
            {
                PermissionName = dto.PermissionName,
                Description = dto.Description,
                Resource = dto.Resource,
                Action = dto.Action
            };

            await _unitOfWork.Permissions.AddAsync(permission);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateAsync(UpdatePermissionDto dto)
        {
            var permission = await _unitOfWork.Permissions.GetByIdAsync(dto.Id);
            if (permission == null)
                return;

            permission.PermissionName = dto.PermissionName;
            permission.Description = dto.Description;
            permission.Resource = dto.Resource;
            permission.Action = dto.Action;

            _unitOfWork.Permissions.Update(permission);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var permission = await _unitOfWork.Permissions.GetByIdAsync(id);
            if (permission == null)
                return;

            _unitOfWork.Permissions.Delete(permission);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<bool> HasPermissionAsync(string module, string action)
        {
            var userId = _httpContextAccessor.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return false;

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("Admin"))
                return true;

            var permissions = await _roleManager.Roles
                .Where(r => roles.Contains(r.Name!) && r.IsActive)
                .SelectMany(r => r.RolePermissions!)
                .Select(rp => rp.Permission!)
                .ToListAsync();

            return permissions.Any(p =>
                p.Resource == module &&
                p.Action == action);
        }


    }
}
