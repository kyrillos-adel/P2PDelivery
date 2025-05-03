using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using P2PDelivery.Application.DTOs.Notifications;
using P2PDelivery.Application.Services;

namespace P2PDelivery.API.Hubs;

[Authorize]
public class NotificationHub : Hub
{
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

    // public async Task SendNotification(NotificationDto notification)
    // {
    //     var connectionId = Connection.UserConnections.FirstOrDefault(x => x.Key == notification.UserId.ToString()).Value;
    //     if (!string.IsNullOrEmpty(connectionId))
    //     {
    //         await Clients.Client(connectionId).SendAsync("ReceiveNotification", notification);
    //     }
    // }
}