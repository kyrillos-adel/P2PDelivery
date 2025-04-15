using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
namespace P2PDelivery.Domain.Entities;

public class User : IdentityUser<int>
{
    
    [StringLength(50)]
    public string FullName { get; set; }
    
    public string Address { get; set; }
    public string NatId { get; set; }
    
    public bool NatIdVerification { get; set; }
    
   
    /// <summary>
    /// /BaseEntity
    /// </summary>
  

    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public int? DeletedBy { get; set; }
    public DateTime? DeletedAt { get; set; }

    public bool IsDeleted { get; set; } = false;

    /* Navigational properties */
    [NotMapped]
    public ICollection<DeliveryRequest> DeliveryRequests { get; set; } = new List<DeliveryRequest>();

    [NotMapped]
    public ICollection<DRApplication> Applications { get; set; } = new List<DRApplication>();

    [NotMapped]
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();

    [NotMapped]
    public ICollection<DeliveryRequestUpdate> DeliveryRequestUpdates { get; set; } = new List<DeliveryRequestUpdate>();

    [NotMapped]
    public ICollection<Chat> Chats { get; set; } = new List<Chat>();
}