using System.ComponentModel.DataAnnotations;

namespace P2PDelivery.Domain.Entities;

public class Item : BaseEntity
{
    [StringLength(50)]
    public string Name { get; set; }
    
    public decimal Weight { get; set; }
    
    public string ImageUrl { get; set; }
    
    public int DeliveryRequestId { get; set; } // Foreign key
    public DeliveryRequest? DeliveryRequest { get; set; }
}