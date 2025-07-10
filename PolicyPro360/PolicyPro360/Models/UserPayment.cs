using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PolicyPro360.Models
{
    [Table("Tbl_UserPayment")]
    public class UserPayment
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string PayerName { get; set; }
        [Required]
        public string PayerEmail { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }
        public string Reference { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }
} 