using Microsoft.AspNetCore.Mvc;
using P2PDelivery.Application.DTOs.ChatDTOs;
using P2PDelivery.Application.Interfaces.Services;
using P2PDelivery.Application.Response;

namespace P2PDelivery.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            var response = await _chatService.GetChatById(chatId);
            
            if (!response.IsSuccess)
                return NotFound(response);
            
            return Ok(response);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<RequestResponse<ICollection<ChatDto>>>> GetChatsByUserId(int userId)
        {
            var response = await _chatService.GetChatsByUserId(userId);
            
            if (!response.IsSuccess)
                return NotFound(response);
            
            return Ok(response);
        }
    }
}
