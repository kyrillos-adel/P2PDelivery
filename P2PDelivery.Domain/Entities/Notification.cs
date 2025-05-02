using System.ComponentModel.DataAnnotations;

namespace P2PDelivery.Domain.Entities;

public class Notification : BaseEntity
{
    [Required]
    [StringLength(500)]
    public string Message { get; set; }
    
    [Required]
    public int UserId { get; set; }
    public User? User { get; set; }
    
    public bool IsRead { get; set; }
}