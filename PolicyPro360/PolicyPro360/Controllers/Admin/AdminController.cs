using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using PolicyPro360.Models;
using System.IO; 
using System;
using Microsoft.EntityFrameworkCore; 


namespace PolicyPro360.Controllers.Admin
{
    public class AdminController : BaseAdminController
    {

        private myContext _db;

        public AdminController(myContext db)
        {
            _db = db;

        }

        public IActionResult Dashboard()
        {

            if (HttpContext.Session.GetString("name") != null)
            {
                ViewBag.name = HttpContext.Session.GetString("name");
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

            ViewBag.AdminEarningsThisMonth = _db.Tbl_AdminWallet
                .Where(aw => aw.TransactionDate >= startDateOfMonth)
                .Sum(aw => (decimal?)aw.Amount) ?? 0;

            ViewBag.GrossRevenueThisMonth = _db.Tbl_TransactionHistory
                .Where(th => th.Date >= startDateOfMonth)
                .Sum(th => (decimal?)th.Amount) ?? 0;

            ViewBag.TransactionsThisMonth = _db.Tbl_TransactionHistory
                .Where(th => th.Date >= startDateOfMonth)
                .Count();

            ViewBag.TotalAdminEarnings = _db.Tbl_AdminWallet.Sum(aw => (decimal?)aw.Amount) ?? 0;
            ViewBag.TotalCompanyPayouts = _db.Tbl_CompanyWallet.Sum(cw => (decimal?)cw.Amount) ?? 0;

  
            ViewBag.RecentPolicies = _db.Tbl_UserPolicy
                .Include(up => up.User)
                .Include(up => up.Policy)
                .OrderByDescending(up => up.PurchaseDate)
                .Take(5)
                .ToList();


            ViewBag.TopCompanies = _db.Tbl_CompanyWallet
                .Include(cw => cw.Company)
                .GroupBy(cw => new { cw.CompanyId, cw.Company.CompanyName, cw.Company.CompanyLogoPath })
                .Select(g => new
                {
                    CompanyName = g.Key.CompanyName,
                    CompanyLogo = g.Key.CompanyLogoPath,
                    TotalEarnings = g.Sum(cw => cw.Amount)
                })
                .OrderByDescending(x => x.TotalEarnings)
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

            return View();
        }
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("name") != null)
            {
                return RedirectToAction("Dashboard");
            }
            return View();
        }
        [HttpPost]
        public IActionResult Login(string aemail, string apass)
        {
            var admin = _db.Tbl_Admin.FirstOrDefault(a => a.Email == aemail && a.Password == apass);
            if (admin != null)
            {
                HttpContext.Session.SetString("name", admin.Name);
                HttpContext.Session.SetString("email", admin.Email);
                HttpContext.Session.SetString("Img", admin.Img ?? "/admin/assets/images/profiles/default.png"); 
                TempData["success"] = "Login successful!";
                return RedirectToAction("Dashboard");
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid email or password.";

            }
            return View();

        }

        public IActionResult Register()
        {
            if (HttpContext.Session.GetString("name") == null)
                return RedirectToAction("Login");

            return View();
        }
        [HttpPost]

        [HttpPost]
        public IActionResult Register(PolicyPro360.Models.Admin admin, IFormFile ProfileImage)
        {
           
            if (HttpContext.Session.GetString("name") == null)
                return RedirectToAction("Login");

         
            if (ModelState.IsValid)
            {
            
                var existing = _db.Tbl_Admin.FirstOrDefault(x => x.Email == admin.Email);
                if (existing != null)
                {
                    ViewBag.ErrorMessage = "Email already exists.";
                    return View();
                }

          
                if (ProfileImage != null && ProfileImage.Length > 0)
                {
                   
                    var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/admin/assets/images/profiles");

       
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

        
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ProfileImage.FileName);
                    var filePath = Path.Combine(uploadPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        ProfileImage.CopyTo(stream);
                    }

                    admin.Img = "/admin/assets/images/profiles/" + fileName;
                }
                else
                {
          
                    admin.Img = null;
                }

                _db.Tbl_Admin.Add(admin);
                _db.SaveChanges();
                HttpContext.Session.SetString("Img", admin.Img ?? "/admin/assets/images/profiles/default.png");
                TempData["success"] = "Admin registered successfully!";
                return RedirectToAction("Dashboard");
            }

     
            return View();
        }
        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("name") != null)
            {
                HttpContext.Session.Remove("name");
                return RedirectToAction("Login");
            }
       
           return View();
        }

        public IActionResult Profile()
        {
            var adminemail = HttpContext.Session.GetString("email");

            if (string.IsNullOrEmpty(adminemail))
            {

                return RedirectToAction("Login");
            }

            var adminDetails = _db.Tbl_Admin.FirstOrDefault(a => a.Email == adminemail);

            if (adminDetails == null)
            {

                return NotFound("Admin profile not found.");
            }


            return View(adminDetails);
        }

        public IActionResult EditProfile()
        {
            var adminEmail = HttpContext.Session.GetString("email");
            if (string.IsNullOrEmpty(adminEmail))
            {
                return RedirectToAction("Login");
            }

     
            var admin = _db.Tbl_Admin.FirstOrDefault(a => a.Email == adminEmail);
            if (admin == null)
            {
                return NotFound();
            }

       
            return View(admin);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditProfile(PolicyPro360.Models.Admin admin, string currentPassword, string confirmPassword, IFormFile? ProfileImage)
        {
       
        
            if (string.IsNullOrEmpty(admin.Password))
            {
                ModelState.Remove("Password");
            }
            else 
            {
         
                if (admin.Password != confirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "The new password and confirmation password do not match.");
                }

             
                if (string.IsNullOrEmpty(currentPassword))
                {
                    ModelState.AddModelError("CurrentPassword", "Current password is required to set a new password.");
                }
            }


            if (ModelState.IsValid)
            {
                var adminToUpdate = _db.Tbl_Admin.Find(admin.Id);
                if (adminToUpdate == null)
                {
                    return NotFound();
                }

       
                if (!string.IsNullOrEmpty(admin.Password))
                {
               
                    if (adminToUpdate.Password != currentPassword)
                    {
                        ModelState.AddModelError("CurrentPassword", "The current password you entered is incorrect.");
                        return View(admin); 
                    }

               
                    adminToUpdate.Password = admin.Password; 
                }


                adminToUpdate.Name = admin.Name;
                adminToUpdate.Email = admin.Email;

            
                if (ProfileImage != null && ProfileImage.Length > 0)
                {
                    if (!string.IsNullOrEmpty(adminToUpdate.Img))
                    {
                        var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", adminToUpdate.Img.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/admin/assets/images/profiles");
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ProfileImage.FileName);
                    var filePath = Path.Combine(uploadPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        ProfileImage.CopyTo(stream);
                    }
                    adminToUpdate.Img = "/admin/assets/images/profiles/" + fileName;
                }

                _db.SaveChanges();
                TempData["success"] = "Profile updated successfully!";


                HttpContext.Session.SetString("name", adminToUpdate.Name);
                HttpContext.Session.SetString("email", adminToUpdate.Email);

                return RedirectToAction("Profile");
            }

    
            return View(admin);
        }
        public IActionResult AdminWallet()
        {
            var transactions = _db.Set<AdminWallet>()
                .Include(w => w.Policy)
                .Include(w => w.Company)
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
                    Status = "Credited"
                })
                .ToList();
            return View(transactions);
        }
    }
}
