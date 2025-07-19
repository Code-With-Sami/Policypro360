using System;
using System.Collections.Generic;
using PolicyPro360.Models;

namespace PolicyPro360.ViewModels
{
    public class DashboardViewModel
    {
        public int ActivePolicies { get; set; }
        public decimal TotalPremium { get; set; }
        public DateTime? NextPaymentDue { get; set; }
        public List<UserPolicy> Policies { get; set; }
        public List<MyApplicationViewModel> RecentApplications { get; set; }
        public List<UserPolicy> UpcomingPremiums { get; set; }
    }
}
