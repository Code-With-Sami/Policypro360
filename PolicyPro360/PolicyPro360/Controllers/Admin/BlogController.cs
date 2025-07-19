using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PolicyPro360.Models;
using System.Linq;


namespace PolicyPro360.Controllers.Admin
{
    public class BlogController : BaseAdminController
    {
        private readonly myContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BlogController(myContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var blogs = _context.Set<Blog>().OrderByDescending(b => b.CreatedAt).ToList();
            return View("~/Views/AdminBlog/Index.cshtml", blogs);
        }

        public IActionResult Create()
        {
            return View("~/Views/AdminBlog/Create.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Blog model, IFormFile FeaturedImage)
        {
            if (ModelState.IsValid)
            {
                if (FeaturedImage != null)
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/blogs");
                    Directory.CreateDirectory(uploadsFolder);
                    var fileName = Guid.NewGuid() + Path.GetExtension(FeaturedImage.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        FeaturedImage.CopyTo(stream);
                    }
                    model.FeaturedImageUrl = "/uploads/blogs/" + fileName;
                }

                model.CreatedAt = DateTime.Now;

                _context.Add(model);
                _context.SaveChanges();
                TempData["Success"] = "Blog Created Successfully!";
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public IActionResult Details(int id)
        {
            var blog = _context.Tbl_Blog.FirstOrDefault(b => b.Id == id);
            if (blog == null)
            {
                return NotFound();
            }
            return View("~/Views/AdminBlog/Details.cshtml", blog);
        }

        public IActionResult Edit(int id)
        {
            var blog = _context.Set<Blog>().Find(id);
            if (blog == null) return NotFound();
            return View("~/Views/AdminBlog/Edit.cshtml", blog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Blog model, IFormFile FeaturedImage)
        {
            var blog = _context.Set<Blog>().Find(model.Id);
            if (blog == null) return NotFound();

            if (ModelState.IsValid)
            {
                blog.Title = model.Title;
                blog.Content = model.Content;
                blog.Summary = model.Summary;
                blog.IsPublished = model.IsPublished;
                blog.AuthorName = model.AuthorName;
                blog.UpdatedAt = DateTime.Now;

                if (FeaturedImage != null)
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/blogs");
                    Directory.CreateDirectory(uploadsFolder);
                    var fileName = Guid.NewGuid() + Path.GetExtension(FeaturedImage.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        FeaturedImage.CopyTo(stream);
                    }
                    blog.FeaturedImageUrl = "/uploads/blogs/" + fileName;
                }

                _context.Update(blog);
                _context.SaveChanges();
                TempData["Success"] = "Blog Updated Successfully!";
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var blog = _context.Tbl_Blog.Find(id);
            if (blog != null)
            {
                _context.Tbl_Blog.Remove(blog);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Blog deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
