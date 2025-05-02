using System.ComponentModel.DataAnnotations;

namespace P2PDelivery.Domain.Entities;

public class Match : BaseEntity
{
    public int DeliveryRequestId { get; set; } // Foreign key
    public DeliveryRequest? DeliveryRequest { get; set; }
    
    public int ApplicationId { get; set; } // Foreign key
    public DRApplication? Application { get; set; }
    
    [DataType(DataType.Currency)]
    public double Price { get; set; }
    
    public DateTime Date { get; set; }
}