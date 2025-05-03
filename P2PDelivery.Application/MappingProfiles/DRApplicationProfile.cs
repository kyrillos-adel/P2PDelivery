using P2PDelivery.Application.DTOs.ApplicationDTOs;
using P2PDelivery.Domain.Entities;
using AutoMapper;

namespace P2PDelivery.Application.MappingProfiles
{
    class DRApplicationProfile : Profile
    {
        public DRApplicationProfile()
        { 
            CreateMap<DRApplication, DRApplicationDTO>()
                .ForMember(dest => dest.ApplicationStatus, opt => opt.MapFrom(src => src.ApplicationStatus.ToString()))
                .ForMember(dest => dest.DeliveryTitle, opt => opt.MapFrom(src => src.DeliveryRequest != null ? src.DeliveryRequest.Title : null));


            CreateMap<AddApplicationDTO, DRApplication>();

            CreateMap<ApplicationDTO, DRApplication>().ReverseMap()
                .ForMember( dst=> dst.UserName, opt=> opt.MapFrom(src=>src.User.FullName))
                .ForMember( dst=> dst.UserProfileUrl, opt=> opt.MapFrom(src=>src.User.ProfileImageUrl))
                ;
        }
    }
}
