using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PolicyPro360.Models;

namespace PolicyPro360.Controllers.Admin
{
    public class AdminCompanyController : BaseAdminController
    {
        private readonly myContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminCompanyController(myContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var companies = await _context.Tbl_Company.ToListAsync();
            return View(companies);
        }

        public IActionResult PendingCompanies()
        {
            var companies = _context.Tbl_Company.Where(c => c.Status == "Pending").ToList();
            return View(companies);
        }

        public IActionResult ApprovedCompanies()
        {
            var companies = _context.Tbl_Company.Where(c => c.Status == "Approved").ToList();
            return View(companies);
        }

        public IActionResult RejectedCompanies()
        {
            var companies = _context.Tbl_Company.Where(c => c.Status == "Rejected").ToList();
            return View(companies);
        }

        public IActionResult Approve(int id)
        {
            var company = _context.Tbl_Company.Find(id);
            if (company != null)
            {
                company.Status = "Approved";
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Reject(int id)
        {
            var company = _context.Tbl_Company.Find(id);
            if (company != null)
            {
                company.Status = "Rejected";
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        //public async Task<IActionResult> ChangeStatus(int id, string status)
        //{
        //    var company = await _context.Tbl_Company.FindAsync(id);
        //    if (company == null) return NotFound();

        //    company.Status = status;
        //    _context.Tbl_Company.Update(company);
        //    await _context.SaveChangesAsync();

        //    return RedirectToAction(nameof(Index));
        //}
    }
}
