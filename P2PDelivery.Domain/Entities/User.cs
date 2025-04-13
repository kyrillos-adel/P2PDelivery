using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using P2PDelivery.Domain.Enums;

namespace P2PDelivery.Domain.Entities;

public class User : BaseEntity
{
    [StringLength(50)]
    public string FullName { get; set; }
    
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")]
    public string Email { get; set; }
    
    [RegularExpression(@"^(?:\+20|0)?1[0125][0-9]{8}$")]
    public string PhoneNumber { get; set; }
    
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$")]
    public string Password { get; set; }
    
    [RegularExpression(@"^2|3\d{1}\d{2}(0[1-9]|1[0-2])(0[1-9]|[12][0-9]|3[01])\d{2}\d{5}$")]
    public string NatId { get; set; }
    
    public bool NatIdVerification { get; set; }
    
    public Role Role { get; set; }

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