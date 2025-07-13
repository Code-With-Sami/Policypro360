using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using PolicyPro360.Models;

namespace PolicyPro360.Controllers.Admin
{
    public class AdminCompanyController : BaseAdminController
    {
        private readonly myContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminCompanyController(myContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var companies = await _context.Tbl_Company.ToListAsync();
            return View(companies);
        }

        public IActionResult PendingCompanies()
        {
            var companies = _context.Tbl_Company.Where(c => c.Status == "Pending").ToList();
            return View(companies);
        }

        public IActionResult ApprovedCompanies()
        {
            var companies = _context.Tbl_Company.Where(c => c.Status == "Approved").ToList();
            return View(companies);
        }

        public IActionResult RejectedCompanies()
        {
            var companies = _context.Tbl_Company.Where(c => c.Status == "Rejected").ToList();
            return View(companies);
        }

        public IActionResult Approve(int id)
        {
            var company = _context.Tbl_Company.Find(id);
            if (company != null)
            {
                company.Status = "Approved";
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Reject(int id)
        {
            var company = _context.Tbl_Company.Find(id);
            if (company != null)
            {
                company.Status = "Rejected";
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public IActionResult AllCompanyPolicies()
        {
            var policies = _context.Tbl_Policy
                .Include(p => p.Category)
                .Include(p => p.Company)
                .ToList();
            return View(policies);
        }

        public IActionResult CompanyPoliciesByCategory(int categoryId)
        {
            var policies = _context.Tbl_Policy
                .Include(p => p.Category)
                .Include(p => p.Company)
                .Where(p => p.PolicyTypeId == categoryId)
                .ToList();
            return View("AllCompanyPolicies", policies);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            ViewBag.PolicyCategories = _context.Tbl_Category.Where(c => c.Status).ToList();
        }

        public IActionResult LifeCompanyPolicy()
        {
            var category = _context.Tbl_Category.FirstOrDefault(c => c.Name.ToLower().Contains("life"));
            if (category == null)
                return View("AllCompanyPolicies", new List<PolicyPro360.Models.Policy>());
            var policies = _context.Tbl_Policy
                .Include(p => p.Category)
                .Include(p => p.Company)
                .Where(p => p.PolicyTypeId == category.Id)
                .ToList();
            return View("AllCompanyPolicies", policies);
        }

        public IActionResult MotorCompanyPolicy()
        {
            var category = _context.Tbl_Category.FirstOrDefault(c => c.Name.ToLower().Contains("motor"));
            if (category == null)
                return View("AllCompanyPolicies", new List<PolicyPro360.Models.Policy>());
            var policies = _context.Tbl_Policy
                .Include(p => p.Category)
                .Include(p => p.Company)
                .Where(p => p.PolicyTypeId == category.Id)
                .ToList();
            return View("AllCompanyPolicies", policies);
        }

        public IActionResult HomeCompanyPolicy()
        {
            var category = _context.Tbl_Category.FirstOrDefault(c => c.Name.ToLower().Contains("home"));
            if (category == null)
                return View("AllCompanyPolicies", new List<PolicyPro360.Models.Policy>());
            var policies = _context.Tbl_Policy
                .Include(p => p.Category)
                .Include(p => p.Company)
                .Where(p => p.PolicyTypeId == category.Id)
                .ToList();
            return View("AllCompanyPolicies", policies);
        }

        public IActionResult MedicalCompanyPolicy()
        {
            var category = _context.Tbl_Category.FirstOrDefault(c => c.Name.ToLower().Contains("medical"));
            if (category == null)
                return View("AllCompanyPolicies", new List<PolicyPro360.Models.Policy>());
            var policies = _context.Tbl_Policy
                .Include(p => p.Category)
                .Include(p => p.Company)
                .Where(p => p.PolicyTypeId == category.Id)
                .ToList();
            return View("AllCompanyPolicies", policies);
        }



    }
}
