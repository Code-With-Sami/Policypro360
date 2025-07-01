using System;
using System.ComponentModel.DataAnnotations;
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

        [Display(Name = "Business Type")]
        public string BusinessType { get; set; }

        [Display(Name = "Industry Type")]
        public string IndustryType { get; set; }

        public string Description { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Phone No")]
        public string PhoneNo { get; set; }

        [Display(Name = "Company Logo")]
        public string CompanyLogoPath { get; set; } // Store image path

        // Owner Information
        [Display(Name = "Owner Name")]
        public string OwnerName { get; set; }

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime? OwnerDOB { get; set; }

        [Display(Name = "Owner Nationality")]
        public string OwnerNationality { get; set; }

        [EmailAddress]
        [Display(Name = "Owner Email")]
        public string OwnerEmail { get; set; }

        [Display(Name = "Owner Phone No")]
        public string OwnerPhoneNo { get; set; }

        [Display(Name = "Owner Role / Position")]
        public string OwnerRole { get; set; }

        [Display(Name = "Owner CNIC")]
        public string OwnerCNIC { get; set; }

        [Display(Name = "Owner Image")]
        public string OwnerImagePath { get; set; } // Store image path

        // Registration Info
        [Display(Name = "Registration No")]
        public string RegistrationNo { get; set; }

        public string Status { get; set; } = "Pending";
    }
}
