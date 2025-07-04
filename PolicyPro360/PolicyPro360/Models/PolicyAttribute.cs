using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PolicyPro360.Models
{
    public class PolicyAttribute
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Policy")]
        public int PolicyId { get; set; }
        public Policy Policy { get; set; }

        [Required]
        [StringLength(100)]
        public string Key { get; set; }

        [Required]
        public string Value { get; set; }
    }
}
