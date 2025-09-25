// Models/QuizAnswer.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PolicyPro360.Models
{
    [Table("Tbl_QuizAnswer")]
    public class QuizAnswer
    {
        [Key]
        public int Id { get; set; }
        public int? ResultId { get; set; }
        [ForeignKey("ResultId")]
        public virtual QuizResult Result { get; set; }

        public int QuestionId { get; set; }
        [ForeignKey("QuestionId")]
        public virtual QuizQuestion Question { get; set; }

        // CSV of selected option ids for multi/select questions. For numeric/raw, store raw text here.
        public string OptionIdsCsv { get; set; }

        public string RawAnswer { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
