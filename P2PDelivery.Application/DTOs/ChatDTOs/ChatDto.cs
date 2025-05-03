using P2PDelivery.Domain.Entities;

namespace P2PDelivery.Application.DTOs.ChatDTOs;

public class ChatDto
{
    public int Id { get; set; }
    
    public int DeliveryRequestId { get; set; } // Foreign key
    
    public int UserAId { get; set; } // Foreign key
    
    public int UserBId { get; set; } // Foreign key
    
    public string ChattingWith { get; set; } = string.Empty;
    
    public ICollection<ChatMessageDto> Messages { get; set; } = new List<ChatMessageDto>();
}