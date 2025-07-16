using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using PolicyPro360.Models;
using System;
using System.IO;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;



namespace PolicyPro360.Controllers.Company
{

    public class CompanyUsersController : BaseCompanyController
    {
        private readonly myContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CompanyUsersController(myContext context, IWebHostEnvironment webHostEnvironment) : base(context)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var companyId = HttpContext.Session.GetInt32("companyId");

            var userPolicies = _context.Tbl_UserPolicy
               .Include(up => up.Policy)
               .ThenInclude(p => p.Company)
               .Include(up => up.Policy)
               .ThenInclude(p => p.Category)
               .Include(up => up.User)
               .Where(up => up.Policy.CompanyId == companyId)
               .ToList();
            return View(userPolicies);
        }

        public IActionResult ViewUserPolicy(int id)
        {
            var policy = _context.Tbl_UserPolicy
               .Include(up => up.User)
               .Include(up => up.Policy)
               .ThenInclude(p => p.Company)
               .FirstOrDefault(up => up.Id == id);

            if (policy == null)
                return NotFound();

            return View(policy);
        }


        public IActionResult UserPoliciesByCategory(int categoryId)
        {
            var companyId = HttpContext.Session.GetInt32("companyId");

            var userPolicies = _context.Tbl_UserPolicy
                .Include(up => up.Policy)
                .ThenInclude(p => p.Company)
                .Include(up => up.Policy)
                .ThenInclude(p => p.Category)
                .Include(up => up.User)
                .Where(up => up.Policy != null && up.Policy.PolicyTypeId == categoryId && up.Policy.CompanyId == companyId)
                .ToList();
            return View("Index", userPolicies);
        }

        public IActionResult UserClaims()
        {
            var companyId = HttpContext.Session.GetInt32("companyId");
            var userClaims = _context.Tbl_UserClaim
                .Where(c => _context.Tbl_Policy.Any(p => p.Id == c.PolicyId && p.CompanyId == companyId))
                .Include(c => c.Category)
                .Include(c => c.Policy)
                .Include(c => c.Users)
                .ToList();

            return View(userClaims);

        }

        public IActionResult ViewUserClaim(int id)
        {
            var userclaim = _context.Tbl_UserClaim
               .Include(up => up.Users)
               .Include(up => up.Policy)
               .ThenInclude(p => p.Company)
               .FirstOrDefault(up => up.Id == id);

            if (userclaim == null)
                return NotFound();

            return View(userclaim);

        }


        public IActionResult UserClaimsByCategory(int categoryId)
        {
            var companyId = HttpContext.Session.GetInt32("companyId");

            var userClaims = _context.Tbl_UserClaim
                .Include(c => c.Users)
                .Include(c => c.Policy)
                    .ThenInclude(p => p.Category)
                .Include(c => c.Policy)
                    .ThenInclude(p => p.Company)
                .Where(c => c.Policy != null &&
                            c.Policy.PolicyTypeId == categoryId &&
                            c.Policy.CompanyId == companyId)
                .ToList();

            return View("UserClaims", userClaims);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            ViewBag.PolicyCategories = _context.Tbl_Category.Where(c => c.Status).ToList();
        }
    }
}
