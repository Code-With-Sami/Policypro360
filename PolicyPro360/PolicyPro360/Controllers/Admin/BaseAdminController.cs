using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PolicyPro360.Controllers.Admin
{
    public class BaseAdminController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var actionName = context.ActionDescriptor.RouteValues["action"];
            var controllerName = context.ActionDescriptor.RouteValues["controller"];

            if ((controllerName == "Admin" && (actionName == "Login" || actionName == "Register")))
            {
                base.OnActionExecuting(context);
                return;
            }

            context.HttpContext.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            context.HttpContext.Response.Headers["Pragma"] = "no-cache";
            context.HttpContext.Response.Headers["Expires"] = "0";

            if (context.HttpContext.Session.GetString("name") == null)
            {
                context.Result = new RedirectToActionResult("Login", "Admin", null);
            }
            base.OnActionExecuting(context);
        }
    }
}