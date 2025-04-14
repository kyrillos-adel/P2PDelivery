using AutoMapper;
using P2PDelivery.Application.DTOs;
using P2PDelivery.Domain.Entities;

namespace P2PDelivery.Application.MappingProfiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<DeliveryRequest, DeliveryRequestDto>()
            .ForMember(dest => dest.TotalWeight, opt => opt.MapFrom(src => src.Items.Sum(i => i.Weight)))
            .ReverseMap();;
    }
}