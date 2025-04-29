using AutoMapper;
using P2PDelivery.Application.DTOs.ChatDTOs;
using P2PDelivery.Application.Interfaces;
using P2PDelivery.Application.Interfaces.Services;
using P2PDelivery.Application.Response;
using P2PDelivery.Domain.Entities;

namespace P2PDelivery.Application.Services;

public class ChatService : IChatService
{
    private readonly IRepository<ChatMessage> _chatMessageRepository;
    private readonly IRepository<Chat> _chatRepository;
    private readonly IMapper _mapper;
    
    public ChatService(IRepository<ChatMessage> chatMessageRepository,
        IRepository<Chat> chatRepository,
        IMapper mapper
        )
    {
        _mapper = mapper;
        _chatRepository = chatRepository;
        _chatMessageRepository = chatMessageRepository;
    }
    
    public async Task<RequestResponse<ChatMessageDto>> SendMessage(string message, int senderId, int receiverId, int deliveryRequestId)
    {
        var chat = _chatRepository.GetAll(c => ((c.UserAId == senderId && c.UserBId == receiverId) || 
                                                (c.UserAId == receiverId && c.UserBId == senderId)) && 
                                               c.DeliveryRequestId == deliveryRequestId)
            .FirstOrDefault();
        
        if (chat == null)
        {
            chat = new Chat()
            {
                DeliveryRequestId = deliveryRequestId,
                UserAId = senderId,
                UserBId = receiverId
            };
            await _chatRepository.AddAsync(chat);
            await _chatRepository.SaveChangesAsync();
        }
        
        var chatMessage = new ChatMessage()
        {
            Message = message,
            SenderId = senderId,
            ReceiverId = receiverId,
            ChatId = chat.Id,
            IsReceived = false
        };
        
        await _chatMessageRepository.AddAsync(chatMessage);
        await _chatMessageRepository.SaveChangesAsync();
        
        return RequestResponse<ChatMessageDto>.Success(_mapper.Map<ChatMessageDto>(chatMessage), "Message sent successfully");
    }

    public async Task<RequestResponse<ChatDto>> GetChatById(int chatId)
    {
        var chat = await _chatRepository.GetByIDAsync(chatId);
        if (chat == null)
            return RequestResponse<ChatDto>.Failure(ErrorCode.ChatNotFound, "Chat not found");
        
        var chatDto = _mapper.Map<ChatDto>(chat);
        return RequestResponse<ChatDto>.Success(chatDto);
    }

    public async Task<RequestResponse<ICollection<ChatDto>>> GetChatsByUserId(int userId)
    {
        var chats = _chatRepository.GetAll(c => c.UserAId == userId || c.UserBId == userId).ToList();
        
        if (chats == null || !chats.Any())
            return RequestResponse<ICollection<ChatDto>>.Failure(ErrorCode.ChatNotFound, "This user has no chats");
        
        var chatDtos = _mapper.Map<ICollection<ChatDto>>(chats);
        
        return RequestResponse<ICollection<ChatDto>>.Success(chatDtos);
    }
}