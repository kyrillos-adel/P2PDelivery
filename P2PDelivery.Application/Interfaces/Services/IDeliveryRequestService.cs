using P2PDelivery.Application.DTOs;
using P2PDelivery.Application.Response;
using P2PDelivery.Domain.Entities;

namespace P2PDelivery.Application.Interfaces.Services
{
    public interface IDeliveryRequestService
    {
        Task<RequestResponse<DeliveryRequest>> UpdateAsync(int id, DeliveryRequestDto deliveryRequestDtodto);
        Task<RequestResponse<DeliveryRequest>> DeleteAsync(int id);
    }
}
