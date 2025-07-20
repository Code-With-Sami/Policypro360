using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PolicyPro360.Attributes;
using PolicyPro360.Models;
using System.IO;

namespace PolicyPro360.Controllers.Company
{

    public class CompanyController : BaseCompanyController
    {
        private readonly myContext _db; 
        private readonly IWebHostEnvironment _hostingEnvironment;


        public CompanyController(myContext db, IWebHostEnvironment hostingEnvironment) : base(db)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;
        }

    
        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("companyname") != null)
            {
                ViewBag.CompanyName = HttpContext.Session.GetString("companyname");
                var companyId = HttpContext.Session.GetInt32("companyId");
                
      
                var activeCategories = _db.Tbl_Category
                    .Where(c => c.Status == true)
                    .ToList();
                
               
                var policiesCountDict = new Dictionary<int, int>();
                foreach (var category in activeCategories)
                {
                    var count = _db.Tbl_Policy.Count(p => p.PolicyTypeId == category.Id && p.CompanyId == companyId);
                    policiesCountDict[category.Id] = count;
                }
                
                ViewBag.Categories = activeCategories;
                ViewBag.PoliciesCountDict = policiesCountDict;
                Console.WriteLine("Category count: " + activeCategories.Count);
            }
            else
            {
                return RedirectToAction("Login");
            }

            ViewBag.TotalUsers = _db.Tbl_Users.Count();
            ViewBag.PolicyHolders = _db.Tbl_UserPolicy
                .Select(up => up.UserId)
                .Distinct()
                .Count();
            ViewBag.TotalPoliciesSold = _db.Tbl_UserPolicy.Count();
            ViewBag.TotalCompanies = _db.Tbl_Company.Count();
            ViewBag.TotalCompanies = _db.Tbl_Company.Count(c => c.Status == "Approved");
            ViewBag.TotalPolicyTypes = _db.Tbl_Category.Count(c => c.Status);

            ViewBag.GrossRevenue = _db.Tbl_TransactionHistory.Sum(th => (decimal?)th.Amount) ?? 0;


            var today = DateTime.Today;
            var startDateOfMonth = new DateTime(today.Year, today.Month, 1);

            ViewBag.ComapnyEarningsThisMonth = _db.Tbl_CompanyWallet
                .Where(aw => aw.TransactionDate >= startDateOfMonth)
                .Sum(aw => (decimal?)aw.Amount) ?? 0;

            ViewBag.GrossRevenueThisMonth = _db.Tbl_TransactionHistory
                .Where(th => th.Date >= startDateOfMonth)
                .Sum(th => (decimal?)th.Amount) ?? 0;

            ViewBag.RecentPolicies = _db.Tbl_UserPolicy
                .Include(up => up.User)
                .Include(up => up.Policy)
                .OrderByDescending(up => up.PurchaseDate)
                .Take(5)
                .ToList();


            ViewBag.NewUsers = _db.Tbl_Users
                   .OrderByDescending(u => u.Id)
                   .Take(7)
                   .ToList();

            ViewBag.RecentSales = _db.Tbl_UserPolicy
                .Include(up => up.User)
                .Include(up => up.Policy)
                    .ThenInclude(p => p.Company)
                .OrderByDescending(up => up.PurchaseDate)
                .Take(6)
                .ToList();
            ViewBag.TotalUserLoans = _db.Tbl_LoanRequests.Count();
            ViewBag.TotalUserClaims = _db.Tbl_UserClaims.Count();


            var categories = _db.Tbl_Category.Where(c => c.Status).ToList();

            var categoryStats = categories.Select(cat => new {
                CategoryName = cat.Name,
                ClaimsCount = _db.Tbl_UserClaims.Count(c => c.PolicyCategoryId == cat.Id),
                LoansCount = _db.Tbl_LoanRequests.Count(l => l.Policy.PolicyTypeId == cat.Id)
            }).ToList();

            ViewBag.CategoryStats = categoryStats;

            var categoryPolicyStats = categories.Select(cat => new {
                CategoryName = cat.Name,
                PoliciesCount = _db.Tbl_Policy.Count(p => p.PolicyTypeId == cat.Id)
            }).ToList();

            ViewBag.CategoryPolicyStats = categoryPolicyStats;

            return View();
        }


        [AllowAnonymousCompany]
        public IActionResult Register()
        {
            return View();
        }

      

        [HttpPost]
        [AllowAnonymousCompany]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Register(PolicyPro360.Models.Company company, IFormFile CompanyLogoFile, IFormFile OwnerImageFile)
        {
          
            if (CompanyLogoFile == null || CompanyLogoFile.Length == 0)
            {
                ModelState.AddModelError("CompanyLogoPath", "Company logo is required.");
            }
            if (OwnerImageFile == null || OwnerImageFile.Length == 0)
            {
                ModelState.AddModelError("OwnerImagePath", "Owner image is required.");
            }

            if (ModelState.IsValid)
            {
                if (CompanyLogoFile != null)
                {
              
                    string uniqueLogoName = Guid.NewGuid().ToString() + "_" + CompanyLogoFile.FileName;
                    string logoUploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "companies/companyimages");
                    string logoFilePath = Path.Combine(logoUploadsFolder, uniqueLogoName);
                    using (var fileStream = new FileStream(logoFilePath, FileMode.Create))
                    {
                        await CompanyLogoFile.CopyToAsync(fileStream);
                    }

     
                    company.CompanyLogoPath = "/companies/companyimages/" + uniqueLogoName;
                }

                if (OwnerImageFile != null)
                {
 
                    string uniqueOwnerImageName = Guid.NewGuid().ToString() + "_" + OwnerImageFile.FileName;
                    string ownerImageUploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "companies/ownerimages");
                    string ownerImageFilePath = Path.Combine(ownerImageUploadsFolder, uniqueOwnerImageName);
                    using (var fileStream = new FileStream(ownerImageFilePath, FileMode.Create))
                    {
                        await OwnerImageFile.CopyToAsync(fileStream);
                    }


                    company.OwnerImagePath = "/companies/ownerimages/" + uniqueOwnerImageName;
                }

                company.Status = "Pending"; // Ensure status is set to Pending
                _db.Tbl_Company.Add(company);
                await _db.SaveChangesAsync();

                ViewBag.SuccessMessage = "Company registered successfully! Please wait for admin approval before logging in.";
                return View("Login");
            }

            return View(company);
        }
        [AllowAnonymousCompany]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("companyname") != null)
            {
                return RedirectToAction("Dashboard");
            }
            return View();
        }

        [AllowAnonymousCompany]
        [HttpPost]
        public IActionResult Login(string companyEmail, string companyPassword)
        {
            var company = _db.Tbl_Company.FirstOrDefault(c => c.Email == companyEmail && c.Password == companyPassword);
            if (company != null)
            {
                if (company.Status == "Approved")
                {
                    HttpContext.Session.SetString("companyname", company.CompanyName);
                    HttpContext.Session.SetString("companyEmail", company.Email);
                    HttpContext.Session.SetString("companyImg", company.CompanyLogoPath);
                    HttpContext.Session.SetInt32("companyId", company.Id);

                    TempData["success"] = "Login successful!";
                    return RedirectToAction("Dashboard");
                }
                else if (company.Status == "Pending")
                {
                    ViewBag.ErrorMessage = "Your account is pending admin approval. Please wait for approval before logging in.";
                    return View();
                }
                else
                {
                    ViewBag.ErrorMessage = "Your account has not been approved or has been rejected.";
                    return View();
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid email or password.";
            }
            return View();

           
        }

        

        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("companyname") != null)
            {
                HttpContext.Session.Remove("companyname");
                HttpContext.Session.Remove("companyEmail");
                HttpContext.Session.Remove("companyId");
                return RedirectToAction("Login");
            }
            return View();
        }

        public IActionResult Profile()
        {
            var companyEmail = HttpContext.Session.GetString("companyEmail"); 

            if (string.IsNullOrEmpty(companyEmail))
            {
        
                return RedirectToAction("Login");
            }

            var companyDetails = _db.Tbl_Company.FirstOrDefault(c => c.Email == companyEmail);

            if (companyDetails == null)
            {

                return NotFound("Company profile not found.");
            }

  
            return View(companyDetails);
        }

  
        public async Task<IActionResult> EditProfile()
        {
           
            var companyId = HttpContext.Session.GetInt32("companyId");
            if (companyId == null)
            {
                return RedirectToAction("Login");
            }

            var company = await _db.Tbl_Company.FindAsync(companyId.Value);
            if (company == null)
            {
                return NotFound();
            }

        
            return View(company);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(PolicyPro360.Models.Company company, IFormFile? newCompanyLogo, IFormFile? newOwnerImage)
        {

            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");

 
            if (ModelState.IsValid)
            {
      
                var companyToUpdate = await _db.Tbl_Company.FindAsync(company.Id);
                if (companyToUpdate == null)
                {
                    return NotFound();
                }

                if (newCompanyLogo != null && newCompanyLogo.Length > 0)
                {
                    if (!string.IsNullOrEmpty(companyToUpdate.CompanyLogoPath))
                    {
                        var oldLogoPath = Path.Combine(_hostingEnvironment.WebRootPath, companyToUpdate.CompanyLogoPath.TrimStart('/'));
                        if (System.IO.File.Exists(oldLogoPath))
                        {
                            System.IO.File.Delete(oldLogoPath);
                        }
                    }
                    string uniqueLogoName = Guid.NewGuid().ToString() + "_" + newCompanyLogo.FileName;
                    string logoUploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "companies/companyimages");
                    string logoFilePath = Path.Combine(logoUploadsFolder, uniqueLogoName);
                    using (var fileStream = new FileStream(logoFilePath, FileMode.Create))
                    {
                        await newCompanyLogo.CopyToAsync(fileStream);
                    }
                    companyToUpdate.CompanyLogoPath = "/companies/companyimages/" + uniqueLogoName;
                }

                if (newOwnerImage != null && newOwnerImage.Length > 0)
                {
                    if (!string.IsNullOrEmpty(companyToUpdate.OwnerImagePath))
                    {
                        var oldImagePath = Path.Combine(_hostingEnvironment.WebRootPath, companyToUpdate.OwnerImagePath.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    string uniqueOwnerImageName = Guid.NewGuid().ToString() + "_" + newOwnerImage.FileName;
                    string ownerImageUploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "companies/ownerimages");
                    string ownerImageFilePath = Path.Combine(ownerImageUploadsFolder, uniqueOwnerImageName);
                    using (var fileStream = new FileStream(ownerImageFilePath, FileMode.Create))
                    {
                        await newOwnerImage.CopyToAsync(fileStream);
                    }
                    companyToUpdate.OwnerImagePath = "/companies/ownerimages/" + uniqueOwnerImageName;
                }

                companyToUpdate.CompanyName = company.CompanyName;
                companyToUpdate.BusinessType = company.BusinessType;
                companyToUpdate.IndustryType = company.IndustryType;
                companyToUpdate.Description = company.Description;
                companyToUpdate.Country = company.Country;
                companyToUpdate.City = company.City;
                companyToUpdate.Address = company.Address;
                companyToUpdate.Email = company.Email;
                companyToUpdate.PhoneNo = company.PhoneNo;
                companyToUpdate.OwnerName = company.OwnerName;
                companyToUpdate.OwnerDOB = company.OwnerDOB;
                companyToUpdate.OwnerNationality = company.OwnerNationality;
                companyToUpdate.OwnerEmail = company.OwnerEmail;
                companyToUpdate.OwnerPhoneNo = company.OwnerPhoneNo;
                companyToUpdate.OwnerRole = company.OwnerRole;
                companyToUpdate.OwnerCNIC = company.OwnerCNIC;
                companyToUpdate.RegistrationNo = company.RegistrationNo;

                try
                {
                    _db.Tbl_Company.Update(companyToUpdate);
                    await _db.SaveChangesAsync();
                    HttpContext.Session.SetString("companyname", company.CompanyName);
                    HttpContext.Session.SetString("companyEmail", company.Email);
                    HttpContext.Session.SetString("companyImg", company.CompanyLogoPath);
                    TempData["success"] = "Profile updated successfully!";
                    return RedirectToAction(nameof(Profile));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Please try again.");
                    return View(company);
                }
            }

            
            return View(company);
        }

        public IActionResult CompanyWallet()
        {
            int? companyId = HttpContext.Session.GetInt32("companyId");
            if (companyId == null)
            {
                return RedirectToAction("Login");
            }

          
            var transactions = _db.Set<CompanyWallet>()
                .Include(w => w.Policy)
                .Include(w => w.Company)
                .Where(w => w.CompanyId == companyId.Value)
                .OrderByDescending(w => w.TransactionDate)
                .Select(w => new
                {
                    Wallet = w,
                    UserPolicy = _db.Tbl_UserPolicy
                        .Where(up => up.PolicyId == w.PolicyId && up.UserId == w.UserId)
                        .OrderByDescending(up => up.PurchaseDate)
                        .FirstOrDefault()
                })
                .AsEnumerable()
                .Select(x => new PolicyPro360.ViewModels.WalletTransactionViewModel
                {
                    TransactionId = x.Wallet.Id,
                    PolicyName = x.Wallet.Policy != null ? x.Wallet.Policy.Name : "-",
                    CompanyName = x.Wallet.Company != null ? x.Wallet.Company.CompanyName : "-",
                    CompanyLogoUrl = x.Wallet.Company != null ? x.Wallet.Company.CompanyLogoPath : string.Empty,
                    TotalPremium = x.UserPolicy != null ? x.UserPolicy.CalculatedPremium : 0,
                    CommissionEarned = x.Wallet.Amount,
                    Date = x.Wallet.TransactionDate,
                    Status = x.Wallet.Amount >= 0 ? "Credited" : "Withdrawn"
                })
                .ToList();

          
            var actualBalance = transactions.Sum(x => x.CommissionEarned);
            ViewBag.ActualBalance = actualBalance;

            return View(transactions);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> WithdrawFunds(decimal withdrawAmount, string withdrawalReason, string bankDetails)
        {
            try
            {
                int? companyId = HttpContext.Session.GetInt32("companyId");
                if (companyId == null)
                {
                    return RedirectToAction("Login");
                }

                if (withdrawAmount <= 0)
                {
                    TempData["ErrorMessage"] = "Withdrawal amount must be greater than zero.";
                    return RedirectToAction("CompanyWallet");
                }

             
                var company = await _db.Tbl_Company.FindAsync(companyId.Value);
                if (company == null)
                {
                    TempData["ErrorMessage"] = "Company not found.";
                    return RedirectToAction("CompanyWallet");
                }

             
                var allTransactions = _db.Set<CompanyWallet>()
                    .Where(w => w.CompanyId == companyId.Value)
                    .ToList();
                var currentBalance = allTransactions.Sum(x => x.Amount);

                if (withdrawAmount > currentBalance)
                {
                    TempData["ErrorMessage"] = "Insufficient balance. Available balance: PKR " + currentBalance.ToString("N2");
                    return RedirectToAction("CompanyWallet");
                }

                var validUser = _db.Tbl_Users.FirstOrDefault();
                var validPolicy = _db.Tbl_Policy.FirstOrDefault();

                if (validUser == null || validPolicy == null)
                {
                    TempData["ErrorMessage"] = "System configuration error. Please contact administrator.";
                    return RedirectToAction("CompanyWallet");
                }

                var withdrawalTransaction = new CompanyWallet
                {
                    CompanyId = companyId.Value,
                    UserId = validUser.Id,
                    PolicyId = validPolicy.Id,
                    Amount = -withdrawAmount,
                    TransactionDate = DateTime.Now,
                    Description = string.IsNullOrEmpty(withdrawalReason) ? "Fund withdrawal" : withdrawalReason
                };

                _db.Set<CompanyWallet>().Add(withdrawalTransaction);
                await _db.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Withdrawal request submitted successfully! Amount: PKR {withdrawAmount:N2}. Funds will be transferred to your bank account within 2-3 business days.";
                return RedirectToAction("CompanyWallet");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while processing your withdrawal request. Please try again.";
                return RedirectToAction("CompanyWallet");
            }
        }


    }

}
