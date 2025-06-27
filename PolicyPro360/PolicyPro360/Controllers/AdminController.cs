using Microsoft.AspNetCore.Mvc;
using PolicyPro360.Models;

namespace PolicyPro360.Controllers
{
    public class AdminController : Controller
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

                HttpContext.Session.SetString("name", value: admin.Name);
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
            if (HttpContext.Session.GetString("name") != null)
            {
                HttpContext.Session.Remove("name");
                return RedirectToAction("Login");
            }
       
                return View();
        }
    }
}
