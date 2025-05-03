using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P2PDelivery.Application.Interfaces.Services;
using P2PDelivery.Application.Response;
using P2PDelivery.Application.DTOs.Notifications;

namespace P2PDelivery.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpGet]
    public async Task<ActionResult<RequestResponse<List<NotificationDto>>>> GetAllNotifications()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return BadRequest(RequestResponse<List<NotificationDto>>.Failure(ErrorCode.Unauthorized, "User ID not found in token."));

        var response = await _notificationService.GetAll(int.Parse(userId));
        
        if (response.IsSuccess)
            return Ok(response);
        
        return BadRequest(response);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<RequestResponse<NotificationDto>>> GetNotificationById(int id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return BadRequest(RequestResponse<NotificationDto>.Failure(ErrorCode.Unauthorized, "User ID not found in token."));

        var response = await _notificationService.GetByIdAsync(id);
        
        if (response.IsSuccess && response.Data.UserId == int.Parse(userId))
            return Ok(response);
        
        return NotFound(response);
    }
    
    [HttpPut("mark-as-read")]
    public async Task<ActionResult<RequestResponse<bool>>> MarkAsRead(ICollection<int> ids)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return BadRequest(RequestResponse<bool>.Failure(ErrorCode.Unauthorized, "User ID not found in token."));

        var response = await _notificationService.MarkAsReadAsync(ids, int.Parse(userId));
        
        if (response.IsSuccess)
            return Ok(response);
        
        return NotFound(response);
    }
    
    // [HttpPost]
    // public async Task<ActionResult<RequestResponse<NotificationDto>>> CreateNotification([FromBody] NotificationDto notification)
    // {
    //     var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    //     if (userId == null)
    //         return RequestResponse<NotificationDto>.Failure(ErrorCode.Unauthorized, "User ID not found in token.");
    //
    //     notification.UserId = int.Parse(userId);
    //     
    //     var response = await _notificationService.CreateAsync(notification);
    //
    //     if (response.IsSuccess)
    //         return Ok(response);
    //     
    //     return BadRequest(response);
    // }
}