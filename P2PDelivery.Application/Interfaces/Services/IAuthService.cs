using P2PDelivery.Application.DTOs;

namespace P2PDelivery.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDTO> LoginAsync(LoginDTO loginDto);
    }
}
