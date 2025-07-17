using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PolicyPro360.Models
{
    [Table("Tbl_LoanPayments")]
    public class LoanPayment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int LoanInstallmentId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PaidAmount { get; set; }

        public DateTime PaymentDate { get; set; } = DateTime.Now;

        [StringLength(50)]
        public string PaymentMode { get; set; } // Wallet, Bank, Easypaisa, etc.

        // Navigation Properties
        [ForeignKey("UserId")]
        public virtual Users User { get; set; }

        [ForeignKey("LoanInstallmentId")]
        public virtual LoanInstallment LoanInstallment { get; set; }
    }
} 
