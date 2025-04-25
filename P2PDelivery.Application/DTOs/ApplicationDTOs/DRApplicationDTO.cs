using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PDelivery.Application.DTOs.ApplicationDTOs
{
     public class DRApplicationDTO
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double OfferedPrice { get; set; }
        public string ApplicationStatus { get; set; }

        public int DeliveryRequestId { get; set; }
        public string? DeliveryTitle { get; set; }

    }
}
