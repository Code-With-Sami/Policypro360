using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace PolicyPro360.Models
{
    public class Conversation
    {
        [Key]
        public int Id { get; set; }


        [Required]
        public int UserId { get; set; }


        [Required]
        public int CompanyId { get; set; }


        // Optional: if the conversation is about a specific policy
        public int? PolicyId { get; set; }


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastMessageAt { get; set; }


        public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}