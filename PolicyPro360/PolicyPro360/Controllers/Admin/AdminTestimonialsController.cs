using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PolicyPro360.Models;

namespace PolicyPro360.Controllers.Admin
{
    public class AdminTestimonialsController : BaseAdminController
    {
        private readonly myContext _context;
        public AdminTestimonialsController(myContext context) => _context = context;

        public IActionResult Index()
        {
            var testimonials = _context.Tbl_Testimonial.Include(t => t.User).ToList();
            return View(testimonials);
        }

        [HttpPost, ActionName("PublishTestimonial")]
        [ValidateAntiForgeryToken]
        public IActionResult Publish(int id)
        {
            var testimonial = _context.Tbl_Testimonial.Find(id);
            if (testimonial != null) 
            {
                testimonial.Status = "Published";
                _context.SaveChanges();
                TempData["Success"] = "Testimonial published!";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var testimonial = _context.Tbl_Testimonial.Find(id);
            if (testimonial != null)
            {
                _context.Tbl_Testimonial.Remove(testimonial);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Testimonial deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
