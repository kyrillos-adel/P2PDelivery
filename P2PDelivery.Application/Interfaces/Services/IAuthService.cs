using P2PDelivery.Application.DTOs;
using P2PDelivery.Application.Response;
using P2PDelivery.Domain.Entities;
namespace P2PDelivery.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<RequestResponse<LoginResponseDTO>> LoginAsync(LoginDTO loginDto);
        Task<RequestResponse<RegisterDTO>> RegisterAsync(RegisterDTO registerDTO);
        
        Task<RequestResponse<string>> GetByName(string username);

        Task<RequestResponse<string>> DeleteUserNameIdAsync(string UserName);
    }
}
