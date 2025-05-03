using P2PDelivery.Domain.Entities;
using P2PDelivery.Application.DTOs;
using P2PDelivery.Application.Response;
using P2PDelivery.Application.DTOs.DeliveryRequestDTOs;
using P2PDelivery.Domain.Helpers;
using P2PDelivery.Domain;


namespace P2PDelivery.Application.Interfaces.Services
{
    public interface IDeliveryRequestService
    {
        Task<RequestResponse<DeliveryRequestDTO>> CreateDeliveryRequestAsync(CreateDeliveryRequestDTO dto);
        Task<RequestResponse<DeliveryRequestDTO>> GetDeliveryRequestByIdAsync(int id);
        Task<RequestResponse<PageList<DeliveryRequestDTO>>> GetAllDeliveryRequestsAsync(DeliveryRequestParams deliveryRequestParams, int userID);
        Task<RequestResponse<List<DeliveryRequestDTO>>> GetDeliveryRequestsByUserIdAsync(int userId);
        Task<RequestResponse<DeliveryRequestDTO>>DeleteDeliveryRequestAsync(int id);

        Task<RequestResponse<DeliveryRequestDetailsDTO>> GetDeliveryRequestDetailsAsync(int  deliveryId,int userID);
        Task<bool> IsDeliveryRequestExist(int deliveryId);
        Task<RequestResponse<DeliveryRequest>> UpdateAsync(int id, DeliveryRequestUpdateDto deliveryRequestUpdateDtodto);
        Task<RequestResponse<bool>> DeleteAsync(int id);
        Task<bool> updatestatuse(int id ,int statuse);
    }
}
