using System;

namespace PolicyPro360.ViewModels
{
    public class WalletTransactionViewModel
    {
        public int TransactionId { get; set; }
        public string PolicyName { get; set; }
        public string CompanyName { get; set; }
        public string CompanyLogoUrl { get; set; }
        public decimal TotalPremium { get; set; }
        public decimal CommissionEarned { get; set; } 
        public DateTime Date { get; set; }
        public string Status { get; set; }
    }
} 