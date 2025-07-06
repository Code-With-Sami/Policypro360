using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PolicyPro360.Migrations;
using PolicyPro360.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
                    var policies = _context.Tbl_Policy.Where(p => p.PolicyTypeId == categoryId).ToList();
                    return View("LifePolicy", policies);

                case "motor insurance":
                    ViewBag.CategoryId = categoryId;
                    policies = _context.Tbl_Policy.Where(p => p.PolicyTypeId == categoryId).ToList();
                    return View("MotorPolicy", policies);

                case "home insurance":
                    ViewBag.CategoryId = categoryId;
                    policies = _context.Tbl_Policy.Where(p => p.PolicyTypeId == categoryId).ToList();
                    return View("HomePolicy", policies);

                case "medical insurance":
                    ViewBag.CategoryId = categoryId;
                    policies = _context.Tbl_Policy.Where(p => p.PolicyTypeId == categoryId).ToList();
                    return View("MedicalPolicy", policies);

                default:
                    return RedirectToAction("AllPolicies");
            }
        }

        public IActionResult AllPolicies()
        {
            var policies = _context.Tbl_Policy.ToList();
            return View(policies);
        }

        public IActionResult LifePolicy(int categoryId)
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
        public async Task<IActionResult> CreateLifePolicy(Policy policy, List<PolicyAttribute> attributes, IFormFile BrochureFile)
        {
            var companyId = HttpContext.Session.GetInt32("companyId");

            if (companyId == null)
            {
                TempData["Error"] = "Please login again.";
                return RedirectToAction("Login", "Company");
            }

            // Handle brochure upload
            if (BrochureFile != null && BrochureFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/policies/brochure");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(BrochureFile.FileName)}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await BrochureFile.CopyToAsync(fileStream);
                }

                // Save path relative to wwwroot for serving on the web
                policy.BrochureUrl = "/policies/brochure/" + uniqueFileName;
            }

            policy.CompanyId = companyId.Value;
            policy.CreatedAt = DateTime.Now;
            policy.Active = policy.Active;

            _context.Tbl_Policy.Add(policy);
            await _context.SaveChangesAsync();

            if (attributes != null && attributes.Any())
            {
                foreach (var attr in attributes)
                {
                    attr.Id = 0;
                    attr.PolicyId = policy.Id;
                    _context.Tbl_PolicyAttributes.Add(attr);
                }

                await _context.SaveChangesAsync();
            }

            TempData["Success"] = "Life Insurance Policy created successfully!";
            return RedirectToAction("Index", new { categoryId = policy.PolicyTypeId });
        }

        public IActionResult EditLifePolicy(int id)
        {
            var policy = _context.Tbl_Policy.FirstOrDefault(p => p.Id == id);
            if (policy == null)
            {
                TempData["Error"] = "Policy not found.";
                return RedirectToAction("AllPolicies");
            }

            var attributes = _context.Tbl_PolicyAttributes
                .Where(a => a.PolicyId == id)
                .ToList();

            ViewBag.CategoryId = policy.PolicyTypeId;
            ViewBag.MinEntryAge = attributes.FirstOrDefault(a => a.Key == "min_entry_age")?.Value;
            ViewBag.MaxEntryAge = attributes.FirstOrDefault(a => a.Key == "max_entry_age")?.Value;
            ViewBag.MaturityBenefit = attributes.FirstOrDefault(a => a.Key == "maturity_benefit")?.Value;
            ViewBag.DeathBenefit = attributes.FirstOrDefault(a => a.Key == "death_benefit")?.Value;
            ViewBag.RidersAvailable = attributes.FirstOrDefault(a => a.Key == "riders_available")?.Value;
            ViewBag.PremiumFrequency = attributes.FirstOrDefault(a => a.Key == "premium_frequency")?.Value;
            ViewBag.PremiumPaymentTerm = attributes.FirstOrDefault(a => a.Key == "premium_payment_term")?.Value;
            ViewBag.Exclusions = attributes.FirstOrDefault(a => a.Key == "exclusions")?.Value;

            return View(policy);
        }


        public IActionResult CreateMotorPolicy()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateMotorPolicy(Policy policy, List<PolicyAttribute> attributes, IFormFile BrochureFile)
        {
            var companyId = HttpContext.Session.GetInt32("companyId");

            if (companyId == null)
            {
                TempData["Error"] = "Please login again.";
                return RedirectToAction("Login", "Company");
            }

            // Handle brochure upload
            if (BrochureFile != null && BrochureFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/policies/brochure");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(BrochureFile.FileName)}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await BrochureFile.CopyToAsync(fileStream);
                }

                // Save path relative to wwwroot for serving on the web
                policy.BrochureUrl = "/policies/brochure/" + uniqueFileName;
            }

            policy.CompanyId = companyId.Value;
            policy.CreatedAt = DateTime.Now;
            policy.Active = policy.Active;

            _context.Tbl_Policy.Add(policy);
            await _context.SaveChangesAsync();

            if (attributes != null && attributes.Any())
            {
                foreach (var attr in attributes)
                {
                    attr.Id = 0;
                    attr.PolicyId = policy.Id;
                    _context.Tbl_PolicyAttributes.Add(attr);
                }

                await _context.SaveChangesAsync();
            }

            TempData["Success"] = "Life Insurance Policy created successfully!";
            return RedirectToAction("Index", new { categoryId = policy.PolicyTypeId });
        }

        public IActionResult EditMotorPolicy(int id)
        {
            var policy = _context.Tbl_Policy.FirstOrDefault(p => p.Id == id);
            if (policy == null)
            {
                TempData["Error"] = "Policy not found.";
                return RedirectToAction("AllPolicies");
            }

            var attributes = _context.Tbl_PolicyAttributes
                .Where(a => a.PolicyId == id)
                .ToList();

            ViewBag.CategoryId = policy.PolicyTypeId;
            ViewBag.CoverageType = attributes.FirstOrDefault(a => a.Key == "coverage_type")?.Value;
            ViewBag.VehicleType = attributes.FirstOrDefault(a => a.Key == "vehicle_type")?.Value;
            ViewBag.EngineCapacity = attributes.FirstOrDefault(a => a.Key == "engine_capacity")?.Value;
            ViewBag.VehicleAgeLimit = attributes.FirstOrDefault(a => a.Key == "vehicle_age_limit")?.Value;
            ViewBag.IDV = attributes.FirstOrDefault(a => a.Key == "idv")?.Value;
            ViewBag.AddOnCovers = attributes.FirstOrDefault(a => a.Key == "add_on_covers")?.Value;
            ViewBag.ClaimLimit = attributes.FirstOrDefault(a => a.Key == "claim_limit")?.Value;
            ViewBag.Exclusions = attributes.FirstOrDefault(a => a.Key == "exclusions")?.Value;

            return View(policy);
        }

        public IActionResult CreateHomePolicy()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateHomePolicy(Policy policy, List<PolicyAttribute> attributes, IFormFile BrochureFile)
        {
            var companyId = HttpContext.Session.GetInt32("companyId");

            if (companyId == null)
            {
                TempData["Error"] = "Please login again.";
                return RedirectToAction("Login", "Company");
            }

            // Handle brochure upload
            if (BrochureFile != null && BrochureFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/policies/brochure");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(BrochureFile.FileName)}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await BrochureFile.CopyToAsync(fileStream);
                }

                // Save path relative to wwwroot for serving on the web
                policy.BrochureUrl = "/policies/brochure/" + uniqueFileName;
            }

            policy.CompanyId = companyId.Value;
            policy.CreatedAt = DateTime.Now;
            policy.Active = policy.Active;

            _context.Tbl_Policy.Add(policy);
            await _context.SaveChangesAsync();

            if (attributes != null && attributes.Any())
            {
                foreach (var attr in attributes)
                {
                    attr.Id = 0;
                    attr.PolicyId = policy.Id;
                    _context.Tbl_PolicyAttributes.Add(attr);
                }

                await _context.SaveChangesAsync();
            }

            TempData["Success"] = "Life Insurance Policy created successfully!";
            return RedirectToAction("Index", new { categoryId = policy.PolicyTypeId });
        }

        public IActionResult EditHomePolicy(int id)
        {
            var policy = _context.Tbl_Policy.FirstOrDefault(p => p.Id == id);
            if (policy == null)
            {
                TempData["Error"] = "Policy not found.";
                return RedirectToAction("AllPolicies");
            }

            var attributes = _context.Tbl_PolicyAttributes
                .Where(a => a.PolicyId == id)
                .ToList();

            ViewBag.CategoryId = policy.PolicyTypeId;
            ViewBag.PropertyType = attributes.FirstOrDefault(a => a.Key == "property_type")?.Value;
            ViewBag.CoverageType = attributes.FirstOrDefault(a => a.Key == "coverage_type")?.Value;
            ViewBag.PropertyAge = attributes.FirstOrDefault(a => a.Key == "property_age")?.Value;
            ViewBag.ConstructionType = attributes.FirstOrDefault(a => a.Key == "construction_type")?.Value;
            ViewBag.LocationRisk = attributes.FirstOrDefault(a => a.Key == "location_risk")?.Value;
            ViewBag.AddOnCovers = attributes.FirstOrDefault(a => a.Key == "add_on_covers")?.Value;
            ViewBag.Exclusions = attributes.FirstOrDefault(a => a.Key == "exclusions")?.Value;

            return View(policy);
        }

        public IActionResult CreateMedicalPolicy()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateMedicalPolicy(Policy policy, List<PolicyAttribute> attributes, IFormFile BrochureFile)
        {
            var companyId = HttpContext.Session.GetInt32("companyId");

            if (companyId == null)
            {
                TempData["Error"] = "Please login again.";
                return RedirectToAction("Login", "Company");
            }

            // Handle brochure upload
            if (BrochureFile != null && BrochureFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/policies/brochure");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(BrochureFile.FileName)}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await BrochureFile.CopyToAsync(fileStream);
                }

                // Save path relative to wwwroot for serving on the web
                policy.BrochureUrl = "/policies/brochure/" + uniqueFileName;
            }

            policy.CompanyId = companyId.Value;
            policy.CreatedAt = DateTime.Now;
            policy.Active = policy.Active;

            _context.Tbl_Policy.Add(policy);
            await _context.SaveChangesAsync();

            if (attributes != null && attributes.Any())
            {
                foreach (var attr in attributes)
                {
                    attr.Id = 0;
                    attr.PolicyId = policy.Id;
                    _context.Tbl_PolicyAttributes.Add(attr);
                }

                await _context.SaveChangesAsync();
            }

            TempData["Success"] = "Life Insurance Policy created successfully!";
            return RedirectToAction("Index", new { categoryId = policy.PolicyTypeId });
        }

        public IActionResult EditMedicalPolicy(int id)
        {
            var policy = _context.Tbl_Policy.FirstOrDefault(p => p.Id == id);
            if (policy == null)
            {
                TempData["Error"] = "Policy not found.";
                return RedirectToAction("AllPolicies");
            }

            var attributes = _context.Tbl_PolicyAttributes
                .Where(a => a.PolicyId == id)
                .ToList();

            ViewBag.CategoryId = policy.PolicyTypeId;
            ViewBag.CoverageType = attributes.FirstOrDefault(a => a.Key == "coverage_type")?.Value;
            ViewBag.MinEntryAge = attributes.FirstOrDefault(a => a.Key == "min_entry_age")?.Value;
            ViewBag.MaxEntryAge = attributes.FirstOrDefault(a => a.Key == "max_entry_age")?.Value;
            ViewBag.PreHospitalization = attributes.FirstOrDefault(a => a.Key == "pre_hospitalization")?.Value;
            ViewBag.PostHospitalization = attributes.FirstOrDefault(a => a.Key == "post_hospitalization")?.Value;
            ViewBag.DaycareProcedures = attributes.FirstOrDefault(a => a.Key == "daycare_procedures")?.Value;
            ViewBag.CoPaymentClause = attributes.FirstOrDefault(a => a.Key == "co_payment_clause")?.Value;
            ViewBag.WaitingPeriod = attributes.FirstOrDefault(a => a.Key == "waiting_period")?.Value;
            ViewBag.AddOnCovers = attributes.FirstOrDefault(a => a.Key == "add_on_covers")?.Value;
            ViewBag.Exclusions = attributes.FirstOrDefault(a => a.Key == "exclusions")?.Value;

            return View(policy);
        }

        // Edit Post Action For Editing Policies

        [HttpPost]
        public async Task<IActionResult> EditPolicy(Policy policy, List<PolicyAttribute> attributes, IFormFile BrochureFile)
        {
            var existingPolicy = _context.Tbl_Policy.FirstOrDefault(p => p.Id == policy.Id);

            if (existingPolicy == null)
            {
                TempData["Error"] = "Policy not found.";
                return RedirectToAction("AllPolicies");
            }

            // Brochure upload (if new)
            if (BrochureFile != null && BrochureFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/policies/brochure");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(BrochureFile.FileName)}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await BrochureFile.CopyToAsync(fileStream);
                }

                existingPolicy.BrochureUrl = "/policies/brochure/" + uniqueFileName;
            }

            // Update fields
            existingPolicy.Name = policy.Name;
            existingPolicy.Description = policy.Description;
            existingPolicy.SumInsured = policy.SumInsured;
            existingPolicy.Premium = policy.Premium;
            existingPolicy.Tenure = policy.Tenure;
            existingPolicy.TermsConditions = policy.TermsConditions;
            existingPolicy.Active = policy.Active;

            // Remove existing attributes and re-add updated ones
            var oldAttributes = _context.Tbl_PolicyAttributes.Where(a => a.PolicyId == policy.Id);
            _context.Tbl_PolicyAttributes.RemoveRange(oldAttributes);

            if (attributes != null && attributes.Any())
            {
                foreach (var attr in attributes)
                {
                    attr.Id = 0;
                    attr.PolicyId = policy.Id;
                    _context.Tbl_PolicyAttributes.Add(attr);
                }
            }

            await _context.SaveChangesAsync();

            TempData["Success"] = "Policy updated successfully!";
            return RedirectToAction("Index", new { categoryId = existingPolicy.PolicyTypeId });
        }

        //Delete Action For Deleting Policy

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var policy = _context.Tbl_Policy.Find(id);
            if (policy != null)
            {
                // Step 1: Delete related policy attributes
                var attributes = _context.Tbl_PolicyAttributes
                    .Where(attr => attr.PolicyId == id)
                    .ToList();

                _context.Tbl_PolicyAttributes.RemoveRange(attributes);

                // Step 2: Delete the policy
                _context.Tbl_Policy.Remove(policy);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Policy and its attributes deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Policy not found.";
            }

            return RedirectToAction("AllPolicies");
        }
    }
}
