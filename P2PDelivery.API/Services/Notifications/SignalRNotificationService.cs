using Microsoft.AspNetCore.SignalR;
using P2PDelivery.API.Hubs;
using P2PDelivery.Application.DTOs.Notifications;
using P2PDelivery.Application.Interfaces.Services;

namespace P2PDelivery.API.Services.Notifications;

public class SignalRNotificationService : ISignalRNotificationService
{
    private readonly IHubContext<NotificationHub> _hubContext;
    
    public SignalRNotificationService(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }
    
    public Task SendNotificationToUserAsync(NotificationDto notification)
    {
        var connectionId = Connection.UserConnections.FirstOrDefault(x => x.Key == notification.UserId.ToString()).Value;
        
        if (!string.IsNullOrEmpty(connectionId))
        {
            return _hubContext.Clients.Client(connectionId).SendAsync("ReceiveNotification", notification);
        }
        
        return Task.CompletedTask;
    }
}