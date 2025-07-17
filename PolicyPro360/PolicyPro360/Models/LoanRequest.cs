using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PolicyPro360.Models
{
    [Table("Tbl_LoanRequests")]
    public class LoanRequest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int PolicyId { get; set; }

        [ForeignKey("PolicyId")]
        public virtual Policy Policy { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal LoanAmount { get; set; }

        [Required, StringLength(50)]
        public string LoanType { get; set; }

        [Required]
        public string Purpose { get; set; }

        [Required]
        public int DurationInMonths { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = "Pending"; 

        public DateTime RequestDate { get; set; } = DateTime.Now;

        public DateTime? ApprovalDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? DisbursedAmount { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        // Navigation Properties
        [ForeignKey("UserId")]
        public virtual Users User { get; set; }
        public virtual ICollection<LoanInstallment> Installments { get; set; } = new List<LoanInstallment>();
    }
} 