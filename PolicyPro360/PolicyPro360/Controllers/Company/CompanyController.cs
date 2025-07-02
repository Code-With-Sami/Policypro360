using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PolicyPro360.Attributes;
using PolicyPro360.Models;

namespace PolicyPro360.Controllers.Company
{

    public class CompanyController : BaseCompanyController
    {
        private myContext _db;

        public CompanyController(myContext db)
        {
            _db = db;

        }

        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("companyname") != null)
            {
                ViewBag.CompanyName = HttpContext.Session.GetString("companyname");
                var activeCategories = _db.Tbl_Category.Where(c => c.Status == true).ToList();
                ViewBag.Categories = activeCategories;
                Console.WriteLine("Category count: " + activeCategories.Count);
            }
            else
            {
                return RedirectToAction("Login");
            }
            return View();
        }
        [AllowAnonymousCompany]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymousCompany]
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
                // Save logo
                if (CompanyLogoFile != null)
                {
                    var logoPath = Path.Combine("wwwroot/companies/companyimages", CompanyLogoFile.FileName);
                    using var stream = new FileStream(logoPath, FileMode.Create);
                    await CompanyLogoFile.CopyToAsync(stream);
                    company.CompanyLogoPath = "/companies/companyimages/" + CompanyLogoFile.FileName;
                }

                // Save owner image
                if (OwnerImageFile != null)
                {
                    var ownerImgPath = Path.Combine("wwwroot/companies/ownerimages", OwnerImageFile.FileName);
                    using var stream = new FileStream(ownerImgPath, FileMode.Create);
                    await OwnerImageFile.CopyToAsync(stream);
                    company.OwnerImagePath = "/companies/ownerimages" + OwnerImageFile.FileName;
                }

                _db.Tbl_Company.Add(company);
                await _db.SaveChangesAsync();

                TempData["success"] = "Company registered successfully!";
                return RedirectToAction("Dashboard");
            }


            return RedirectToAction("Dashboard");
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
                HttpContext.Session.SetString("companyname", value: company.CompanyName);
                TempData["success"] = "Login successful!";
                return RedirectToAction("Dashboard");
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
                return RedirectToAction("Login");
            }
            return View();
        }

    }

}
