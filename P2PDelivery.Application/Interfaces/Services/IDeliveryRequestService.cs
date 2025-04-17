using P2PDelivery.Application.DTOs;
using P2PDelivery.Application.DTOs.DeliveryRequestDTOs;
using P2PDelivery.Application.Response;
using P2PDelivery.Application.Response;
using P2PDelivery.Domain.Entities;

namespace P2PDelivery.Application.Interfaces.Services
{
    public interface IDeliveryRequestService
    {

        Task<RequestResponse<DeliveryRequestDetailsDTO>> GetDeliveryRequestDetailsAsync(int  deliveryId,int userID);
        Task<bool> IsDeliveryRequestExist(int deliveryId);
        Task<RequestResponse<DeliveryRequest>> UpdateAsync(int id, DeliveryRequestUpdateDto deliveryRequestUpdateDtodto);
        Task<RequestResponse<bool>> DeleteAsync(int id);
    }
}
