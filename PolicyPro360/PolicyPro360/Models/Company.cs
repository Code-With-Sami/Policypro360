using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace PolicyPro360.Models
{
    public class Company
    {
        [Key]
        public int Id { get; set; }

        // Company Information
        [Required]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }
        
        [Required]
        [Display(Name = "Business Type")]
        public string BusinessType { get; set; }

        [Required]
        [Display(Name = "Industry Type")]
        public string IndustryType { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }


        [Required]
        [Display(Name = "Phone No")]
        public string PhoneNo { get; set; }


        [Display(Name = "Company Logo")]
        public string? CompanyLogoPath { get; set; }

        // Owner Information
        [Required]
        [Display(Name = "Owner Name")]
        public string OwnerName { get; set; }

        [Required]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime? OwnerDOB { get; set; }

        [Required]
        [Display(Name = "Owner Nationality")]
        public string OwnerNationality { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Owner Email")]
        public string OwnerEmail { get; set; }

        [Required]
        [Display(Name = "Owner Phone No")]
        public string OwnerPhoneNo { get; set; }

        [Required]
        [Display(Name = "Owner Role / Position")]
        public string OwnerRole { get; set; }

        [Required]
        [Display(Name = "Owner CNIC")]
        public string OwnerCNIC { get; set; }


        [Display(Name = "Owner Image")]
        public string? OwnerImagePath { get; set; }

        // Registration Info
        [Required]
        [Display(Name = "Registration No")]
        public string RegistrationNo { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [NotMapped]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string? Status { get; set; } = "Pending";
    }
}
