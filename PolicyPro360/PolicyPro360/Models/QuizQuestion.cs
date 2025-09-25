// Models/QuizQuestion.cs
using Microsoft.VisualBasic.FileIO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PolicyPro360.Models
{
    [Table("Tbl_QuizQuestion")]
    public class QuizQuestion
    {
        [Key]
        public int Id { get; set; }
        public int QuizId { get; set; }
        [ForeignKey("QuizId")]
        public virtual Quiz Quiz { get; set; }

        [Required, MaxLength(1000)]
        public string Text { get; set; }

        // "single","multi","numeric"
        [MaxLength(50)]
        public string QuestionType { get; set; } = "single";

        public int Order { get; set; }

        public virtual ICollection<QuizOption> Options { get; set; }
    }
}
