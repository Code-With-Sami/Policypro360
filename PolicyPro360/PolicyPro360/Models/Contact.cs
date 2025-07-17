using System;
using System.ComponentModel.DataAnnotations;

namespace PolicyPro360.Models
{
    public class Contact
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Please enter only letters for your name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        [StringLength(200)]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Message is required")]
        [StringLength(2000)]
        public string Message { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
} 