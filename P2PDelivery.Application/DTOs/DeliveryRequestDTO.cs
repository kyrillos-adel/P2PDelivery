using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PDelivery.Application.DTOs
{
   public  class DeliveryRequestDTO
    {
        
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double TotalWeight { get; set; }
        public string PickUpLocation { get; set; }
        public string DropOffLocation { get; set; }
        public DateTime PickUpDate { get; set; }
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
        public string Status { get; set; } // e.g. Pending, Accepted, Completed, Cancelled, Delivered
        public int UserId { get; set; }
    }
}
