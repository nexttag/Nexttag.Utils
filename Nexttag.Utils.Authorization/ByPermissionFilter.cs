using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Nexttag.Utils.Authorization
{
    public class ByPermissionFilter : IAuthorizationFilter
    {
        public string Permission { get; }

        public ByPermissionFilter(string permission)
        {
            Permission = permission;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.Result == null)
            {
                context.Result =
                    context.HttpContext.User.Claims.Any(c => c.Type == "permissions" && c.Value == Permission) ?
                        null :
                        new ForbidResult();
            }
        }
    }
}