using System.ComponentModel.DataAnnotations;
using P2PDelivery.Domain.Enums;

namespace P2PDelivery.Domain.Entities;

public class DRApplication : BaseEntity
{
    public DateTime Date { get; set; }
    
    [DataType(DataType.Currency)]
    public double OfferedPrice { get; set; }
    
    public ApplicationStatus ApplicationStatus { get; set; } // e.g. Pending, Accepted, Rejected, Completed, Cancelled
    
    public int UserId { get; set; } // Foreign key
    public User? User { get; set; }
    
    public int DeliveryRequestId { get; set; } // Foreign key
    public DeliveryRequest? DeliveryRequest { get; set; }
    
    /* Navigational properties */
    
}