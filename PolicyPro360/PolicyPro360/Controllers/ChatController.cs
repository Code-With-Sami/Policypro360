using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PolicyPro360.Models;

namespace PolicyPro360.Controllers
{

    public class ChatController : Controller
    {
        private readonly myContext _db;
        public ChatController(myContext db) { _db = db; }

        private bool IsCompany => HttpContext.Session.GetInt32("CompanyId") != null;
        private bool IsUser => HttpContext.Session.GetInt32("userId") != null;

        private int CurrentCompanyId => HttpContext.Session.GetInt32("CompanyId") ?? 0;
        private int CurrentUserId => HttpContext.Session.GetInt32("userId") ?? 0;

        // =================== Chat Pages ===================
        [HttpGet]
        public async Task<IActionResult> UserChat()
        {
            int? userId = HttpContext.Session.GetInt32("userId");

            // ✅ Get companies for which the user purchased policies
            var companies = await _db.Tbl_UserPolicy
                .Where(p => p.UserId == userId)
                .Include(p => p.Policy.Company) // Include the company through the policy
                .GroupBy(p => p.Policy.Company) // Group by the company
                .Select(g => new
                    {
                        g.Key.Id,
                        g.Key.CompanyName,
                        g.Key.Email
                    }
                )
                .ToListAsync();

            ViewBag.Companies = companies;
            ViewBag.UserId = userId;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CompanyChat()
        {
            var companyId = HttpContext.Session.GetInt32("companyId");

            // ✅ Get users who purchased policies from this company
            var users = await _db.Tbl_UserPolicy
                .Where(p => p.Policy.CompanyId == companyId)
                .Include(p => p.User) // Include user details
                .GroupBy(p => p.User) // group by user
                .Select(g => new
                {
                    g.Key.Id,
                    g.Key.Name,
                    g.Key.Email
                })
                .ToListAsync();

            ViewBag.Users = users;
            ViewBag.CompanyId = companyId;
            return View();
        }

        // =================== API Endpoints ===================
        [HttpGet("api/chat/conversations")]
        public async Task<IActionResult> GetConversations()
        {
            if (IsCompany)
            {
                var companyId = CurrentCompanyId;
                var convs = await _db.Conversations
                    .Where(c => c.CompanyId == companyId)
                    .OrderByDescending(c => c.LastMessageAt)
                    .Select(c => new
                    {
                        c.Id,
                        c.UserId,
                        c.CompanyId,
                        c.PolicyId,
                        c.LastMessageAt,
                        LastMessage = _db.Messages.Where(m => m.ConversationId == c.Id)
                            .OrderByDescending(m => m.CreatedAt)
                            .Select(m => new { m.Text, m.CreatedAt }).FirstOrDefault(),
                        UnreadCount = _db.Messages
                            .Where(m => m.ConversationId == c.Id && !m.IsRead && m.SenderType == "User")
                            .Count(),
                        User = _db.Tbl_Users
                            .Where(u => u.Id == c.UserId)
                            .Select(u => new { u.Id, u.Name, u.Email })
                            .FirstOrDefault()
                    })
                    .ToListAsync();

                return Ok(convs);
            }
            else if (IsUser)
            {
                var userId = CurrentUserId;
                var convs = await _db.Conversations
                    .Where(c => c.UserId == userId)
                    .OrderByDescending(c => c.LastMessageAt)
                    .Select(c => new
                    {
                        c.Id,
                        c.UserId,
                        c.CompanyId,
                        c.PolicyId,
                        c.LastMessageAt,
                        LastMessage = _db.Messages.Where(m => m.ConversationId == c.Id)
                            .OrderByDescending(m => m.CreatedAt)
                            .Select(m => new { m.Text, m.CreatedAt }).FirstOrDefault(),
                        UnreadCount = _db.Messages
                            .Where(m => m.ConversationId == c.Id && !m.IsRead && m.SenderType == "Company")
                            .Count(),
                        Company = _db.Tbl_Company
                            .Where(co => co.Id == c.CompanyId)
                            .Select(co => new { co.Id, co.CompanyName, co.Email })
                            .FirstOrDefault()
                    })
                    .ToListAsync();

                return Ok(convs);
            }

            return Unauthorized();
        }

        [HttpGet("api/chat/messages/{conversationId}")]
        public async Task<IActionResult> GetMessages(int conversationId, int page = 1, int pageSize = 50)
        {
            var conv = await _db.Conversations.FindAsync(conversationId);
            if (conv == null) return NotFound();

            if (IsCompany && conv.CompanyId != CurrentCompanyId) return Forbid();
            if (IsUser && conv.UserId != CurrentUserId) return Forbid();

            var messages = await _db.Messages
                .Where(m => m.ConversationId == conversationId)
                .OrderByDescending(m => m.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(m => new { m.Id, m.SenderType, m.SenderId, m.Text, m.IsRead, m.CreatedAt })
                .ToListAsync();

            messages.Reverse();
            return Ok(messages);
        }

        [HttpPost("api/chat/ensure-conversation")]
        public async Task<IActionResult> EnsureConversation([FromBody] EnsureConversationDto dto)
        {
            if (IsUser && dto.UserId != CurrentUserId) return Forbid();
            if (IsCompany && dto.CompanyId != CurrentCompanyId) return Forbid();

            var conv = await _db.Conversations
                .FirstOrDefaultAsync(c => c.UserId == dto.UserId && c.CompanyId == dto.CompanyId && c.PolicyId == dto.PolicyId);

            if (conv == null)
            {
                conv = new Conversation { UserId = dto.UserId, CompanyId = dto.CompanyId, PolicyId = dto.PolicyId };
                _db.Conversations.Add(conv);
                await _db.SaveChangesAsync();
            }
            return Ok(new { conv.Id });
        }
    }

    public class EnsureConversationDto
    {
        public int UserId { get; set; }
        public int CompanyId { get; set; }
        public int? PolicyId { get; set; }
    }
}
