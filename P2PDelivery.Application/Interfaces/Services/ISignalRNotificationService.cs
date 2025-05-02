using P2PDelivery.Application.DTOs.Notifications;

namespace P2PDelivery.Application.Interfaces.Services;

public interface ISignalRNotificationService
{
    Task SendNotificationToUserAsync(NotificationDto notificationDto);
}