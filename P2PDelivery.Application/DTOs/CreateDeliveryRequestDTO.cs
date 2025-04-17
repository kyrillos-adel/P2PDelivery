using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace P2PDelivery.Application.DTOs
{
    public class CreateDeliveryRequestDTO
    {
        [StringLength(50)] 
        public string Title { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public double TotalWeight { get; set; } // derived from the items


        public string PickUpLocation { get; set; }

        public string DropOffLocation { get; set; }

        public DateTime PickUpDate { get; set; }

        [DataType(DataType.Currency)]
        public double MinPrice { get; set; }

        [DataType(DataType.Currency)]
        public double MaxPrice { get; set; }
       
        public int UserId { get; set; } // Get this from token in production

    }
}
