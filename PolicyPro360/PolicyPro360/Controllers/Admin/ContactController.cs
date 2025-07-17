using Microsoft.AspNetCore.Mvc;
using PolicyPro360.Models;
using System.Linq;

namespace PolicyPro360.Controllers.Admin
{
    public class ContactController : BaseAdminController
    {
        private readonly myContext _context;
        public ContactController(myContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            var contacts = _context.Tbl_Contact.OrderByDescending(c => c.CreatedDate).ToList();
            return View("~/Views/AdminContact/Index.cshtml", contacts);
        }


        public IActionResult Details(int id)
        {
            var contact = _context.Tbl_Contact.FirstOrDefault(c => c.Id == id);
            if (contact == null)
            {
                return NotFound();
            }
            return View("~/Views/AdminContact/Details.cshtml", contact);
        }


        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Contact contact)
        {
            if (ModelState.IsValid)
            {
                contact.CreatedDate = System.DateTime.Now;
                _context.Tbl_Contact.Add(contact);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Contact created successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(contact);
        }


        public IActionResult Edit(int id)
        {
            var contact = _context.Tbl_Contact.FirstOrDefault(c => c.Id == id);
            if (contact == null)
            {
                return NotFound();
            }
            return View("~/Views/AdminContact/Edit.cshtml", contact);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Contact contact)
        {
            if (ModelState.IsValid)
            {
                _context.Tbl_Contact.Update(contact);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Contact updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View("~/Views/AdminContact/Edit.cshtml", contact);
        }

        public IActionResult Delete(int id)
        {
            var contact = _context.Tbl_Contact.FirstOrDefault(c => c.Id == id);
            if (contact == null)
            {
                return NotFound();
            }
            return View("~/Views/AdminContact/Delete.cshtml", contact);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var contact = _context.Tbl_Contact.FirstOrDefault(c => c.Id == id);
            if (contact != null)
            {
                _context.Tbl_Contact.Remove(contact);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Contact deleted successfully.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
} 