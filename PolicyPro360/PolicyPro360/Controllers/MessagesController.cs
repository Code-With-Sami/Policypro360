using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PolicyPro360.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PolicyPro360.Controllers.Api
{
    [ApiController]
    [Route("api/messages")]
    public class MessagesController : ControllerBase
    {
        private readonly myContext _db;

        public MessagesController(myContext db)
        {
            _db = db;
        }

        // ✅ Send Message
        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] MessageDto dto)
        {
            // check conversation exists
            var conv = await _db.Conversations.FindAsync(dto.ConversationId);
            if (conv == null) return NotFound("Conversation not found");

            var msg = new Message
            {
                ConversationId = dto.ConversationId,
                Text = dto.Text,
                SenderType = dto.SenderType,   // "User" or "Company"
                SenderId = dto.SenderId,
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            };

            _db.Messages.Add(msg);

            // update conversation last message time
            conv.LastMessageAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            return Ok(new { msg.Id, msg.Text, msg.SenderType, msg.CreatedAt });
        }

        // ✅ Get Messages of a conversation
        [HttpGet("{conversationId}")]
        public async Task<IActionResult> GetMessages(int conversationId, int page = 1, int pageSize = 50)
        {
            var messages = await _db.Messages
                .Where(m => m.ConversationId == conversationId)
                .OrderBy(m => m.CreatedAt) // oldest first
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(m => new
                {
                    m.Id,
                    m.SenderType,
                    m.SenderId,
                    m.Text,
                    m.IsRead,
                    m.CreatedAt
                })
                .ToListAsync();

            return Ok(messages);
        }

        // ✅ Mark messages as read
        [HttpPost("mark-read")]
        public async Task<IActionResult> MarkAsRead([FromBody] MarkReadDto dto)
        {
            var msgs = await _db.Messages
                .Where(m => m.ConversationId == dto.ConversationId && !m.IsRead && m.SenderType != dto.ReaderType)
                .ToListAsync();

            foreach (var msg in msgs)
                msg.IsRead = true;

            await _db.SaveChangesAsync();

            return Ok(new { Updated = msgs.Count });
        }
    }

    // DTOs
    public class MessageDto
    {
        public int ConversationId { get; set; }
        public string Text { get; set; }
        public string SenderType { get; set; } // "User" or "Company"
        public int SenderId { get; set; }
    }

    public class MarkReadDto
    {
        public int ConversationId { get; set; }
        public string ReaderType { get; set; } // "User" or "Company"
    }
}
