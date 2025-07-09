using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PolicyPro360.Models;

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

            //model.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);

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
