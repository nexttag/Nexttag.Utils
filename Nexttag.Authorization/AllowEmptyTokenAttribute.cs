using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Nexttag.Authorization;

public class AllowEmptyTokenAttribute : TypeFilterAttribute
{
    public AllowEmptyTokenAttribute() : base(typeof(AllowEmptyTokenAttribute))
    {
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.Result == null)
        {

        }
    }
}