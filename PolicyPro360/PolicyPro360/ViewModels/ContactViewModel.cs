using System.ComponentModel.DataAnnotations;

namespace PolicyPro360.ViewModels
{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "Name is required.")]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Name must only contain letters and spaces.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        public string Phone { get; set; }

        [Required(ErrorMessage = "Subject is required.")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Message is required.")]
        public string Message { get; set; }
    }
}