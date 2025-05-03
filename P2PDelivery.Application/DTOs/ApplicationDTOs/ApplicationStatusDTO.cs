using P2PDelivery.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace P2PDelivery.Application.DTOs.ApplicationDTOs
{
    public class ApplicationStatusDTO
    {
        public int deleveryRequestId {  get; set; }
        public int Id { get; set; }
        public int Status { get; set; }

    }
}
