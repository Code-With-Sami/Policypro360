// BaseCompanyController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PolicyPro360.Attributes; 

namespace PolicyPro360.Controllers.Company
{
    public class BaseCompanyController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var actionHasAllowAnonymous = context.ActionDescriptor.EndpointMetadata
                .Any(em => em.GetType() == typeof(AllowAnonymousCompanyAttribute));

            if (!actionHasAllowAnonymous && context.HttpContext.Session.GetString("companyname") == null)
            {
                context.Result = new RedirectToActionResult("Login", "Company", null);
            }

            base.OnActionExecuting(context);
        }
    }
}