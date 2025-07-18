using System.ComponentModel.DataAnnotations;

namespace PolicyPro360.ViewModels
{
    public class EditAdminViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100)]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        [StringLength(150)]
        public string? Email { get; set; }

        [StringLength(255, MinimumLength = 4, ErrorMessage = "Password must be at least 4 characters long.")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        public string? CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The new password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }

        public string? Img { get; set; }
    }
}