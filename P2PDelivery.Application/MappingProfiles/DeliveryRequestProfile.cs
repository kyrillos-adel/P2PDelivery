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
             .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName)).ReverseMap();

        }
    }
}
