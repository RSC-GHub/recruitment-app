using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Recruitment.Web.Authorization
{
    public class RequirePermissionFilter : IAsyncAuthorizationFilter
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly string _resource;
        private readonly string _action;

        public RequirePermissionFilter(IAuthorizationService authorizationService, string resource, string action)
        {
            _authorizationService = authorizationService;
            _resource = resource;
            _action = action;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            var requirement = new PermissionRequirement(_resource, _action);

            var authorized = await _authorizationService.AuthorizeAsync(user, null, requirement);
            if (!authorized.Succeeded)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
