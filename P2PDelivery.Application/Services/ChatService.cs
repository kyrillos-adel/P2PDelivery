using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
    
    public async Task<RequestResponse<ChatMessageDto>> SendMessage(ChatMessageDto message, int deliveryRequestId)
    {
        Chat? chat;
        
        if (message.ChatId == 0 && message.SenderId != 0 && message.ReceiverId != 0)
        {
            chat = _chatRepository.GetAll(c => (c.UserAId == message.SenderId && c.UserBId == message.ReceiverId) ||
                                               (c.UserBId == message.SenderId && c.UserAId == message.ReceiverId))
                .FirstOrDefault();

            if (chat == null)
            {
                chat = new Chat()
                {
                    DeliveryRequestId = deliveryRequestId,
                    UserAId = message.SenderId,
                    UserBId = message.ReceiverId
                };
                await _chatRepository.AddAsync(chat);
                await _chatRepository.SaveChangesAsync();
            }
            
            message.ChatId = chat.Id;
        }
        else
        {
            chat = await _chatRepository.GetByIDAsync(message.ChatId);
            if (chat == null)
                return RequestResponse<ChatMessageDto>.Failure(ErrorCode.ChatNotFound, "Chat not found");
        }
        
        var chatMessage = new ChatMessage();
        
        _mapper.Map<ChatMessageDto, ChatMessage>(message, chatMessage);
        
        chatMessage.ReceiverId = chat.UserAId == message.SenderId ? chat.UserBId : chat.UserAId;
        
        chatMessage.ChatId = chat.Id;
        
        await _chatMessageRepository.AddAsync(chatMessage);
        await _chatMessageRepository.SaveChangesAsync();
        
        return RequestResponse<ChatMessageDto>.Success(_mapper.Map<ChatMessageDto>(chatMessage), "Message sent successfully");
    }

    public async Task<RequestResponse<ChatDto>> GetChatById(int chatId, int userId)
    {
        var chat = await _chatRepository.GetByIDAsync(chatId);
        if (chat == null)
            return RequestResponse<ChatDto>.Failure(ErrorCode.ChatNotFound, "Chat not found");

        if (chat.UserAId == userId || chat.UserBId == userId)
        {
            var chatDto = _mapper.Map<ChatDto>(chat);
            return RequestResponse<ChatDto>.Success(chatDto);
        }
        
        return RequestResponse<ChatDto>.Failure(ErrorCode.Unauthorized, "You are not authorized to view this chat");
    }
    

    public async Task<RequestResponse<ICollection<ChatDto>>> GetChatsByUserId(int userId)
    {
        var chats = _chatRepository.GetAll(c => c.UserAId == userId || c.UserBId == userId)
            .Include(chat => chat.Messages)
            .Select(chatDto => new ChatDto
            {
                Id = chatDto.Id,
                DeliveryRequestId = chatDto.DeliveryRequestId,
                UserAId = chatDto.UserAId,
                UserBId = chatDto.UserBId,
                ChattingWith = userId == chatDto.UserAId ? chatDto.UserB.FullName : chatDto.UserA.FullName,
                Messages = chatDto.Messages.Select(m => new ChatMessageDto
                {
                    Id = m.Id,
                    SenderId = m.SenderId,
                    ReceiverId = m.ReceiverId,
                    Message = m.Message,
                    Date = m.Date,
                    IsReceived = m.IsReceived,
                    ChatId = m.ChatId
                }).ToList()
            }).ToList();
        
        
        
        if (chats == null || !chats.Any())
            return RequestResponse<ICollection<ChatDto>>.Failure(ErrorCode.ChatNotFound, "This user has no chats");
        
        var chatDtos = _mapper.Map<ICollection<ChatDto>>(chats);
        
        return RequestResponse<ICollection<ChatDto>>.Success(chatDtos);
    }
}