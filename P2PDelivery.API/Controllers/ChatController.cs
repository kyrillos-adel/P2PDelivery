using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P2PDelivery.Application.DTOs.ChatDTOs;
using P2PDelivery.Application.Interfaces.Services;
using P2PDelivery.Application.Response;

namespace P2PDelivery.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }
        
        
        [HttpGet("{chatId}")]
        public async Task<ActionResult<RequestResponse<ChatDto>>> GetChatById(int chatId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userId, out int userIdInt))
                return BadRequest();
            
            var response = await _chatService.GetChatById(chatId, userIdInt);
            
            if (response.ErrorCode == ErrorCode.UnAuthorize)
                return Unauthorized(response);
            
            if (!response.IsSuccess)
                return NotFound(response);
            
            return Ok(response);
        }

        [HttpGet("user")]
        public async Task<ActionResult<RequestResponse<ICollection<ChatDto>>>> GetUserChats()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            var response = await _chatService.GetChatsByUserId(int.Parse(userId));
            
            if (!response.IsSuccess)
                return NotFound(response);
            
            return Ok(response);
        }
    }
}
