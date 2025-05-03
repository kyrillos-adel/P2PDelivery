using System;
using System.Collections.Generic;
using P2PDelivery.Domain.Enums;

namespace P2PDelivery.Application.DTOs.Track
{
    public class TrackDTO
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public TrackingStatus Status { get; set; }
        public int UserId { get; set; }
        public int DeliveryRequestId { get; set; }
    }
}
