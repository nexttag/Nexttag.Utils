using Microsoft.AspNetCore.Mvc;
using Nexttag.Authorization;

namespace Nexttag.Authorization;

public class ByPermissionAuthorizeAttribute : TypeFilterAttribute
{
    public ByPermissionAuthorizeAttribute(string Permission = "") : base(typeof(ByPermissionFilter))
    {
        Arguments = new object[] { Permission };
    }
}