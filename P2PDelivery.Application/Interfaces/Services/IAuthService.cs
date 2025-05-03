using P2PDelivery.Application.DTOs;
using P2PDelivery.Application.Response;

namespace P2PDelivery.Application.Interfaces.Services;

public interface IAuthService
{
    LoginResponseDTO respond {  get; }
    Task<RequestResponse<LoginResponseDTO>> LoginAsync(LoginDTO loginDto);
    Task<RequestResponse<RegisterDTO>> RegisterAsync(RegisterDTO registerDTO);
    Task<RequestResponse<UserProfile>> GetByName(string username);
    Task<RequestResponse<string>> DeleteUser(string UserName);
    Task<UserProfile> GetUserProfile(string userName);

    Task<RequestResponse<string>> EditUserInfo(string UserName, UserProfile userProfile);
    Task<RequestResponse<string>> RecoverMyAccount( string username);

    Task<RequestResponse<LoginResponseDTO>> RefreshTokenAsync(string refreshToken);

    
}
