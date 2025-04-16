using P2PDelivery.Application.DTOs.DeliveryRequestDTOs;
using P2PDelivery.Application.Response;

namespace P2PDelivery.Application.Interfaces.Services
{
    public interface IDeliveryRequestService
    {
        Task<RequestResponse<DeliveryRequestDetailsDTO>> GetDeliveryRequestDetailsAsync(int  deliveryId,int userID);
        Task<bool> IsDeliveryRequestExist(int deliveryId);
    }
}
