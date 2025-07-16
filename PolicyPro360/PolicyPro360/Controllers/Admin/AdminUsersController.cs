using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using PolicyPro360.Models;
using System;
using System.IO;
using System.Threading.Tasks;


namespace PolicyPro360.Controllers.Admin
{
    public class AdminUsersController : BaseAdminController
    {
        private readonly myContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public int Id { get; private set; }

        public AdminUsersController(myContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

    
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var allUsers = await _context.Tbl_Users.ToListAsync();
            return View(allUsers);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Users());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Users model)
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

            model.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);

            try
            {
                _context.Tbl_Users.Add(model);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"User '{model.Username}' has been created successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                var innerMessage = ex.InnerException?.Message ?? ex.Message;
                ModelState.AddModelError("", "Database Error: " + innerMessage);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _context.Tbl_Users.FindAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Users model)
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
                TempData["SuccessMessage"] = "User updated successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error updating user: " + ex.Message);
                return View(model);
            }
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Tbl_Users.FindAsync(id);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return RedirectToAction("Index");
            }

            _context.Tbl_Users.Remove(user);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "User deleted successfully!";
            return RedirectToAction("Index");
        }


        public IActionResult AllUserPolicies()
        {
            var userPolicies = _context.Tbl_UserPolicy
                .Include(up => up.Policy)
                .ThenInclude(p => p.Company)
                .Include(up => up.Policy)
                .ThenInclude(p => p.Category)
                .Include(up => up.User)
                .ToList();
            return View(userPolicies);
        }
        public IActionResult ViewUserPolicy(int id)
        {
            var policy = _context.Tbl_UserPolicy
                .Include(up => up.User)
                .Include(up => up.Policy)
                .ThenInclude(p => p.Company)
                .FirstOrDefault(up => up.Id == id);

            if (policy == null)
                return NotFound();

            return View(policy);
        }


        //public IActionResult LifeUserPolicy()
        //{
        //    return View();
        //}

        //public IActionResult MotorUserPolicy()
        //{

        //    return View();
        //}

        //public IActionResult HomeUserPolicy()
        //{

        //    return View();
        //}

        //public IActionResult MedicalUserPolicy()
        //{

        //    return View();
        //}
        public IActionResult AllUserPayments()
        {
            var userPayments = _context.Tbl_UserPayment.ToList();
            return View(userPayments);
        }
        public IActionResult ViewPaymentDetail(int id)
        {
            var paymentDetail = _context.Tbl_UserPayment.FirstOrDefault(p => p.Id == id);

            if (paymentDetail == null)
            {
                return NotFound();
            }

            return View(paymentDetail);
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            ViewBag.PolicyCategories = _context.Tbl_Category.Where(c => c.Status).ToList();
        }

        public IActionResult UserPoliciesByCategory(int categoryId)
        {
            var userPolicies = _context.Tbl_UserPolicy
                .Include(up => up.Policy)
                .ThenInclude(p => p.Company)
                .Include(up => up.Policy)
                .ThenInclude(p => p.Category)
                .Include(up => up.User)
                .Where(up => up.Policy != null && up.Policy.PolicyTypeId == categoryId)
                .ToList();
            return View("AllUserPolicies", userPolicies);
        }
    }
}