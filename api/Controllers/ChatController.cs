
using api.Models;
using api.Repositories.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace ChatApp.API.Controllers
{
    [ApiController]
    [Route("api/messages")]
    [Authorize]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageRepository _messageRepo;

        public MessagesController(IMessageRepository messageRepo)
        {
            _messageRepo = messageRepo;
        }

        public class SendMessageRequest { public int ReceiverId { get; set; } public string Content { get; set; } = string.Empty; }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetMessages(int userId)
        {
            var currentUserId = GetCurrentUserId();
            if (currentUserId == null) return Unauthorized();

            var messages = await _messageRepo.GetConversationAsync(currentUserId.Value, userId);
            return Ok(messages);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest request)
        {
            var currentUserId = GetCurrentUserId();
            if (currentUserId == null) return Unauthorized();

            var newMessage = new Message
            {
                SenderId = currentUserId.Value,
                ReceiverId = request.ReceiverId,
                Content = request.Content,
                Timestamp = DateTime.UtcNow
            };

            var savedMessage = await _messageRepo.CreateMessageAsync(newMessage);
            return Ok(savedMessage);
        }

        private int? GetCurrentUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                 ?? User.FindFirst("sub")?.Value
                 ?? User.FindFirst("id")?.Value;

            return int.TryParse(claim, out var id) ? id : null;
        }
    }
}