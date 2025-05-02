using P2PDelivery.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PDelivery.Application.DTOs.ApplicationDTOs
{
    public class ApplicationStatusDTO
    {
       
            public int Id { get; set; }
            public ApplicationStatus Status { get; set; }

    }
}
