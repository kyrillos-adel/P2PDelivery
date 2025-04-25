using System.ComponentModel.DataAnnotations.Schema;

namespace P2PDelivery.Domain.Entities;

public class ChatMessage : BaseEntity
{
    public DateTime Date { get; set; } = DateTime.UtcNow; // Date when the message was sent
    
    public string Message { get; set; }
    
    [ForeignKey("Sender")]
    public int SenderId { get; set; } // Foreign key
    public User? Sender { get; set; }
    
    [ForeignKey("Receiver")]
    public int ReceiverId { get; set; } // Foreign key
    public User? Receiver { get; set; }
    
    public bool IsReceived { get; set; } // true if the message is received by the receiver
    
    public int ChatId { get; set; } // Foreign key
    public Chat? Chat { get; set; }
}