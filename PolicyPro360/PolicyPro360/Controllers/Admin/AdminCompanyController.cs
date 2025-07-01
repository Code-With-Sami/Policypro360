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

        public async Task<IActionResult> ChangeStatus(int id, string status)
        {
            var company = await _context.Tbl_Company.FindAsync(id);
            if (company == null) return NotFound();

            company.Status = status;
            _context.Tbl_Company.Update(company);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
