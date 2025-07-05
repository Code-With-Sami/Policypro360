using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PolicyPro360.Migrations;
using PolicyPro360.Models;

namespace PolicyPro360.Controllers.Company
{
    public class CompanyPolicyController : BaseCompanyController
    {
        private readonly myContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CompanyPolicyController(myContext context, IWebHostEnvironment webHostEnvironment) : base(context)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index(int categoryId)
        {
            var category = _context.Tbl_Category.FirstOrDefault(c => c.Id == categoryId);

            if (category == null)
            {
                return RedirectToAction("AllPolicies");
            }

            //dynamically load view based on category name
            switch (category.Name.ToLower())
            {
                case "life insurance":
                    ViewBag.CategoryId = categoryId;
                    return View("LifePolicy");

                case "motor insurance":
                    ViewBag.CategoryId = categoryId;
                    return View("MotorPolicy");

                case "home insurance":
                    ViewBag.CategoryId = categoryId;
                    return View("HomePolicy");

                case "medical insurance":
                    ViewBag.CategoryId = categoryId;
                    return View("MedicalPolicy");

                default:
                    return RedirectToAction("AllPolicies");
            }
        }

        public IActionResult AllPolicies()
        {
            return View();
        }

        public IActionResult LifePolicy()
        {
            return View();
        }

        public IActionResult MotorPolicy()
        {
            return View();
        }

        public IActionResult HomePolicy()
        {
            return View();
        }

        public IActionResult MedicalPolicy()
        {
            return View();
        }

        public IActionResult Create (int categoryId)
        {
            var category = _context.Tbl_Category.FirstOrDefault(c => c.Id == categoryId);

            if (category == null)
            {
                return NotFound();
            }

            //dynamically load view based on category name
            switch (category.Name.ToLower())
            {
                case "life insurance":
                    ViewBag.CategoryId = categoryId;
                    return View("CreateLifePolicy");

                case "motor insurance":
                    ViewBag.CategoryId = categoryId;
                    return View("CreateMotorPolicy");

                case "home insurance":
                    ViewBag.CategoryId = categoryId;
                    return View("CreateHomePolicy");

                case "medical insurance":
                    ViewBag.CategoryId = categoryId;
                    return View("CreateMedicalPolicy");

                default:
                    return NotFound();
            }
        }

        public IActionResult CreateLifePolicy()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateLifePolicy(Policy policy, List<PolicyAttribute> attributes)
        {
            // Get company ID from session
            var companyId = HttpContext.Session.GetInt32("companyId");

            if (companyId == null)
            {
                TempData["Error"] = "Please login again.";
                return RedirectToAction("Login", "Company");
            }

            policy.CompanyId = companyId.Value;
            policy.CreatedAt = DateTime.Now;
            policy.Active = true;

            _context.Tbl_Policy.Add(policy);
            await _context.SaveChangesAsync();

            // Save dynamic attributes
            if (attributes != null && attributes.Any())
            {
                foreach (var attr in attributes)
                {
                    attr.Id = 0; //  Optional safety line to force EF to auto-generate Id
                    attr.PolicyId = policy.Id;
                    _context.Tbl_PolicyAttributes.Add(attr);
                }

                await _context.SaveChangesAsync();
            }

            TempData["Success"] = "Life Insurance Policy created successfully!";
            return RedirectToAction("Index", new { categoryId = policy.PolicyTypeId });

        }

        public IActionResult CreateMotorPolicy()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateMotorPolicy(Policy policy, List<PolicyAttribute> attributes)
        {
            // Get company ID from session
            var companyId = HttpContext.Session.GetInt32("companyId");

            if (companyId == null)
            {
                TempData["Error"] = "Please login again.";
                return RedirectToAction("Login", "Company");
            }

            policy.CompanyId = companyId.Value;
            policy.CreatedAt = DateTime.Now;
            policy.Active = true;

            _context.Tbl_Policy.Add(policy);
            await _context.SaveChangesAsync();

            // Save dynamic attributes
            if (attributes != null && attributes.Any())
            {
                foreach (var attr in attributes)
                {
                    attr.Id = 0; //  Optional safety line to force EF to auto-generate Id
                    attr.PolicyId = policy.Id;
                    _context.Tbl_PolicyAttributes.Add(attr);
                }

                await _context.SaveChangesAsync();
            }

            TempData["Success"] = "Life Insurance Policy created successfully!";
            return RedirectToAction("Index", new { categoryId = policy.PolicyTypeId });

        }

        public IActionResult CreateHomePolicy()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateHomePolicy(Policy policy, List<PolicyAttribute> attributes)
        {
            // Get company ID from session
            var companyId = HttpContext.Session.GetInt32("companyId");
            
            if (companyId == null)
            {
                TempData["Error"] = "Please login again.";
                return RedirectToAction("Login", "Company");
            }

            policy.CompanyId = companyId.Value;
            policy.CreatedAt = DateTime.Now;
            policy.Active = true;

            _context.Tbl_Policy.Add(policy);
            await _context.SaveChangesAsync();

            // Save dynamic attributes
            if (attributes != null && attributes.Any())
            {
                foreach (var attr in attributes)
                {
                    attr.Id = 0; //  Optional safety line to force EF to auto-generate Id
                    attr.PolicyId = policy.Id;
                    _context.Tbl_PolicyAttributes.Add(attr);
                }

                await _context.SaveChangesAsync();
            }

            TempData["Success"] = "Life Insurance Policy created successfully!";
            return RedirectToAction("Index", new { categoryId = policy.PolicyTypeId });

        }

        public IActionResult CreateMedicalPolicy()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateMedicalPolicy(Policy policy, List<PolicyAttribute> attributes)
        {
            // Get company ID from session
            var companyId = HttpContext.Session.GetInt32("companyId");

            if (companyId == null)
            {
                TempData["Error"] = "Please login again.";
                return RedirectToAction("Login", "Company");
            }

            policy.CompanyId = companyId.Value;
            policy.CreatedAt = DateTime.Now;
            policy.Active = true;

            _context.Tbl_Policy.Add(policy);
            await _context.SaveChangesAsync();

            // Save dynamic attributes
            if (attributes != null && attributes.Any())
            {
                foreach (var attr in attributes)
                {
                    attr.Id = 0; //  Optional safety line to force EF to auto-generate Id
                    attr.PolicyId = policy.Id;
                    _context.Tbl_PolicyAttributes.Add(attr);
                }

                await _context.SaveChangesAsync();
            }

            TempData["Success"] = "Life Insurance Policy created successfully!";
            return RedirectToAction("Index", new { categoryId = policy.PolicyTypeId });

        }
    }
}
