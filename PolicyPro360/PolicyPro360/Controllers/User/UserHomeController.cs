using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PolicyPro360.Models;
using PolicyPro360.ViewModels;
using System.Text.Json;

namespace PolicyPro360.Controllers.User
{
    public class UserHomeController : Controller
    {

        private readonly myContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserHomeController (myContext context, IWebHostEnvironment webHostEnvironment)
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

            // Save payment
            var payment = new UserPayment
            {
                PayerName = PayerName,
                PayerEmail = PayerEmail,
                Amount = Amount,
                Reference = Reference
            };
            _context.Tbl_UserPayment.Add(payment);

            // Save user policy
            DateTime purchaseDate = DateTime.Now;
            DateTime expiryDate = purchaseDate.AddYears(1);
            var userPolicy = new UserPolicy
            {
                PolicyId = PolicyId,
                UserId = userId.Value,
                CalculatedPremium = CalculatedPremium,
                CoverageAmount = CoverageAmount ?? 0,
                PurchaseDate = purchaseDate,
                ExpiryDate = expiryDate,
                Status = "Active"
            };
            _context.Tbl_UserPolicy.Add(userPolicy);

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
            var user = _context.Tbl_Users.FirstOrDefault(u => u.Email == u.Email && u.Password == userPassword);
            if (user != null)
            {

                HttpContext.Session.SetString("userName", value: user.Name);
                HttpContext.Session.SetInt32("userId", value: user.Id);
                HttpContext.Session.SetString("userEmail", value: user.Email);
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
            }
            else
            {
                return RedirectToAction("Login");
            }

            return View();
        }
        public IActionResult Profile()
        {
            return View();
        }
        public IActionResult EditProfile()
        {
            return View();
        }
        public IActionResult ChangePassword()
        {
            return View();
        }
        public IActionResult MakePayment()
        {
            return View();
        }
        public IActionResult MyPolicies()
        {
            return View();
        }
        public IActionResult MyPolicyDetail()
        {
            return View();
        }
        public IActionResult ApplyLoan()
        {
            return View();
        }
        public IActionResult FileClaim()
        {
            return View();
        }
        public IActionResult MyApplication()
        {
            return View();
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
