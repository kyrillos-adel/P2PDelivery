using System.ComponentModel.DataAnnotations;

namespace P2PDelivery.Application.DTOs.Notifications;

public class NotificationDto
{
    public int? Id { get; set; }
    
    [Required]
    [StringLength(500)]
    public string Message { get; set; }
    
    [Required]
    public int UserId { get; set; }
    
    public bool? IsRead { get; set; }
    
    public DateTime? CreatedAt { get; set; }
}