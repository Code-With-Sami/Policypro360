using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PolicyPro360.Models;

namespace PolicyPro360.Controllers.Admin
{
    public class FAQController : BaseAdminController
    {
        private readonly myContext _context;

        public FAQController(myContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var faqs = _context.Tbl_FAQ.OrderByDescending(f => f.CreatedDate).ToList();
            return View("~/Views/AdminFAQ/Index.cshtml", faqs);
        }

        public IActionResult Create()
        {
            return View("~/Views/AdminFAQ/Create.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Question,Answer,IsActive")] FAQ faq)
        {
            if (ModelState.IsValid)
            {
                faq.CreatedDate = DateTime.Now;
                _context.Add(faq);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "FAQ created successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View("~/Views/AdminFAQ/Create.cshtml", faq);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faq = _context.Tbl_FAQ.Find(id);
            if (faq == null)
            {
                return NotFound();
            }
            return View("~/Views/AdminFAQ/Edit.cshtml", faq);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Question,Answer,IsActive,CreatedDate")] FAQ faq)
        {
            if (id != faq.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(faq);
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "FAQ updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FAQExists(faq.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View("~/Views/AdminFAQ/Edit.cshtml", faq);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var faq = _context.Tbl_FAQ.Find(id);
            if (faq != null)
            {
                _context.Tbl_FAQ.Remove(faq);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "FAQ deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool FAQExists(int id)
        {
            return _context.Tbl_FAQ.Any(e => e.Id == id);
        }
    }
} 