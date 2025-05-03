using P2PDelivery.Domain.Enums;

namespace P2PDelivery.Domain.Entities;

/* To track who updated the delivery request and when */
public class DeliveryRequestUpdate : BaseEntity
{
    public DateTime Date { get; set; }
    
    public TrackingStatus Status { get; set; } 
    
    /* The user who updated the status */
    public int UserId { get; set; } // Foreign key
    public User? User { get; set; }
    
    public int DeliveryRequestId { get; set; } // Foreign key
    public DeliveryRequest? DeliveryRequest { get; set; }
}