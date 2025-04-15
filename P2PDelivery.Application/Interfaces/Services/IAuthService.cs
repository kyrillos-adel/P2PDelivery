using P2PDelivery.Application.DTOs;
using P2PDelivery.Application.Response;

namespace P2PDelivery.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<RequestResponse<RegisterDTO>> RegisterAsync(RegisterDTO registerDTO);
    }
}
