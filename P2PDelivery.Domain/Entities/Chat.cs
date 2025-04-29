using System.ComponentModel.DataAnnotations.Schema;

namespace P2PDelivery.Domain.Entities;

public class Chat : BaseEntity
{
    public int DeliveryRequestId { get; set; } // Foreign key
    public DeliveryRequest? DeliveryRequest { get; set; }

    [ForeignKey("UserA")]
    public int UserAId { get; set; } // Foreign key
    public User? UserA { get; set; }

    [ForeignKey("UserB")]
    public int UserBId { get; set; } // Foreign key
    public User? UserB { get; set; }

    /* Navigational properties */
    public virtual ICollection<ChatMessage>? Messages { get; set; } = new List<ChatMessage>();
}