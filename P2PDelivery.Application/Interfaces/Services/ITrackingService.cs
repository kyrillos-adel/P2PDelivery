using P2PDelivery.Application.DTOs.Track;
using P2PDelivery.Application.Response;

namespace P2PDelivery.Application.Interfaces.Services
{
    public interface ITrackingService
    {
        Task<RequestResponse<bool>> AddNewStatus(AddTrackDTO addTrackDTO);
        RequestResponse<TrackDTO> GetLastTrack(int userID, int deliveryRequestID);
    }
}
