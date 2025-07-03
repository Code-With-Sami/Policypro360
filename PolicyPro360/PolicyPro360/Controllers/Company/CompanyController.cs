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

                _db.Tbl_Company.Add(company);
                await _db.SaveChangesAsync();

                TempData["success"] = "Company registered successfully! Please login to continue.";
                return RedirectToAction("Login");
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
                HttpContext.Session.SetString("companyname", company.CompanyName);
                HttpContext.Session.SetString("companyEmail", company.Email);
                HttpContext.Session.SetInt32("companyId", company.Id); 

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


    }

}
