using Recruitment.Application.DTOs.UserManagement.Permission;
using Recruitment.Application.DTOs.UserManagement.Role;
using Recruitment.Application.Interfaces.Persistence;
using Recruitment.Application.Interfaces.Persistence.UserManagement;
using Recruitment.Application.Interfaces.Services.UserManagement;
using Recruitment.Domain.Entities.UserManagement;

namespace Recruitment.Application.Services.UserManagement
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRoleRepository _roleRepository;

        public RoleService(IUnitOfWork unitOfWork, IRoleRepository roleRepository)
        {
            _unitOfWork = unitOfWork;
            _roleRepository = roleRepository;
        }

        public async Task<IEnumerable<RoleReadDto>> GetAllAsync()
        {
            var roles = await _unitOfWork.Roles.GetAllAsync();
            return roles.Select(r => new RoleReadDto
            {
                Id = r.Id,
                RoleName = r.Name,
                Description = r.Description,
                IsActive = r.IsActive
            });
        }

        public async Task<IEnumerable<RoleReadDto>> GetAllWithPermissionsAsync()
        {
            var roles = await _roleRepository.GetAllWithPermissionsAsync();
            return roles.Select(r => new RoleReadDto
            {
                Id = r.Id,
                RoleName = r.Name,
                Description = r.Description,
                IsActive = r.IsActive,
                Permissions = r.RolePermissions?.Select(rp => new PermissionDto
                {
                    Id = rp.Permission.Id,
                    PermissionName = rp.Permission.PermissionName,
                    Description = rp.Permission.Description,
                    Resource = rp.Permission.Resource,
                    Action = rp.Permission.Action
                }).ToList()
            });
        }


        public async Task<RoleReadDto?> GetByIdAsync(int id)
        {
            var role = await _unitOfWork.Roles.GetByIdAsync(id);
            if (role == null)
                return null;

            return new RoleReadDto
            {
                Id = role.Id,
                RoleName = role.Name,
                Description = role.Description,
                IsActive = role.IsActive
            };
        }

        public async Task<RoleReadDto?> GetByIdWithPermissionsAsync(int id)
        {
            var role = await _roleRepository.GetByIdWithPermissionsAsync(id);
            if (role == null)
                return null;

            return new RoleReadDto
            {
                Id = role.Id,
                RoleName = role.Name,
                Description = role.Description,
                IsActive = role.IsActive,
                Permissions = role.RolePermissions?.Select(rp => new PermissionDto
                {
                    Id = rp.Permission.Id,
                    PermissionName = rp.Permission.PermissionName,
                    Description = rp.Permission.Description,
                    Resource = rp.Permission.Resource,
                    Action = rp.Permission.Action
                }).ToList()
            };
        }


        public async Task AddAsync(RoleCreateDto dto)
        {
            var role = new Role
            {
                Name = dto.RoleName,
                Description = dto.Description,
                IsActive = dto.IsActive
            };

            // Add the role first
            await _unitOfWork.Roles.AddAsync(role);
            await _unitOfWork.CompleteAsync();

            // Then assign permissions if any
            if (dto.PermissionIds != null && dto.PermissionIds.Any())
            {
                foreach (var pid in dto.PermissionIds)
                {
                    var permission = await _unitOfWork.Permissions.GetByIdAsync(pid);
                    if (permission != null)
                    {
                        var rolePermission = new RolePermission
                        {
                            RoleId = role.Id,
                            PermissionId = pid,
                            GrantedBy = 1, // temporary (to be replaced by logged-in user)
                        };
                        await _unitOfWork.RolePermissions.AddAsync(rolePermission);
                    }
                }
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task UpdateAsync(RoleUpdateDto dto)
        {
            var role = await _roleRepository.GetByIdWithPermissionsAsync(dto.Id);
            if (role == null)
                return;

            role.Name = dto.RoleName;
            role.Description = dto.Description;
            role.IsActive = dto.IsActive;

            // Remove old permissions
            if (role.RolePermissions != null)
            {
                foreach (var rp in role.RolePermissions.ToList())
                {
                    _unitOfWork.RolePermissions.Delete(rp);
                }
            }

            // Add new permissions
            if (dto.PermissionIds != null && dto.PermissionIds.Any())
            {
                foreach (var pid in dto.PermissionIds)
                {
                    var permission = await _unitOfWork.Permissions.GetByIdAsync(pid);
                    if (permission != null)
                    {
                        var rolePermission = new RolePermission
                        {
                            RoleId = role.Id,
                            PermissionId = pid,
                            GrantedBy = 1
                        };
                        await _unitOfWork.RolePermissions.AddAsync(rolePermission);
                    }
                }
            }

            _unitOfWork.Roles.Update(role);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var role = await _roleRepository.GetByIdWithPermissionsAsync(id);
            if (role == null)
                return;

            // Delete role permissions first
            if (role.RolePermissions != null)
            {
                foreach (var rp in role.RolePermissions.ToList())
                {
                    _unitOfWork.RolePermissions.Delete(rp);
                }
            }

            _unitOfWork.Roles.Delete(role);
            await _unitOfWork.CompleteAsync();
        }
    }
}
