using Microsoft.AspNetCore.Mvc;

namespace PolicyPro360.Controllers.User
{
    public class UserHomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
