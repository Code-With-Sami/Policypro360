using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PolicyPro360.Models;

namespace PolicyPro360.Controllers.Company
{
    public class CompanyController : Controller
    {
        private myContext _db;

        public CompanyController(myContext db)
        {
            _db = db;

        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(PolicyPro360.Models.Company company, IFormFile CompanyLogoFile, IFormFile OwnerImageFile)
        {
            if (ModelState.IsValid)
            {
                // Save images
                if (CompanyLogoFile != null)
                {
                    var logoPath = Path.Combine("wwwroot/uploads", CompanyLogoFile.FileName);
                    using (var stream = new FileStream(logoPath, FileMode.Create))
                    {
                        await CompanyLogoFile.CopyToAsync(stream);
                    }
                    company.CompanyLogoPath = "/uploads/" + CompanyLogoFile.FileName;
                }

                if (OwnerImageFile != null)
                {
                    var ownerImagePath = Path.Combine("wwwroot/uploads", OwnerImageFile.FileName);
                    using (var stream = new FileStream(ownerImagePath, FileMode.Create))
                    {
                        await OwnerImageFile.CopyToAsync(stream);
                    }
                    company.OwnerImagePath = "/uploads/" + OwnerImageFile.FileName;
                }

                _db.Tbl_Company.Add(company);
                await _db.SaveChangesAsync();

                TempData["Success"] = "Company registration submitted!";
                return RedirectToAction("Register");
            }

            return View(company);
        }
    }
}
