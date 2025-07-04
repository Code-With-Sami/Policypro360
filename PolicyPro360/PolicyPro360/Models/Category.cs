using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;



namespace PolicyPro360.Models
{
    [Table("Tbl_Category")]
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Category Name is required")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public bool Status { get; set; }

    }
}