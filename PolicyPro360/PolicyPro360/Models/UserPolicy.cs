using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PolicyPro360.Models
{
    [Table("Tbl_UserPolicy")]
    public class UserPolicy
    {
        [Key]
        public int Id { get; set; }

        // Foreign Keys
        [Required]
        public int PolicyId { get; set; } 
        [ForeignKey("PolicyId")]
        public virtual Policy Policy { get; set; }

        [Required]
        public int UserId { get; set; } 
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal CalculatedPremium { get; set; }

        // Coverage Details
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal CoverageAmount { get; set; } 

        // Purchase Details
        [Required]
        public DateTime PurchaseDate { get; set; } = DateTime.Now;

        [Required]
        public DateTime ExpiryDate { get; set; }

        [Required]
        public string Status { get; set; } = "Active"; 
    }
}