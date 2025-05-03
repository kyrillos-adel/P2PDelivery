using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PDelivery.Application.DTOs.MatchDTO
{
    public class MatchDTO
    {

        public int ApplicationId { get; set; }
        public int DeliveryRequestId { get; set; }
        public double Price { get; set; }

    }
}
