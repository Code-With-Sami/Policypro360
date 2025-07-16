using Microsoft.AspNetCore.Http;
using PolicyPro360.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PolicyPro360.ViewModels
{
    public class ClaimFormViewModel
    {
        [Required]
        public int SelectedCategoryId { get; set; }

        [Required]
        public int SelectedPolicyId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfIncident { get; set; }

        [Required]
        [StringLength(1000)]
        public string IncidentDetails { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Claimed amount must be greater than 0")]
        public decimal ClaimedAmount { get; set; }

        [Required]
        [StringLength(500)]
        public string UserRequest { get; set; }

        public List<IFormFile> SupportingDocuments { get; set; }

        public List<Category> Categories { get; set; } = new List<Category>();
        public List<Policy> Policies { get; set; } = new List<Policy>();
    }
}