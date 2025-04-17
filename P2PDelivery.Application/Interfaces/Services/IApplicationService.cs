using P2PDelivery.Application.DTOs.ApplicationDTOs;
using P2PDelivery.Application.Response;

namespace P2PDelivery.Application.Interfaces.Services
{
    public interface IApplicationService
    {
        Task<RequestResponse<ICollection<ApplicationDTO>>> GetApplicationByRequestAsync(int deliveryRequestID);
    }
}
