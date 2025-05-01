using System.Text.Json.Serialization;

namespace P2PDelivery.Application.DTOs.ChatDTOs;

public class ChatMessageDto
{
    public int Id { get; set; }
    
    public DateTime Date { get; set; } = DateTime.UtcNow; // Date when the message was sent
    
    public string Message { get; set; }
    
    public int SenderId { get; set; }
    
    public int ReceiverId { get; set; } 
    
    public bool IsReceived { get; set; }
    
    public int ChatId { get; set; } 
    
    [JsonIgnore]
    public ChatDto? Chat { get; set; }
}