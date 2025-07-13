using System.ComponentModel.DataAnnotations;

namespace PolicyPro360.Models
{
    public class Admin
    {
        [Key]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string? Name { get; set; }
        
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [StringLength(150, ErrorMessage = "Email cannot be longer than 150 characters.")]
        public string? Email { get; set; }
        
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(255, MinimumLength = 4, ErrorMessage = "Password must be at least 4 characters long.")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        
        public string? Img { get; set; }
    }
}
