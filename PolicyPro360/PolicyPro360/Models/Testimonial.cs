using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PolicyPro360.Models
{
    [Table("Tbl_Testimonial")]
    public class Testimonial
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual Users User { get; set; }

        [Required]
        [StringLength(500)]
        public string Feedback { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        public string Status { get; set; } = "Pending";  // Pending, Published, Rejected

        public DateTime SubmittedAt { get; set; } = DateTime.Now;
    }
}
