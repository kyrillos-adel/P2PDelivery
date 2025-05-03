using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PDelivery.Application.MappingProfiles
{
    using AutoMapper;
    using P2PDelivery.Application.DTOs.MatchDTO;
    using P2PDelivery.Domain.Entities;

    public class MatchMappingProfile : Profile
    {
        public MatchMappingProfile()
        {
           
            CreateMap<MatchDTO, Match>()

                .ForMember(dest => dest.ApplicationId, opt => opt.MapFrom(src => src.ApplicationId))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.DeliveryRequestId, opt => opt.MapFrom(src => src.DeliveryRequestId));
               

           
            CreateMap<Match, MatchDTO>();
        }
    }
}
