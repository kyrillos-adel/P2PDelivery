using P2PDelivery.Domain.Enums;

namespace P2PDelivery.Application.DTOs.Track
{
    public class AddTrackDTO
    {
        public TrackingStatus Status { get; set; } 
        public int UserId { get; set; }
        public int DeliveryRequestId { get; set; }
    }
}
