using P2PDelivery.Application.DTOs.Track;
using P2PDelivery.Application.Interfaces;
using P2PDelivery.Application.Interfaces.Services;
using P2PDelivery.Application.Response;
using P2PDelivery.Domain.Entities;

namespace P2PDelivery.Application.Services
{
    public class TrackingService : ITrackingService
    {
        private readonly IRepository<DeliveryRequestUpdate> _trackingRepo;
        public TrackingService(IRepository<DeliveryRequestUpdate> trackingrepo)
        {
            _trackingRepo = trackingrepo;
        }
        public async Task<RequestResponse<bool>> AddNewStatus(AddTrackDTO addTrackDTO)
        {
            var drUpdate = new DeliveryRequestUpdate
            {
                Date = DateTime.Now,
                DeliveryRequestId = addTrackDTO.DeliveryRequestId,
                UserId = addTrackDTO.UserId,
                Status = addTrackDTO.Status,
            };

            await _trackingRepo.AddAsync(drUpdate);
            await _trackingRepo.SaveChangesAsync();
            return RequestResponse<bool>.Success(true);
        }

        public RequestResponse<TrackDTO> GetLastTrack(int userID, int deliveryRequestID)
        {
            var track = _trackingRepo
                .GetAll(x=>x.UserId ==userID && x.DeliveryRequestId == deliveryRequestID)
                .OrderByDescending(x=>x.Date)
                .Select(x=>new TrackDTO
                {
                    Id = x.Id,
                    DeliveryRequestId = deliveryRequestID,
                    Date = x.Date,
                    UserId=x.UserId,
                    Status=x.Status,
                })
                .FirstOrDefault();

            return RequestResponse<TrackDTO>.Success(track);
        }
    }
}
