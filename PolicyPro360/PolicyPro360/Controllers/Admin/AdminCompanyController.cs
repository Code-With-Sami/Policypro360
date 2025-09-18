using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using PolicyPro360.Models;

namespace PolicyPro360.Controllers.Admin
{
    public class AdminCompanyController : BaseAdminController
    {
        private readonly myContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly PolicyPro360.Services.IEmailService _emailService;
        private readonly Microsoft.Extensions.Options.IOptions<PolicyPro360.Models.EmailSettings> _emailOptions;

        public AdminCompanyController(myContext context, IWebHostEnvironment webHostEnvironment, PolicyPro360.Services.IEmailService emailService, Microsoft.Extensions.Options.IOptions<PolicyPro360.Models.EmailSettings> emailOptions)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _emailService = emailService;
            _emailOptions = emailOptions;
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

        public IActionResult View(int id)
        {
            var company = _context.Tbl_Company
                .Include(c => c.Policies)
                .FirstOrDefault(c => c.Id == id);

            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        public async Task<IActionResult> Approve(int id)
        {
            var company = _context.Tbl_Company.Find(id);
            if (company != null)
            {
                company.Status = "Approved";
                _context.SaveChanges();

                // Send approval email
                if (!string.IsNullOrWhiteSpace(company.Email))
                {
                    var brand = string.IsNullOrWhiteSpace(_emailOptions.Value.DisplayName) ? "Asaan Zindagi" : _emailOptions.Value.DisplayName;
                    var subject = $"Your company has been approved - {brand}";
                    var body = $@"<div style='font-family:Segoe UI,Arial,sans-serif;font-size:14px;color:#222'>
                        <h2 style='color:#f5a526;margin:0 0 10px'>Approval Confirmed</h2>
                        <p>Dear {company.CompanyName},</p>
                        <p>We are pleased to inform you that your company account has been <strong>approved</strong> on {brand}.</p>
                        <p>You can now sign in and start managing your policies.</p>
                        <p style='margin-top:20px'>Regards,<br/>{brand} Team</p>
                    </div>";
                    try { await _emailService.SendAsync(company.Email, subject, body); } catch { }
                }
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Reject(int id)
        {
            var company = _context.Tbl_Company.Find(id);
            if (company != null)
            {
                company.Status = "Rejected";
                _context.SaveChanges();

                // Send rejection email
                if (!string.IsNullOrWhiteSpace(company.Email))
                {
                    var brand = string.IsNullOrWhiteSpace(_emailOptions.Value.DisplayName) ? "Asaan Zindagi" : _emailOptions.Value.DisplayName;
                    var subject = $"Company registration update - {brand}";
                    var body = $@"<div style='font-family:Segoe UI,Arial,sans-serif;font-size:14px;color:#222'>
                        <h2 style='color:#dc3545;margin:0 0 10px'>Registration Status Update</h2>
                        <p>Dear {company.CompanyName},</p>
                        <p>We regret to inform you that your company registration has been <strong>rejected</strong> on {brand}.</p>
                        <p>If you have any questions or would like to reapply, please contact our support team.</p>
                        <p style='margin-top:20px'>Regards,<br/>{brand} Team</p>
                    </div>";
                    try { await _emailService.SendAsync(company.Email, subject, body); } catch { }
                }
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var company = _context.Tbl_Company
                .Include(c => c.Policies)
                .FirstOrDefault(c => c.Id == id);

            if (company == null)
            {
                TempData["ErrorMessage"] = "Company not found.";
                return RedirectToAction("Index");
            }

            try
            {
 
                if (company.Policies != null && company.Policies.Any())
                {
                    TempData["ErrorMessage"] = "Cannot delete company. It has associated policies. Please delete policies first.";
                    return RedirectToAction("Index");
                }

                _context.Tbl_Company.Remove(company);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Company deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deleting company. Please try again.";
            }

            return RedirectToAction("Index");
        }
        public IActionResult AllCompanyPolicies()
        {
            var policies = _context.Tbl_Policy
                .Include(p => p.Category)
                .Include(p => p.Company)
                .ToList();
            return View(policies);
        }

        public IActionResult CompanyPoliciesByCategory(int categoryId)
        {
            var policies = _context.Tbl_Policy
                .Include(p => p.Category)
                .Include(p => p.Company)
                .Where(p => p.PolicyTypeId == categoryId)
                .ToList();
            return View("AllCompanyPolicies", policies);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            ViewBag.PolicyCategories = _context.Tbl_Category.Where(c => c.Status).ToList();
        }

        public IActionResult LifeCompanyPolicy()
        {
            var category = _context.Tbl_Category.FirstOrDefault(c => c.Name.ToLower().Contains("life"));
            if (category == null)
                return View("AllCompanyPolicies", new List<PolicyPro360.Models.Policy>());
            var policies = _context.Tbl_Policy
                .Include(p => p.Category)
                .Include(p => p.Company)
                .Where(p => p.PolicyTypeId == category.Id)
                .ToList();
            return View("AllCompanyPolicies", policies);
        }

        public IActionResult MotorCompanyPolicy()
        {
            var category = _context.Tbl_Category.FirstOrDefault(c => c.Name.ToLower().Contains("motor"));
            if (category == null)
                return View("AllCompanyPolicies", new List<PolicyPro360.Models.Policy>());
            var policies = _context.Tbl_Policy
                .Include(p => p.Category)
                .Include(p => p.Company)
                .Where(p => p.PolicyTypeId == category.Id)
                .ToList();
            return View("AllCompanyPolicies", policies);
        }

        public IActionResult HomeCompanyPolicy()
        {
            var category = _context.Tbl_Category.FirstOrDefault(c => c.Name.ToLower().Contains("home"));
            if (category == null)
                return View("AllCompanyPolicies", new List<PolicyPro360.Models.Policy>());
            var policies = _context.Tbl_Policy
                .Include(p => p.Category)
                .Include(p => p.Company)
                .Where(p => p.PolicyTypeId == category.Id)
                .ToList();
            return View("AllCompanyPolicies", policies);
        }

        public IActionResult MedicalCompanyPolicy()
        {
            var category = _context.Tbl_Category.FirstOrDefault(c => c.Name.ToLower().Contains("medical"));
            if (category == null)
                return View("AllCompanyPolicies", new List<PolicyPro360.Models.Policy>());
            var policies = _context.Tbl_Policy
                .Include(p => p.Category)
                .Include(p => p.Company)
                .Where(p => p.PolicyTypeId == category.Id)
                .ToList();
            return View("AllCompanyPolicies", policies);
        }

        public IActionResult DeletePolicy(int id)
        {
            var policy = _context.Tbl_Policy
                .Include(p => p.Attributes)
                .Include(p => p.Company)
                .FirstOrDefault(p => p.Id == id);

            if (policy == null)
            {
                TempData["ErrorMessage"] = "Policy not found.";
                return RedirectToAction("AllCompanyPolicies");
            }

            try
            {
                // Check if policy has any related data
                var hasUserPolicies = _context.Tbl_UserPolicy.Any(up => up.PolicyId == id);
                var hasUserClaims = _context.Tbl_UserClaims.Any(uc => uc.PolicyId == id);
                var hasAdminWallets = _context.Tbl_AdminWallet.Any(aw => aw.PolicyId == id);
                var hasCompanyWallets = _context.Tbl_CompanyWallet.Any(cw => cw.PolicyId == id);
                var hasTransactions = _context.Tbl_TransactionHistory.Any(th => th.PolicyId == id);
                var hasLoanRequests = _context.Tbl_LoanRequests.Any(lr => lr.PolicyId == id);
                var hasLoanInstallments = _context.Tbl_LoanInstallments.Any(li => _context.Tbl_LoanRequests.Any(lr => lr.Id == li.LoanRequestId && lr.PolicyId == id));
                var hasLoanPayments = _context.Tbl_LoanPayments.Any(lp => _context.Tbl_LoanInstallments.Any(li => li.Id == lp.LoanInstallmentId && _context.Tbl_LoanRequests.Any(lr => lr.Id == li.LoanRequestId && lr.PolicyId == id)));

                if (hasUserPolicies || hasUserClaims || hasAdminWallets || hasCompanyWallets || hasTransactions || hasLoanRequests || hasLoanInstallments || hasLoanPayments)
                {
                    // Store policy info in TempData for the confirmation view
                    TempData["PolicyToDelete"] = id;
                    TempData["PolicyName"] = policy.Name;
                    TempData["HasUserPolicies"] = hasUserPolicies;
                    TempData["HasUserClaims"] = hasUserClaims;
                    TempData["HasAdminWallets"] = hasAdminWallets;
                    TempData["HasCompanyWallets"] = hasCompanyWallets;
                    TempData["HasTransactions"] = hasTransactions;
                    TempData["HasLoanRequests"] = hasLoanRequests;
                    TempData["HasLoanInstallments"] = hasLoanInstallments;
                    TempData["HasLoanPayments"] = hasLoanPayments;
                    
                    return RedirectToAction("ConfirmPolicyDeletion");
                }

                // If no related data, delete directly
                DeletePolicyAndRelatedData(id);
                TempData["SuccessMessage"] = $"Policy '{policy.Name}' deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deleting policy. Please try again.";
            }

            return RedirectToAction("AllCompanyPolicies");
        }

        public IActionResult ConfirmPolicyDeletion()
        {
            if (TempData["PolicyToDelete"] == null)
            {
                return RedirectToAction("AllCompanyPolicies");
            }

            ViewBag.PolicyId = TempData["PolicyToDelete"];
            ViewBag.PolicyName = TempData["PolicyName"];
            ViewBag.HasUserPolicies = TempData["HasUserPolicies"];
            ViewBag.HasUserClaims = TempData["HasUserClaims"];
            ViewBag.HasAdminWallets = TempData["HasAdminWallets"];
            ViewBag.HasCompanyWallets = TempData["HasCompanyWallets"];
            ViewBag.HasTransactions = TempData["HasTransactions"];
            ViewBag.HasLoanRequests = TempData["HasLoanRequests"];
            ViewBag.HasLoanInstallments = TempData["HasLoanInstallments"];
            ViewBag.HasLoanPayments = TempData["HasLoanPayments"];

            return View();
        }

        public IActionResult ForceDeletePolicy(int id)
        {
            var policy = _context.Tbl_Policy
                .Include(p => p.Attributes)
                .Include(p => p.Company)
                .FirstOrDefault(p => p.Id == id);

            if (policy == null)
            {
                TempData["ErrorMessage"] = "Policy not found.";
                return RedirectToAction("AllCompanyPolicies");
            }

            try
            {
                DeletePolicyAndRelatedData(id);
                TempData["SuccessMessage"] = $"Policy '{policy.Name}' and all related data deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deleting policy. Please try again.";
            }

            return RedirectToAction("AllCompanyPolicies");
        }

        private void DeletePolicyAndRelatedData(int policyId)
        {
            // Delete related data in order
            var userPolicies = _context.Tbl_UserPolicy.Where(up => up.PolicyId == policyId).ToList();
            var userClaims = _context.Tbl_UserClaims.Where(uc => uc.PolicyId == policyId).ToList();
            var adminWallets = _context.Tbl_AdminWallet.Where(aw => aw.PolicyId == policyId).ToList();
            var companyWallets = _context.Tbl_CompanyWallet.Where(cw => cw.PolicyId == policyId).ToList();
            var transactions = _context.Tbl_TransactionHistory.Where(th => th.PolicyId == policyId).ToList();
            
            // Delete loan-related data
            var loanRequests = _context.Tbl_LoanRequests.Where(lr => lr.PolicyId == policyId).ToList();
            var loanRequestIds = loanRequests.Select(lr => lr.Id).ToList();
            var loanInstallments = _context.Tbl_LoanInstallments.Where(li => loanRequestIds.Contains(li.LoanRequestId)).ToList();
            var loanInstallmentIds = loanInstallments.Select(li => li.Id).ToList();
            var loanPayments = _context.Tbl_LoanPayments.Where(lp => loanInstallmentIds.Contains(lp.LoanInstallmentId)).ToList();

            // Remove related data
            if (userPolicies.Any()) _context.Tbl_UserPolicy.RemoveRange(userPolicies);
            if (userClaims.Any()) _context.Tbl_UserClaims.RemoveRange(userClaims);
            if (adminWallets.Any()) _context.Tbl_AdminWallet.RemoveRange(adminWallets);
            if (companyWallets.Any()) _context.Tbl_CompanyWallet.RemoveRange(companyWallets);
            if (transactions.Any()) _context.Tbl_TransactionHistory.RemoveRange(transactions);
            
            // Remove loan-related data in correct order
            if (loanPayments.Any()) _context.Tbl_LoanPayments.RemoveRange(loanPayments);
            if (loanInstallments.Any()) _context.Tbl_LoanInstallments.RemoveRange(loanInstallments);
            if (loanRequests.Any()) _context.Tbl_LoanRequests.RemoveRange(loanRequests);

            // Get and delete policy attributes
            var policy = _context.Tbl_Policy.Include(p => p.Attributes).FirstOrDefault(p => p.Id == policyId);
            if (policy?.Attributes != null && policy.Attributes.Any())
            {
                _context.Tbl_PolicyAttributes.RemoveRange(policy.Attributes);
            }

            // Finally delete the policy
            _context.Tbl_Policy.Remove(policy);
            _context.SaveChanges();
        }


    }
}
