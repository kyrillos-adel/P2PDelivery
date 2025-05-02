using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
namespace P2PDelivery.Domain.Entities;

public class User : IdentityUser<int>
{

    [StringLength(20, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
    [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Full name must contain only letters and spaces.")]
    public string FullName { get; set; }
    [Required(ErrorMessage = "Address is required")]
    public string Address { get; set; }
    [RegularExpression(@"^2|3\d{1}\d{2}(0[1-9]|1[0-2])(0[1-9]|[12][0-9]|3[01])\d{2}\d{5}$")]
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

    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }

    //image url
    [MaxLength(300)]
    public string? ProfileImageUrl { get; set; }


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
    
    [NotMapped]
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}