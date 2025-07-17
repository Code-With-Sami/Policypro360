using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PolicyPro360.Models
{
    [Table("Tbl_LoanInstallments")]
    public class LoanInstallment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int LoanRequestId { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = "Unpaid"; 

        public DateTime? PaidDate { get; set; }

        // Navigation Properties
        [ForeignKey("LoanRequestId")]
        public virtual LoanRequest LoanRequest { get; set; }
        public virtual ICollection<LoanPayment> Payments { get; set; } = new List<LoanPayment>();
    }
} 