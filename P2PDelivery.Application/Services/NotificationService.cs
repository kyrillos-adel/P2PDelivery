using AutoMapper;
using P2PDelivery.Application.DTOs.Notifications;
using P2PDelivery.Application.Interfaces;
using P2PDelivery.Application.Interfaces.Services;
using P2PDelivery.Application.Response;
using P2PDelivery.Domain.Entities;

namespace P2PDelivery.Application.Services;

public class NotificationService : INotificationService
{
    private readonly IRepository<Notification> _notificationRepository;
    private readonly IMapper _mapper;
    private readonly ISignalRNotificationService _signalRNotificationService;
    
    public NotificationService(
        IRepository<Notification> notificationRepository,
        IMapper mapper,
        ISignalRNotificationService signalRNotificationService)
    {
        _notificationRepository = notificationRepository;
        _mapper = mapper;
        _signalRNotificationService = signalRNotificationService;
    }
    
    public async Task<RequestResponse<NotificationDto>> CreateAsync(NotificationDto notificationDto)
    {
        var notification = _mapper.Map<Notification>(notificationDto);
        notification.CreatedAt = DateTime.UtcNow;
        notification.IsRead = false;

        try
        {
            await _notificationRepository.AddAsync(notification);
            await _notificationRepository.SaveChangesAsync();
            
            // Send notification to SignalR
            await _signalRNotificationService.SendNotificationToUserAsync(notificationDto);
            
            return RequestResponse<NotificationDto>.Success(_mapper.Map<NotificationDto>(notification), "Notification created successfully");
        }
        catch (Exception e)
        {
            return RequestResponse<NotificationDto>.Failure(ErrorCode.NotificationCreationError, e.Message);
        }
    }

    public async Task<RequestResponse<NotificationDto>> GetByIdAsync(int id)
    {
        var notification = await _notificationRepository.GetByIDAsync(id);

        if (notification != null)
            return RequestResponse<NotificationDto>.Success(_mapper.Map<NotificationDto>(notification), "Notification found");
        
        return RequestResponse<NotificationDto>.Failure(ErrorCode.NotificationNotFound, "Notification not found");
    }
    
    public Task<RequestResponse<ICollection<NotificationDto>>> GetAll(int? userId)
    {
        var notifications = userId != null ? 
            _notificationRepository.GetAll(x => x.UserId == userId) 
            : _notificationRepository.GetAll()
                .OrderByDescending(n => !n.IsRead)
                .ThenByDescending(n => n.CreatedAt);
        
        if (notifications != null)
            return Task.FromResult(RequestResponse<ICollection<NotificationDto>>.Success(_mapper.Map<ICollection<NotificationDto>>(notifications), "Notifications found"));
        
        return Task.FromResult(RequestResponse<ICollection<NotificationDto>>.Failure(ErrorCode.NotificationNotFound, "No notifications found"));
    }
}