using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PolicyPro360.Migrations;
using PolicyPro360.Models;
using System;
using System.Linq;

namespace PolicyPro360.Controllers.Company
{
    public class CompanyLoanController : BaseCompanyController
    {
        private readonly myContext _db;

        public CompanyLoanController(myContext db) : base(db)
        {
            _db = db;
        }

        public IActionResult PendingLoans()
        {
            var companyId = HttpContext.Session.GetInt32("companyId");
            if (companyId == null)
                return RedirectToAction("Login", "Company");

            var pendingLoans = _db.Tbl_LoanRequests
                .Include(lr => lr.User)
                .Include(lr => lr.Installments)
                .Where(lr => lr.Status == "Pending" &&
                    _db.Tbl_UserPolicy.Any(up => up.UserId == lr.UserId && up.Policy.CompanyId == companyId))
                .OrderByDescending(lr => lr.RequestDate)
                .ToList();

            return View("~/Views/CompanyLoan/PendingLoans.cshtml", pendingLoans);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ApproveLoan(int loanId, decimal disbursedAmount)
        {
            var companyId = HttpContext.Session.GetInt32("companyId");
            if (companyId == null)
                return RedirectToAction("Login", "Company");

            var loan = _db.Tbl_LoanRequests
                .Include(lr => lr.User)
                .FirstOrDefault(lr => lr.Id == loanId);

            if (loan == null)
            {
                ViewBag.ErrorMessage = "Loan request not found.";
                return RedirectToAction("PendingLoans");
            }

      
            bool isCompanyUser = _db.Tbl_UserPolicy.Any(up => up.UserId == loan.UserId && up.Policy.CompanyId == companyId);
            if (!isCompanyUser)
            {
                ViewBag.ErrorMessage = "You are not authorized to approve this loan.";
                return RedirectToAction("PendingLoans");
            }

          
            loan.Status = "Approved";
            loan.ApprovalDate = DateTime.Now;
            loan.DisbursedAmount = disbursedAmount;
            loan.StartDate = DateTime.Now;
            loan.EndDate = DateTime.Now.AddMonths(loan.DurationInMonths);

       
            decimal monthlyInterestRate = 0.01m; 
            decimal emi = CalculateEMI(loan.LoanAmount, monthlyInterestRate, loan.DurationInMonths);

            for (int i = 1; i <= loan.DurationInMonths; i++)
            {
                var installment = new LoanInstallment
                {
                    LoanRequestId = loan.Id,
                    DueDate = DateTime.Now.AddMonths(i),
                    Amount = emi,
                    Status = "Unpaid"
                };
                _db.Tbl_LoanInstallments.Add(installment);
            }

         
            var userWallet = _db.Tbl_UserWallet.FirstOrDefault(uw => uw.UserId == loan.UserId);
            if (userWallet == null)
            {
                userWallet = new UserWallet
                {
                    UserId = loan.UserId,
                    Balance = disbursedAmount,
                    LastUpdated = DateTime.Now,
                    Description = $"Loan disbursement for User {loan.UserId} - Loan ID: {loan.Id}",
                    PolicyId = loan.PolicyId
                };
                _db.Tbl_UserWallet.Add(userWallet);
            }
            else
            {
                userWallet.Balance += disbursedAmount;
                userWallet.LastUpdated = DateTime.Now;
            }

   
            var companyWallet = new CompanyWallet
            {
                UserId = loan.UserId,
                CompanyId = companyId.Value,
                PolicyId = loan.PolicyId, 
                Amount = -disbursedAmount, 
                Description = $"Loan disbursement for User {loan.UserId} - Loan ID: {loan.Id}",
                TransactionDate = DateTime.Now
            };
            _db.Tbl_CompanyWallet.Add(companyWallet);

            _db.SaveChanges();

            ViewBag.SuccessMessage = $"Loan request approved. Amount {disbursedAmount:C} disbursed to user wallet.";
            return RedirectToAction("PendingLoans");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RejectLoan(int loanId, string rejectionReason)
        {
            var companyId = HttpContext.Session.GetInt32("companyId");
            if (companyId == null)
                return RedirectToAction("Login", "Company");

            var loan = _db.Tbl_LoanRequests.Find(loanId);
            if (loan == null)
            {
                ViewBag.ErrorMessage = "Loan request not found.";
                return RedirectToAction("PendingLoans");
            }

         
            bool isCompanyUser = _db.Tbl_UserPolicy.Any(up => up.UserId == loan.UserId && up.Policy.CompanyId == companyId);
            if (!isCompanyUser)
            {
                ViewBag.ErrorMessage = "You are not authorized to reject this loan.";
                return RedirectToAction("PendingLoans");
            }

            loan.Status = "Rejected";
            loan.ApprovalDate = DateTime.Now;

            _db.SaveChanges();

            ViewBag.SuccessMessage = "Request rejected successfully.";
            return RedirectToAction("PendingLoans");
        }

        private decimal CalculateEMI(decimal principal, decimal monthlyRate, int months)
        {
            if (monthlyRate == 0) return principal / months;
            decimal rate = monthlyRate;
            decimal emi = principal * rate * (decimal)Math.Pow((double)(1 + rate), months);
            emi = emi / ((decimal)Math.Pow((double)(1 + rate), months) - 1);
            return Math.Round(emi, 2);
        }

        public IActionResult CompanyLoans()
        {
            int? companyId = HttpContext.Session.GetInt32("companyId");
            if (companyId == null)
            {
                return RedirectToAction("Login");
            }

            var loans = _db.Tbl_LoanRequests
                .Include(l => l.User)
                .Include(l => l.Policy)
                .Where(l => l.Policy.CompanyId == companyId)
                .OrderByDescending(l => l.RequestDate)
                .ToList();

            return View(loans);
        }

        public IActionResult ViewLoanDetails(int id)
        {
            var loan = _db.Tbl_LoanRequests
                .Include(l => l.User)
                .Include(l => l.Policy)
                .Include(l => l.Installments)
                .FirstOrDefault(l => l.Id == id);

            if (loan == null)
            {
                return RedirectToAction("CompanyLoans");
            }

            return View(loan);
        }

    }
} 