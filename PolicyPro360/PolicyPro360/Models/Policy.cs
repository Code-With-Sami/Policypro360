using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PolicyPro360.Models
{
    public class Policy
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        // Foreign Keys
        [ForeignKey("Category")]
        public int PolicyTypeId { get; set; }
        public Category Category { get; set; }

        [ForeignKey("Company")]
        public int CompanyId { get; set; }
        public Company Company { get; set; }

        // Common fields
        [Column(TypeName = "decimal(18,2)")]
        public decimal SumInsured { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Premium { get; set; }

        public string Tenure { get; set; }

        public string TermsConditions { get; set; }

        public string BrochureUrl { get; set; }

        public bool Active { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<PolicyAttribute> Attributes { get; set; }

        [NotMapped]
        public object PolicyType { get; internal set; }
    }
}
