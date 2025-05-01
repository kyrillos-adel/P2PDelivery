using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using P2PDelivery.Application.DTOs.ChatDTOs;
using P2PDelivery.Application.Interfaces.Services;

namespace P2PDelivery.API.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly IChatService _chatService;
    
    public ChatHub(IChatService chatService)
    {
        _chatService = chatService;
    }

    public override Task OnConnectedAsync()
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (!string.IsNullOrEmpty(userId))
            Connection.UserConnections[userId] = Context.ConnectionId;

        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (!string.IsNullOrEmpty(userId))
            Connection.UserConnections.Remove(userId);

        return base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessageToUser(ChatMessageDto message, string deliveryRequestId)
    {
        // Get the sender's ID from the hub caller context
        var senderId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        // Check if the senderId is a valid integer
        if (!int.TryParse(senderId, out var senderIdInt))
            return;
        
        message.SenderId = senderIdInt;
        
        if (!int.TryParse(deliveryRequestId, out var deliveryRequestIdInt))
            return;
        
        // Save the message to the database
        var response = await _chatService.SendMessage(message, deliveryRequestIdInt);
        if (response.IsSuccess)
        {
            // Notify the receiver
            var connectionId = Connection.UserConnections.FirstOrDefault(x => x.Key == response.Data.ReceiverId.ToString()).Value;
            if (!string.IsNullOrEmpty(connectionId))
            {
                await Clients.Client(connectionId).SendAsync("ReceiveMessage", response.Data);
            }
        }
    }
}