using Microsoft.AspNetCore.Mvc;
using PolicyPro360.Models;
using System.IO; 
using System;


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
            if (HttpContext.Session.GetString("name") != null){
                ViewBag.name = HttpContext.Session.GetString("name");
            }
            else
            {
                return RedirectToAction("Login");
            }
      
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
            return View();
        }
    }
}
