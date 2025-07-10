using System.ComponentModel.DataAnnotations;

namespace PolicyPro360.ViewModels
{
    public class PremiumCalculationViewModel
    {
        public int PolicyId { get; set; }
        public string PolicyName { get; set; }
        public string PolicyType { get; set; } 

        [Display(Name = "Vehicle Value (PKR)")]
        public decimal? VehicleValue { get; set; }

        [Display(Name = "Vehicle Type")]
        public string VehicleType { get; set; } 

        [Display(Name = "Your Age")]
        public int? UserAge { get; set; }

        [Display(Name = "Desired Coverage Amount (PKR)")]
        public decimal? LifeCoverageAmount { get; set; }

        [Display(Name = "Property Value (PKR)")]
        public decimal? PropertyValue { get; set; }

        [Display(Name = "Location Risk")]
        public string LocationRisk { get; set; } 

        [Display(Name = "Number of Dependents")]
        public int? NumberOfDependents { get; set; }

        public decimal? CalculatedPremium { get; set; }
        public decimal? BasePremium { get; set; } 
    }
}