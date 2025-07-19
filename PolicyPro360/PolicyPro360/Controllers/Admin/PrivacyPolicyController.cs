using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PolicyPro360.Models;
using System.Linq;

namespace PolicyPro360.Controllers.Admin
{
    public class PrivacyPolicyController : BaseAdminController
    {
        private readonly myContext _context;

        public PrivacyPolicyController(myContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var policies = _context.Set<PrivacyPolicy>().ToList();
            return View("~/Views/AdminPrivacyPolicy/Index.cshtml", policies);
        }

        public IActionResult Create()
        {
            return View("~/Views/AdminPrivacyPolicy/Create.cshtml");
        }

        [HttpPost]
        public IActionResult Create(PrivacyPolicy policy)
        {
            if (ModelState.IsValid)
            {
                _context.Add(policy);
                _context.SaveChanges();
                TempData["Success"] = "Privacy Policy Created!";
                return RedirectToAction(nameof(Index));
            }
            return View(policy);
        }

        public IActionResult Details(int id)
        {
            var privacypolicy = _context.Tbl_PrivacyPolicy.FirstOrDefault(b => b.Id == id);
            if (privacypolicy == null)
            {
                return NotFound();
            }
            return View("~/Views/AdminPrivacyPolicy/Details.cshtml", privacypolicy);
        }

        public IActionResult Edit(int id)
        {
            var policy = _context.Set<PrivacyPolicy>().Find(id);
            if (policy == null) return NotFound();
            return View("~/Views/AdminPrivacyPolicy/Edit.cshtml", policy);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(PrivacyPolicy model)
        {
            var privacypolicy = _context.Set<PrivacyPolicy>().Find(model.Id);
            if (privacypolicy == null) return NotFound();

            if (ModelState.IsValid)
            {
                privacypolicy.Title = model.Title;
                privacypolicy.Content = model.Content;
                privacypolicy.IsActive = model.IsActive;
                privacypolicy.CreatedAt = model.CreatedAt;


                _context.Update(privacypolicy);
                _context.SaveChanges();
                TempData["Success"] = "PrivacyPolicy Updated Successfully!";
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var privacypolicy = _context.Tbl_PrivacyPolicy.Find(id);
            if (privacypolicy != null)
            {
                _context.Tbl_PrivacyPolicy.Remove(privacypolicy);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "PrivacyPolicy deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
