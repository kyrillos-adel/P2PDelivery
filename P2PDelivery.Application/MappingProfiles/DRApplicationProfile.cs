using P2PDelivery.Application.DTOs.ApplicationDTOs;
using P2PDelivery.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        }
    }
}
