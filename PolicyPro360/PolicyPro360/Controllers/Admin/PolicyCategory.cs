using Microsoft.AspNetCore.Mvc;
using PolicyPro360.Models;

namespace PolicyPro360.Controllers.Admin
{
    public class PolicyCategory : BaseAdminController
    {
        private readonly myContext _context;
        public PolicyCategory(myContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var categories = _context.Tbl_Category.ToList();
            return View(categories);
        }

        public IActionResult Create()
        {
            var model = new Category
            {
                Status = true 
            };
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (!ModelState.IsValid)
                return View(category);
            category.Status = category.Status; 

            _context.Tbl_Category.Add(category);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Policy Category created successfully!";
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var category = _context.Tbl_Category.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Tbl_Category.Update(category);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Policy Category updated successfully!";
                return RedirectToAction("Index");
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var category = _context.Tbl_Category.Find(id);
            if (category != null)
            {
                _context.Tbl_Category.Remove(category);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Policy Category deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Category not found.";
            }

            return RedirectToAction("Index");
        }
    }
}
