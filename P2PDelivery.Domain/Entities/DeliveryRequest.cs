using System.ComponentModel.DataAnnotations;
using P2PDelivery.Domain.Enums;

namespace P2PDelivery.Domain.Entities;

public class DeliveryRequest : BaseEntity
{
    [StringLength(50)]
    public string Title { get; set; }
    
    [StringLength(500)]
    public string Description { get; set; }
    
    
    public double TotalWeight { get; set; } // derived from the items
    
    public string PickUpLocation { get; set; }
    
    public string DropOffLocation { get; set; }
    
    public DateTime PickUpDate { get; set; }
    
    [DataType(DataType.Currency)]
    public double MinPrice { get; set; }
    
    [DataType(DataType.Currency)]
    public double MaxPrice { get; set; }
    
    public DeliveryRequestStatus Status { get; set; } // e.g. Pending, Accepted, Completed, Cancelled, Delivered
    
    public int UserId { get; set; } // Foreign key
    public User? User { get; set; }
    public string? DRImageUrl { get; set; }
    
    
    /* Navigational properties */
    public ICollection<Item> Items { get; set; } = new List<Item>();
    
    public ICollection<DRApplication> Applications { get; set; } = new List<DRApplication>();
    
    public ICollection<DeliveryRequestUpdate> Updates { get; set; } = new List<DeliveryRequestUpdate>();
    
    public Payment Payment { get; set; } // One-to-one relationship with Payment

    public Match Match { get; set; } // One-to-one relationship with Match

    public Chat Chat { get; set; } // One-to-one relationship with Chat

   
}