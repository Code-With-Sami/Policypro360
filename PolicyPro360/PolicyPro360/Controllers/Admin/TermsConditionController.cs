using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PolicyPro360.Models;
using System.Linq;

namespace PolicyPro360.Controllers.Admin
{
    public class TermsConditionController : BaseAdminController
    {
        private readonly myContext _context;

        public TermsConditionController(myContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var terms = _context.Set<TermsCondition>().ToList();
            return View("~/Views/AdminTermsConditions/Index.cshtml", terms);
        }

        public IActionResult Create()
        {
            return View("~/Views/AdminTermsConditions/Create.cshtml");
        }

        [HttpPost]
        public IActionResult Create(TermsCondition terms)
        {
            if (ModelState.IsValid)
            {
                _context.Add(terms);
                _context.SaveChanges();
                TempData["Success"] = "Terms & Conditions Created!";
                return RedirectToAction(nameof(Index));
            }
            return View(terms);
        }

        public IActionResult Details(int id)
        {
            var termsconditions = _context.Tbl_TermsCondition.FirstOrDefault(b => b.Id == id);
            if (termsconditions == null)
            {
                return NotFound();
            }
            return View("~/Views/AdminTermsConditions/Details.cshtml", termsconditions);
        }

        public IActionResult Edit(int id)
        {
            var terms = _context.Set<TermsCondition>().Find(id);
            if (terms == null) return NotFound();
            return View("~/Views/AdminTermsConditions/Edit.cshtml", terms);
        }

        [HttpPost]
        public IActionResult Edit(TermsCondition terms)
        {
            if (ModelState.IsValid)
            {
                _context.Update(terms);
                _context.SaveChanges();
                TempData["Success"] = "Terms & Conditions Updated!";
                return RedirectToAction(nameof(Index));
            }
            return View(terms);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var termsconditions = _context.Tbl_TermsCondition.Find(id);
            if (termsconditions != null)
            {
                _context.Tbl_TermsCondition.Remove(termsconditions);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Terms And Conditions deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
