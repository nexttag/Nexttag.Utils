using Microsoft.AspNetCore.Mvc;

namespace Nexttag.Utils.Authorization
{
    public class ByPermissionAuthorizeAttribute : TypeFilterAttribute
    {
        public ByPermissionAuthorizeAttribute(string Permission = "") : base(typeof(ByPermissionFilter))
        {
            Arguments = new object[] { Permission };
        }
    }
}