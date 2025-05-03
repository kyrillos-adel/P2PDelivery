using AutoMapper;
using P2PDelivery.Application.DTOs;
using P2PDelivery.Domain.Entities;


namespace P2PDelivery.Application.MappingProfiles
{
    public class DeliveryRequestProfile : Profile
    {
        public DeliveryRequestProfile()
        { 

           CreateMap<CreateDeliveryRequestDTO, DeliveryRequest>();
            CreateMap<DeliveryRequest, DeliveryRequestDTO>()
             .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FullName))
             .ForMember(dest => dest.DRImageUrl, opt => opt.MapFrom(src => src.DRImageUrl)) // assuming this property exists on entity
             .ForMember(dest => dest.ProfileImageUrl, opt => opt.MapFrom(src => src.User.ProfileImageUrl)) // assuming User has this
             .ReverseMap();
        }
    }
}
