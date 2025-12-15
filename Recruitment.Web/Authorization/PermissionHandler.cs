using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Recruitment.Domain.Entities.UserManagement;
using System.Security.Claims;

namespace Recruitment.Web.Authorization
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public PermissionHandler(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            if (context.User.Identity?.IsAuthenticated != true)
            {
                context.Fail();
                return;
            }

            // Admin bypass
            if (context.User.IsInRole("Admin"))
            {
                context.Succeed(requirement);
                return;
            }

            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
            {
                context.Fail();
                return;
            }

            var user = await _userManager.FindByIdAsync(userIdClaim);
            if (user == null)
            {
                context.Fail();
                return;
            }

            var roles = await _userManager.GetRolesAsync(user);

            var permissions = await _roleManager.Roles
                .Where(r => roles.Contains(r.Name!) && r.IsActive)
                .Include(r => r.RolePermissions!)
                    .ThenInclude(rp => rp.Permission)
                .SelectMany(r => r.RolePermissions!)
                .Select(rp => rp.Permission!)
                .ToListAsync();

            if (permissions.Any(p =>
                p.Resource.Equals(requirement.Resource, StringComparison.OrdinalIgnoreCase) &&
                p.Action.Equals(requirement.Action, StringComparison.OrdinalIgnoreCase)))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
        }
    }
}
