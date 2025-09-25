// Models/QuizOption.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PolicyPro360.Models
{
    [Table("Tbl_QuizOption")]
    public class QuizOption
    {
        [Key]
        public int Id { get; set; }
        public int QuestionId { get; set; }
        [ForeignKey("QuestionId")]
        public virtual QuizQuestion Question { get; set; }

        [Required, MaxLength(500)]
        public string Text { get; set; }

        /// <summary>
        /// JSON string describing category weights, e.g. {"life":0.2,"medical":0.8,"motor":0,"home":0}
        /// </summary>
        public string CategoryWeightsJson { get; set; } = "{}";

        // optional numeric weight
        public decimal Weight { get; set; } = 1.0m;
    }
}
