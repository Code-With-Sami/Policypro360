using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using PolicyPro360.Migrations;
using PolicyPro360.Models;
using PolicyPro360.ViewModels;
using System.Security.Claims;
using System.Text.Json;

namespace PolicyPro360.Controllers.User
{
    public class BaseUserController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var actionName = context.ActionDescriptor.RouteValues["action"]?.ToLower();
            var controllerName = context.ActionDescriptor.RouteValues["controller"];


            var allowAnonymousActions = new[] { "index", "signin", "signup", "about", "contact", "insurance", "policy", "viewpolicydetails", "calculatepremium", "premiumresult", "terms", "privacy", "news", "faq", "life", "home", "motor", "medical", "makeaclaim", "makeahomeclaim", "makealifeclaim", "makeamotorclaim", "makeamedicalclaim", "makeloanagainstpolicy" };


            context.HttpContext.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            context.HttpContext.Response.Headers["Pragma"] = "no-cache";
            context.HttpContext.Response.Headers["Expires"] = "0";

            if (!allowAnonymousActions.Contains(actionName))
            {
                if (context.HttpContext.Session.GetString("userName") == null)
                {
                    context.Result = new RedirectToActionResult("SignIn", "UserHome", null);
                }
            }
            base.OnActionExecuting(context);
        }
    }

    public class UserHomeController : BaseUserController
    {

        private readonly myContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserHomeController(myContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult Insurance()
        {
            var categories = _context.Tbl_Category
                .Where(c => c.Status)
                .ToList();
            return View(categories);
        }
        public IActionResult Policy()
        {
            try
            {
                var policies = _context.Tbl_Policy
                    .Include(p => p.Category)
                    .Where(p => p.Active && p.Category != null)
                    .ToList();

                if (policies == null || !policies.Any())
                {
                    ViewBag.ErrorMessage = "No policies found from database.";
                }

                return View(policies);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error fetching policies: " + ex.Message;
                return View(new List<Policy>());
            }
        }
        public IActionResult ViewPolicyDetails(int id)
        {
            var policy = _context.Tbl_Policy
                                 .Include(p => p.Category)
                                 .Include(p => p.Attributes)
                                 .Include(p => p.Company)
                                 .FirstOrDefault(p => p.Id == id);

            if (policy == null)
            {
                TempData["ErrorMessage"] = "Policy not found.";
                return RedirectToAction("Policy", "UserHome");
            }

            return View(policy);
        }
        [HttpGet]
        public IActionResult CalculatePremium(int id)
        {
            var policy = _context.Tbl_Policy
                                 .Include(p => p.Category)
                                 .FirstOrDefault(p => p.Id == id);

            if (policy == null)
            {
                return NotFound();
            }

            var viewModel = new PremiumCalculationViewModel
            {
                PolicyId = policy.Id,
                PolicyName = policy.Name,
                PolicyType = policy.Category.Name,
                BasePremium = policy.Premium
            };
            return View("CalculatePremium", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CalculatePremium(PremiumCalculationViewModel model)
        {
            try
            {
                var policyTypeClean = model.PolicyType?.Trim().ToLower() ?? string.Empty;
                if (policyTypeClean.Contains("home"))
                {
                    ModelState.Remove("VehicleType");
                    ModelState.Remove("UserAge");
                    ModelState.Remove("LifeCoverageAmount");
                    ModelState.Remove("NumberOfDependents");
                }
                else if (policyTypeClean.Contains("motor"))
                {
                    ModelState.Remove("PropertyValue");
                    ModelState.Remove("LocationRisk");
                    ModelState.Remove("UserAge");
                    ModelState.Remove("LifeCoverageAmount");
                    ModelState.Remove("NumberOfDependents");
                }
                else if (policyTypeClean.Contains("life"))
                {
                    ModelState.Remove("VehicleType");
                    ModelState.Remove("VehicleValue");
                    ModelState.Remove("PropertyValue");
                    ModelState.Remove("LocationRisk");
                    ModelState.Remove("NumberOfDependents");
                }
                else if (policyTypeClean.Contains("medical"))
                {
                    ModelState.Remove("VehicleType");
                    ModelState.Remove("VehicleValue");
                    ModelState.Remove("PropertyValue");
                    ModelState.Remove("LocationRisk");
                    ModelState.Remove("LifeCoverageAmount");
                }

                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    TempData["ErrorMessage"] = "ModelState Invalid: " + errors;
                    var policyForView = _context.Tbl_Policy.Include(p => p.Category).FirstOrDefault(p => p.Id == model.PolicyId);
                    if (policyForView != null)
                    {
                        model.PolicyName = policyForView.Name;
                        model.PolicyType = policyForView.Category.Name;
                        model.BasePremium = policyForView.Premium;
                    }
                    return View(model);
                }

                var policy = _context.Tbl_Policy.Find(model.PolicyId);
                if (policy == null)
                {
                    TempData["ErrorMessage"] = "Policy not found.";
                    return View(model);
                }

                decimal basePremium = policy.Premium;
                decimal finalPremium = 0;

                if (policyTypeClean.Contains("motor"))
                {
                    if (model.VehicleValue.HasValue)
                    {
                        finalPremium = basePremium + (model.VehicleValue.Value * 0.03m);
                    }
                    else
                    {
                        finalPremium = basePremium;
                    }
                }
                else if (policyTypeClean.Contains("life"))
                {
                    if (model.UserAge.HasValue && model.LifeCoverageAmount.HasValue)
                    {
                        finalPremium = basePremium;
                        if (model.UserAge.Value > 40) { finalPremium += 5000; }
                        else if (model.UserAge.Value > 30) { finalPremium += 2000; }
                        finalPremium += (model.LifeCoverageAmount.Value * 0.001m);
                    }
                    else
                    {
                        finalPremium = basePremium;
                    }
                }
                else if (policyTypeClean.Contains("home"))
                {
                    if (model.PropertyValue.HasValue)
                    {
                        finalPremium = basePremium + (model.PropertyValue.Value * 0.005m);
                        if (model.LocationRisk?.ToLower() == "urban") { finalPremium += (basePremium * 0.1m); }
                    }
                    else
                    {
                        finalPremium = basePremium;
                    }
                }
                else if (policyTypeClean.Contains("medical"))
                {
                    if (model.UserAge.HasValue && model.NumberOfDependents.HasValue)
                    {
                        finalPremium = basePremium;
                        if (model.UserAge.Value > 50) { finalPremium += 8000; }
                        else if (model.UserAge.Value > 35) { finalPremium += 3000; }
                        if (model.NumberOfDependents.Value > 1) { finalPremium += ((model.NumberOfDependents.Value - 1) * 2500); }
                    }
                    else
                    {
                        finalPremium = basePremium;
                    }
                }
                else
                {
                    finalPremium = basePremium;
                }

                model.CalculatedPremium = Math.Round(finalPremium, 2);
                model.BasePremium = basePremium;
                TempData["PremiumResult"] = JsonSerializer.Serialize(model);
                return RedirectToAction("PremiumResult");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Exception: " + ex.Message;
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult PremiumResult()
        {
            if (TempData["PremiumResult"] is string serializedModel)
            {
                var model = JsonSerializer.Deserialize<PremiumCalculationViewModel>(serializedModel);
                return View(model);
            }
            return RedirectToAction("Policy");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ProceedToCheckout(PremiumCalculationViewModel model)
        {
            TempData["CheckoutModel"] = JsonSerializer.Serialize(model);
            return RedirectToAction("Checkout");
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            int? userId = HttpContext.Session.GetInt32("userId");
            if (userId == null)
            {
                TempData["ErrorMessage"] = "User not logged in.";
                return RedirectToAction("SignIn");
            }
            if (TempData["CheckoutModel"] is string serializedModel)
            {
                var model = JsonSerializer.Deserialize<PremiumCalculationViewModel>(serializedModel);
                TempData.Keep("CheckoutModel");
                return View(model);
            }
            return RedirectToAction("Policy");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitManualPayment(string PayerName, string PayerEmail, decimal Amount, string Reference, int PolicyId, decimal CalculatedPremium, decimal? CoverageAmount)
        {
            int? userId = HttpContext.Session.GetInt32("userId");
            if (userId == null)
            {
                TempData["ErrorMessage"] = "User not logged in.";
                return RedirectToAction("SignIn");
            }

            // Step 1: Get the policy from database to access SumInsured and CompanyId
            var policy = _context.Tbl_Policy.FirstOrDefault(p => p.Id == PolicyId);
            if (policy == null)
            {
                TempData["ErrorMessage"] = "Policy not found.";
                return RedirectToAction("Policy");
            }

            int companyId = policy.CompanyId;

            // Step 2: Determine final coverage amount
            decimal finalCoverageAmount = 0;
            if (CoverageAmount.HasValue && CoverageAmount.Value > 0)
            {
                finalCoverageAmount = CoverageAmount.Value;
            }
            else
            {
                finalCoverageAmount = policy.SumInsured; // fallback
            }

            // Step 3: Save payment
            var payment = new UserPayment
            {
                PayerName = PayerName,
                PayerEmail = PayerEmail,
                Amount = Amount,
                Reference = Reference
            };
            _context.Tbl_UserPayment.Add(payment);

            // Step 4: Save user policy
            DateTime purchaseDate = DateTime.Now;
            DateTime expiryDate = purchaseDate.AddYears(1);

            var userPolicy = new UserPolicy
            {
                PolicyId = PolicyId,
                UserId = userId.Value,
                CalculatedPremium = CalculatedPremium,
                CoverageAmount = finalCoverageAmount,
                PurchaseDate = purchaseDate,
                ExpiryDate = expiryDate,
                Status = "Active"
            };
            _context.Tbl_UserPolicy.Add(userPolicy);

            // Step 5: Split payment and update wallets and transaction history
            decimal adminAmount = Math.Round(Amount * 0.25m, 2);
            decimal companyAmount = Amount - adminAmount; // 75%

            // AdminWallet entry
            var adminWallet = new AdminWallet
            {
                UserId = userId.Value,
                CompanyId = companyId,
                PolicyId = PolicyId,
                Amount = adminAmount,
                Description = $"25% commission from user payment (UserId: {userId.Value}) for PolicyId: {PolicyId}",
                TransactionDate = DateTime.Now
            };
            _context.Set<AdminWallet>().Add(adminWallet);

            // CompanyWallet entry
            var companyWallet = new CompanyWallet
            {
                UserId = userId.Value,
                CompanyId = companyId,
                PolicyId = PolicyId,
                Amount = companyAmount,
                Description = $"75% share from user payment (UserId: {userId.Value}) for PolicyId: {PolicyId}",
                TransactionDate = DateTime.Now
            };
            _context.Set<CompanyWallet>().Add(companyWallet);

            // TransactionHistory for Admin
            var adminTransaction = new TransactionHistory
            {
                FromType = "User",
                FromId = userId.Value,
                ToType = "Admin",
                ToId = 1,
                CompanyId = companyId,
                PolicyId = PolicyId,
                Amount = adminAmount,
                Purpose = "Admin commission from user payment",
                Date = DateTime.Now
            };
            _context.Set<TransactionHistory>().Add(adminTransaction);

            // TransactionHistory for Company
            var companyTransaction = new TransactionHistory
            {
                FromType = "User",
                FromId = userId.Value,
                ToType = "Company",
                ToId = companyId,
                CompanyId = companyId,
                PolicyId = PolicyId,
                Amount = companyAmount,
                Purpose = "Company share from user payment",
                Date = DateTime.Now
            };
            _context.Set<TransactionHistory>().Add(companyTransaction);

            _context.SaveChanges();

            TempData["SuccessMessage"] = "Congratulations! Your payment was successful and your insurance policy is now active.";
            return RedirectToAction("Checkout");
        }


        public IActionResult Terms()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult News()
        {
            return View();
        }
        public IActionResult Faq()
        {
            return View();
        }
        public IActionResult Life()
        {
            return View();
        }
        public IActionResult Home()
        {
            return View();
        }
        public IActionResult Motor()
        {
            return View();
        }
        public IActionResult Medical()
        {
            return View();
        }
        public IActionResult Makeaclaim()
        {
            return View();
        }
        public IActionResult MakeaHomeclaim()
        {
            return View();
        }
        public IActionResult MakeaLifeclaim()
        {
            return View();
        }
        public IActionResult MakeaMotorclaim()
        {
            return View();
        }
        public IActionResult MakeaMedicalclaim()
        {
            return View();
        }
        public IActionResult MakeLoanAgainstPolicy()
        {
            return View();
        }
        public IActionResult SignIn()
        {
            if (HttpContext.Session.GetString("userName") != null)
            {
                return RedirectToAction("Dashboard");
            }
            return View();
        }

        [HttpPost]
        public IActionResult SignIn(string userEmail, string userPassword)
        {
            var user = _context.Tbl_Users.FirstOrDefault(u => u.Email == userEmail && u.Password == userPassword);
            if (user != null)
            {

                HttpContext.Session.SetString("userName", value: user.Name);
                HttpContext.Session.SetInt32("userId", value: user.Id);
                HttpContext.Session.SetString("userEmail", value: user.Email);
                HttpContext.Session.SetString("userImg", value: user.ProfileImagePath);
                TempData["success"] = "Login successful!";
                return RedirectToAction("Dashboard");
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid email or password.";

            }
            return View();
        }
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(Users model)
        {
            if (model.ProfileImage != null)
            {
                model.ProfileImagePath = UploadFile(model.ProfileImage);
            }
            else
            {
                model.ProfileImagePath = string.Empty;
            }

            ModelState.Remove("ProfileImagePath");

            if (await _context.Tbl_Users.AnyAsync(u => u.Email == model.Email))
            {
                ModelState.AddModelError("Email", "This email address is already in use.");
            }

            if (await _context.Tbl_Users.AnyAsync(u => u.Username == model.Username))
            {
                ModelState.AddModelError("Username", "This username is already taken.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                _context.Tbl_Users.Add(model);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"User '{model.Username}' has been created successfully!";
                return RedirectToAction("SignIn");
            }
            catch (Exception ex)
            {
                var innerMessage = ex.InnerException?.Message ?? ex.Message;
                ModelState.AddModelError("", "Database Error: " + innerMessage);
                return View(model);
            }
        }

        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("userName") != null)
            {
                ViewBag.name = HttpContext.Session.GetString("userName");
                ViewBag.email = HttpContext.Session.GetString("userEmail");
                var userId = HttpContext.Session.GetInt32("userId");

                var myPolicies = _context.Tbl_UserPolicy
                    .Where(up => up.UserId == userId)  // or the correct field name
                    .Include(up => up.Policy)
                    .ThenInclude(pol => pol.Company)
                    .OrderByDescending(up => up.PurchaseDate)
                    .ToList();

                var activePolicies = myPolicies.Where(p => p.Status == "Active").ToList();
                var activePolicyCount = activePolicies.Count;
                ViewBag.activepolicies = activePolicyCount;

                var totalPremium = activePolicies.Sum(p => p.CalculatedPremium);
                ViewBag.totalPremium = totalPremium;

                var nextPaymentDue = myPolicies
                    .Where(p => p.ExpiryDate > DateTime.Now)
                    .OrderBy(p => p.ExpiryDate)
                    .Select(p => p.ExpiryDate)
                    .FirstOrDefault();

                ViewBag.nextPaymentDue = nextPaymentDue;
            }
            else
            {
                return RedirectToAction("Signin");
            }

            return View();
        }
        public IActionResult Profile()
        {
            var userId = HttpContext.Session.GetInt32("userId");

            if (userId == null)
            {
                return RedirectToAction("Signin");
            }

            var userDetails = _context.Tbl_Users.FirstOrDefault(u => u.Id == userId);

            if (userDetails == null)
            {
                return NotFound("User Profile was not found");
            }
            return View(userDetails);
        }
        public async Task<IActionResult> EditProfile()
        {
            var userId = HttpContext.Session.GetInt32("userId");

            if (userId == null)
            {
                return RedirectToAction("login");
            }
            var user = await _context.Tbl_Users.FindAsync(userId.Value);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(Users model)
        {
            var existingUser = await _context.Tbl_Users.FindAsync(model.Id);
            if (existingUser == null)
            {
                return NotFound();
            }

            if (model.ProfileImage != null)
            {
                existingUser.ProfileImagePath = UploadFile(model.ProfileImage);
            }
            else
            {

                existingUser.ProfileImagePath = model.ProfileImagePath ?? existingUser.ProfileImagePath ?? string.Empty;
            }


            ModelState.Remove("ProfileImagePath");

            existingUser.Name = model.Name;
            existingUser.Username = model.Username;
            existingUser.Email = model.Email;
            existingUser.MobileNumber = model.MobileNumber;
            existingUser.Address = model.Address;
            existingUser.DateOfBirth = model.DateOfBirth;

            try
            {
                await _context.SaveChangesAsync();
                var user = _context.Tbl_Users.FirstOrDefault(u => u.Id == model.Id);
                if (user != null)
                {

                    HttpContext.Session.SetString("userName", value: user.Name);
                    HttpContext.Session.SetString("userEmail", value: user.Email);
                    HttpContext.Session.SetString("userImg", value: user.ProfileImagePath);
                }
                TempData["SuccessMessage"] = "User updated successfully!";
                return RedirectToAction("Profile");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error updating user: " + ex.Message);
                return View(model);
            }
        }
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ChangePassword(string CurrentPassword, string Password, string ConfirmPassword)
        {
            var userId = HttpContext.Session.GetInt32("userId");

            if (userId == null)
            {
                return RedirectToAction("Login");
            }

            var user = _context.Tbl_Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            if (user.Password != CurrentPassword)
            {
                ModelState.AddModelError("CurrentPassword", "Current password is incorrect.");
            }

            if (Password != ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "New password and confirmation do not match.");
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            user.Password = Password;
            _context.SaveChanges();

            TempData["Success"] = "Password updated successfully!";
            return RedirectToAction("Profile");
        }
        public IActionResult MakePayment()
        {
            int? userId = HttpContext.Session.GetInt32("userId");
            if (userId == null)
            {
                TempData["ErrorMessage"] = "User not logged in.";
                return RedirectToAction("SignIn");
            }
            if (TempData["CheckoutModel"] is string serializedModel)
            {
                var model = JsonSerializer.Deserialize<PremiumCalculationViewModel>(serializedModel);
                TempData.Keep("CheckoutModel");
                return View(model);
            }
            return RedirectToAction("MyPolicies");
        }
        public IActionResult MyPolicies()
        {
            var userId = HttpContext.Session.GetInt32("userId");

            var myPolicies = _context.Tbl_UserPolicy
                .Where(up => up.UserId == userId)  // or the correct field name
                .Include(up => up.Policy)
                .ThenInclude(pol => pol.Company)
                .OrderByDescending(up => up.PurchaseDate)
                .ToList();

            var activePolicies = myPolicies.Where(p => p.Status == "Active").ToList();
            var activePolicyCount = activePolicies.Count;
            ViewBag.activepolicies = activePolicyCount;

            var totalPremium = activePolicies.Sum(p => p.CalculatedPremium);
            ViewBag.totalPremium = totalPremium;

            var nextPaymentDue = myPolicies
                .Where(p => p.ExpiryDate > DateTime.Now)
                .OrderBy(p => p.ExpiryDate)
                .Select(p => p.ExpiryDate)
                .FirstOrDefault();

            ViewBag.nextPaymentDue = nextPaymentDue;

            return View(myPolicies);
        }
        public IActionResult MyPolicyDetail(int id)
        {
            ViewBag.name = HttpContext.Session.GetString("userName");

            var policy = _context.Tbl_UserPolicy
                .Include(p => p.Policy)
                .ThenInclude(pol => pol.Company)
                .FirstOrDefault(p => p.Id == id);

            if (policy == null)
            {
                return NotFound();
            }

            return View(policy);
        }
        public IActionResult ApplyLoan()
        {
            var userId = HttpContext.Session.GetInt32("userId");

            var myPolicies = _context.Tbl_UserPolicy
                .Where(up => up.UserId == userId)  
                .Include(up => up.Policy)
                .ThenInclude(pol => pol.Company)
                .OrderByDescending(up => up.PurchaseDate)
                .ToList();

            return View(myPolicies);
        }
        public IActionResult FileClaim()
        {
            var model = new ClaimFormViewModel
            {
                Categories = _context.Tbl_Category.Where(c => c.Status).ToList(),
                Policies = new List<Policy>()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> FileClaim(ClaimFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = _context.Tbl_Category.Where(c => c.Status).ToList();
                return View(model);
            }

            var userId = HttpContext.Session.GetInt32("userId");
            if (userId == null)
            {
                return RedirectToAction("Signin", "User");
            }

            string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/claims");
            if (!Directory.Exists(uploadFolder))
                Directory.CreateDirectory(uploadFolder);

            List<string> filePaths = new List<string>();

            if (model.SupportingDocuments != null && model.SupportingDocuments.Any())
            {
                foreach (var file in model.SupportingDocuments)
                {
                    var uniqueFileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    var fullPath = Path.Combine(uploadFolder, uniqueFileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    filePaths.Add("/uploads/claims/" + uniqueFileName);
                }
            }

            var userClaim = new PolicyPro360.Models.UserClaim
            {
                UserId = userId.Value,
                PolicyCategoryId = model.SelectedCategoryId,
                PolicyId = model.SelectedPolicyId,
                DateOfIncident = model.DateOfIncident,
                IncidentDetails = model.IncidentDetails,
                ClaimedAmount = model.ClaimedAmount,
                UserRequest = model.UserRequest,
                SupportingDocumentPath = string.Join(",", filePaths),
                Status = "Pending"
            };

            _context.Tbl_UserClaim.Add(userClaim);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Your claim has been submitted successfully!";
            return RedirectToAction("MyApplication");
        }

        [HttpGet]
        public JsonResult GetPoliciesByCategory(int categoryId)
        {
            var userId = HttpContext.Session.GetInt32("userId");

            if (userId == null)
            {
                return Json(new { success = false, message = "User not logged in." });
            }

            var policies = _context.Tbl_UserPolicy
                .Where(up => up.UserId == userId.Value && up.Policy.PolicyTypeId == categoryId)
                .Select(up => new
                {
                    id = up.Policy.Id,
                    name = up.Policy.Name
                }).ToList();

            return Json(policies);
        }

        public IActionResult MyApplication()
        {
            var userId = HttpContext.Session.GetInt32("userId");
            var claimApplications = _context.Tbl_UserClaim.Where(c => c.UserId ==  userId)
                .Include(c=> c.Category)
                .Include(c=> c.Policy)
                .ToList();
            return View(claimApplications);
            
        }
        public IActionResult Support()
        {
            return View();
        }

        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("userName") != null)
            {
                HttpContext.Session.Remove("userName");
                HttpContext.Session.Remove("userId");
                HttpContext.Session.Remove("userEmail");
                HttpContext.Session.Remove("userImg");
                return RedirectToAction("signin");
            }

            return View();
        }

        private string UploadFile(IFormFile profileImage)
        {
            string? uniqueFileName = null;

            if (profileImage != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "admin/assets/images/profiles");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(profileImage.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    profileImage.CopyTo(fileStream);
                }
            }

            return uniqueFileName ?? string.Empty;
        }
    }
}