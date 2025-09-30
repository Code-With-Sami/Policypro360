using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace PolicyPro360.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }


        [Required]
        public int ConversationId { get; set; }


        [Required]
        [MaxLength(20)]
        public string SenderType { get; set; } // "User" or "Company"


        [Required]
        public int SenderId { get; set; }


        public string Text { get; set; }


        public bool IsRead { get; set; } = false;


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        [ForeignKey("ConversationId")]
        public virtual Conversation Conversation { get; set; }
    }
}