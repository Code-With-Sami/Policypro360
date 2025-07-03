using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PolicyPro360.Attributes;
using PolicyPro360.Models;
using System.Linq;

namespace PolicyPro360.Controllers.Company
{
    public class BaseCompanyController : Controller
    {
        protected readonly myContext _db;

        public BaseCompanyController(myContext db)
        {
            _db = db;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var actionHasAllowAnonymous = context.ActionDescriptor.EndpointMetadata
                .Any(em => em.GetType() == typeof(AllowAnonymousCompanyAttribute));

 
            if (!actionHasAllowAnonymous && context.HttpContext.Session.GetString("companyname") == null)
            {
                context.Result = new RedirectToActionResult("Login", "Company", null);
            }
            else
            {
        
                var categories = _db.Tbl_Category.Where(c => c.Status == true).ToList();
                ViewBag.Categories = categories;
            }

            base.OnActionExecuting(context);
        }
    }
}