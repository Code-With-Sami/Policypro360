using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PolicyPro360.Models
{
    public class TransactionHistory
    {
        [Key]
        public int Id { get; set; }
        public string? FromType { get; set; }
        public int FromId { get; set; }
        public string? ToType { get; set; }
        public int ToId { get; set; }
        public int? CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public virtual Company? Company { get; set; }

        public int? PolicyId { get; set; }
        [ForeignKey("PolicyId")]
        public virtual Policy? Policy { get; set; }
        public decimal Amount { get; set; }
        public string? Purpose { get; set; }
        public DateTime Date { get; set; }
    }
}
