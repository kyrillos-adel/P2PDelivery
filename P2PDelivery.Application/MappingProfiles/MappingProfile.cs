using AutoMapper;
using P2PDelivery.Application.DTOs;
using P2PDelivery.Application.DTOs.ChatDTOs;
using P2PDelivery.Application.DTOs.Notifications;
using P2PDelivery.Domain.Entities;

namespace P2PDelivery.Application.MappingProfiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<DeliveryRequest, DeliveryRequestUpdateDto>()
            .ForMember(dest => dest.TotalWeight, opt => opt.MapFrom(src => src.Items.Sum(i => i.Weight)))
            .ReverseMap();
        
        CreateMap<Chat, ChatDto>()
            // .ForMember()
            .ReverseMap();

        CreateMap<ChatMessage, ChatMessageDto>()
            // .ForMember()
            .ReverseMap();
        
        CreateMap<Notification, NotificationDto>()
            // .ForMember(n => n.IsRead, opt => opt.MapFrom(n => n.IsRead))
            .ReverseMap();
    }
}