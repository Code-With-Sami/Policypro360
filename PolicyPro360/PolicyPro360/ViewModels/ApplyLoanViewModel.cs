using System.ComponentModel.DataAnnotations;

namespace PolicyPro360.ViewModels
{
    public class ApplyLoanViewModel
    {
        [Required]
        public int PolicyId { get; set; }

        [Required]
        [Range(1000, 10000000)]
        public decimal LoanAmount { get; set; }

        [Required]
        [StringLength(50)]
        public string LoanType { get; set; }

        [Required]
        public string Purpose { get; set; }

        [Required]
        [Range(1, 120)]
        public int DurationInMonths { get; set; }
    }
}
