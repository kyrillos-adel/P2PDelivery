using P2PDelivery.Domain.Entities;
using P2PDelivery.Application.DTOs;
using P2PDelivery.Application.Response;
namespace P2PDelivery.Application.Interfaces.Services
{
    public interface IDeliveryRequestService
    {
        Task<RequestResponse<DeliveryRequestDTO>> CreateDeliveryRequestAsync(CreateDeliveryRequestDTO dto);
        Task<RequestResponse<DeliveryRequestDTO>> GetDeliveryRequestByIdAsync(int id);
        Task<RequestResponse<List<DeliveryRequestDTO>>> GetDeliveryRequestsByUserIdAsync(int userId);
        Task<RequestResponse<DeliveryRequestDTO>>DeleteDeliveryRequestAsync(int id);

    }
}
