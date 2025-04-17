using AutoMapper;
using P2PDelivery.Application.DTOs;
using P2PDelivery.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PDelivery.Application.MappingProfiles
{
    public class DeliveryRequestProfile : Profile
    {
        public DeliveryRequestProfile()
        { 

           CreateMap<CreateDeliveryRequestDTO, DeliveryRequest>();
            CreateMap<DeliveryRequest, DeliveryRequestDTO>();
        }
    }
}
