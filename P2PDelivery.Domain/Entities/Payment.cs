using System.ComponentModel.DataAnnotations;
using P2PDelivery.Domain.Enums;

namespace P2PDelivery.Domain.Entities;

public class Payment : BaseEntity
{
    public int DeliveryRequestId { get; set; } // Foreign key
    public DeliveryRequest? DeliveryRequest { get; set; }
    
    public int PayerId { get; set; } // Foreign key
    public User? Payer { get; set; }

    public int PayeeId { get; set; } // Foreign key
    public User? Payee { get; set; }

    [DataType(DataType.Currency)]
    public double Amount { get; set; }
    
    public DateTime Date { get; set; }
    
    public PaymentStatus Status { get; set; } // e.g. Pending, Completed, Failed
}