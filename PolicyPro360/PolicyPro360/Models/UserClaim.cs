using Microsoft.AspNetCore.Http;
using PolicyPro360.Migrations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PolicyPro360.Models
{
    [Table("Tbl_UserClaims")]
    public class UserClaim
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }  // Who submitted the claim
        [ForeignKey("UserId")]
        public virtual Users Users { get; set; }

        [Required]
        public int PolicyCategoryId { get; set; }  // Category selected
        [ForeignKey("PolicyCategoryId")]
        public virtual Category Category { get; set; }

        [Required]
        public int PolicyId { get; set; }  // Policy selected
        [ForeignKey("PolicyId")]
        public virtual Policy Policy{ get; set; }

        [Required]
        public DateTime DateOfIncident { get; set; }

        [Required]
        [StringLength(1000)]
        public string IncidentDetails { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ClaimedAmount { get; set; }

        [Required]
        [StringLength(500)]
        public string UserRequest { get; set; }  // What user is requesting

        public string? SupportingDocumentPath { get; set; }  // Uploaded doc

        public string Status { get; set; } = "Pending";

        public DateTime SubmittedAt { get; set; } = DateTime.Now;

       

    }
}
