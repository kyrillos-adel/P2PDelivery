using System.ComponentModel.DataAnnotations;

namespace P2PDelivery.Application.DTOs;

public class DeliveryRequestDto
{
    [StringLength(50)]
    public string Title { get; set; }
    
    [StringLength(500)]
    public string Description { get; set; }
    
    public double TotalWeight { get; set; } // derived from the items
    
    public string PickUpLocation { get; set; }
    
    public string DropOffLocation { get; set; }
    
    public DateTime PickUpDate { get; set; }
    
    public double MinPrice { get; set; }
    
    public double MaxPrice { get; set; }
}