using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using P2PDelivery.Application.Interfaces.Services;

namespace P2PDelivery.API.Hubs;

public class ChatHub : Hub
{
    private readonly IChatService _chatService;
    private static readonly Dictionary<string, string> userConnections = new();
    
    public ChatHub(IChatService chatService)
    {
        _chatService = chatService;
    }

    public override Task OnConnectedAsync()
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (!string.IsNullOrEmpty(userId))
            userConnections[userId] = Context.ConnectionId;

        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (!string.IsNullOrEmpty(userId))
            userConnections.Remove(userId);

        return base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessageToUser(string receiverId, string message/*, int deliveryRequestId*/)
    {
        // Get the sender's ID from the hub caller context
        var senderId = Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        // Check if the senderId is a valid integer
        if (!int.TryParse(senderId, out var senderIdInt))
            return;
        
        // Check if the receiverId is a valid integer
        if (!int.TryParse(receiverId, out var receiverIdInt))
            return;
        
        // Save the message to the database
        var response = await _chatService.SendMessage(message, senderIdInt, receiverIdInt, 1);
        if (response.IsSuccess)
        {
            // Notify the receiver
            var connectionId = userConnections.FirstOrDefault(x => x.Key == receiverId.ToString()).Value;
            if (!string.IsNullOrEmpty(connectionId))
            {
                await Clients.Client(connectionId).SendAsync("ReceiveMessage", response.Data);
            }
        }
    }
}