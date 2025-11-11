using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Recruitment.Web.Authorization
{
    public class HasPermissionAttribute : TypeFilterAttribute
    {
        public HasPermissionAttribute(string resource, string action)
            : base(typeof(RequirePermissionFilter))
        {
            Arguments = new object[] { resource, action };
        }
    }
}
