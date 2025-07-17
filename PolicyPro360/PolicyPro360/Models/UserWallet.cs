using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PolicyPro360.Models
{
    [Table("Tbl_UserWallet")]
    public class UserWallet
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; } = 0;

        public DateTime LastUpdated { get; set; } = DateTime.Now;

        public int? PolicyId { get; set; }
        [ForeignKey("PolicyId")]
        public virtual Policy Policy { get; set; }
        public string Description { get; set; }

        // Navigation Properties
        [ForeignKey("UserId")]
        public virtual Users User { get; set; }
    }
} 