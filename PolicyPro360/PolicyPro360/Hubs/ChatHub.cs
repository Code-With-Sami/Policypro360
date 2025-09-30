using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PolicyPro360.Models;

namespace PolicyPro360.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly myContext _db;

        public ChatHub(myContext db)
        {
            _db = db;
        }

        private string GroupName(int conversationId) => $"conv-{conversationId}";

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        public async Task JoinConversation(int conversationId)
        {
            // authorization: ensure caller is participant
            if (!await IsParticipant(conversationId))
            {
                throw new HubException("Not authorized to join this conversation");
            }
            await Groups.AddToGroupAsync(Context.ConnectionId, GroupName(conversationId));
        }

        public async Task LeaveConversation(int conversationId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, GroupName(conversationId));
        }


        public async Task SendMessage(int conversationId, string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return;

            if (!await IsParticipant(conversationId))
                throw new HubException("Not authorized to send message to this conversation");

            var (role, currentId) = GetCurrentRoleAndId();

            var message = new Message
            {
                ConversationId = conversationId,
                SenderType = role,
                SenderId = currentId,
                Text = text,
                CreatedAt = DateTime.UtcNow
            };

            _db.Messages.Add(message);

            var conv = await _db.Conversations.FindAsync(conversationId);
            if (conv != null)
            {
                conv.LastMessageAt = DateTime.UtcNow;
            }

            await _db.SaveChangesAsync();

            var payload = new
            {
                message.Id,
                message.ConversationId,
                message.SenderType,
                message.SenderId,
                message.Text,
                message.IsRead,
                message.CreatedAt
            };

            await Clients.Group(GroupName(conversationId)).SendAsync("ReceiveMessage", payload);
        }

        public async Task Typing(int conversationId, bool isTyping)
        {
            if (!await IsParticipant(conversationId)) return;
            await Clients.OthersInGroup(GroupName(conversationId)).SendAsync("Typing", conversationId, isTyping);
        }


        public async Task MarkMessagesRead(int conversationId)
        {
            if (!await IsParticipant(conversationId)) return;


            var (role, currentId) = GetCurrentRoleAndId();


            var messages = _db.Messages.Where(m => m.ConversationId == conversationId && !m.IsRead && !(m.SenderType == role && m.SenderId == currentId));
            await messages.ForEachAsync(m => m.IsRead = true);
            await _db.SaveChangesAsync();


            await Clients.Group(GroupName(conversationId)).SendAsync("MessagesMarkedRead", conversationId);
        }


        private (string role, int id) GetCurrentRoleAndId()
        {
            var user = Context.User;
            var role = user.FindFirst("Role")?.Value ?? "User"; // default to User if missing
            int id = 0;
            if (role == "Company")
            {
                var claim = user.FindFirst("CompanyId");
                if (claim == null) throw new HubException("CompanyId claim missing");
                id = int.Parse(claim.Value);
            }
            else
            {
                var claim = user.FindFirst("UserId");
                if (claim == null) throw new HubException("UserId claim missing");
                id = int.Parse(claim.Value);
            }
            return (role, id);
        }

        private async Task<bool> IsParticipant(int conversationId)
        {
            var conv = await _db.Conversations.FindAsync(conversationId);
            if (conv == null) return false;


            var (role, currentId) = GetCurrentRoleAndId();
            if (role == "Company") return conv.CompanyId == currentId;
            return conv.UserId == currentId;
        }
    }
}