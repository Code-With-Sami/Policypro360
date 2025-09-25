// Models/QuizResult.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PolicyPro360.Models
{
    [Table("Tbl_QuizResult")]
    public class QuizResult
    {
        [Key]
        public int Id { get; set; }
        public int? UserId { get; set; }  // nullable for guests
        public int QuizId { get; set; }
        [ForeignKey("QuizId")]
        public virtual Quiz Quiz { get; set; }

        // store category scores JSON, e.g. {"life":0.12,"medical":0.65,...}
        public string ScoresJson { get; set; }

        // CSV of suggested policy ids (optional)
        public string SuggestedPolicyIds { get; set; }

        // short textual suggestions or AI response JSON
        public string AiResponseJson { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public virtual ICollection<QuizAnswer> Answers { get; set; }
    }
}
