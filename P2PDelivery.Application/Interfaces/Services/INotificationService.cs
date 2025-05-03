using P2PDelivery.Application.DTOs.Notifications;
using P2PDelivery.Application.Response;

namespace P2PDelivery.Application.Interfaces.Services;

public interface INotificationService
{
    Task<RequestResponse<NotificationDto>> CreateAsync(NotificationDto notificationDto);
    Task<RequestResponse<NotificationDto>> GetByIdAsync(int id);
    Task<RequestResponse<ICollection<NotificationDto>>> GetAll(int? userId);
    Task<RequestResponse<bool>> MarkAsReadAsync(ICollection<int> notificationIds, int userId);
}