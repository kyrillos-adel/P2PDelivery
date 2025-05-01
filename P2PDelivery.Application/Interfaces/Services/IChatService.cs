using P2PDelivery.Application.DTOs.ChatDTOs;
using P2PDelivery.Application.Response;

namespace P2PDelivery.Application.Interfaces.Services;

public interface IChatService
{
    Task<RequestResponse<ChatMessageDto>> SendMessage(ChatMessageDto message, int deliveryRequestId);
    Task<RequestResponse<ChatDto>> GetChatById(int chatId, int userId);
    Task<RequestResponse<ICollection<ChatDto>>> GetChatsByUserId(int userId);
}