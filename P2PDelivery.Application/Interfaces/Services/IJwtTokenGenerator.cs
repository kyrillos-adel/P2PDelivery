using P2PDelivery.Domain.Entities;


namespace P2PDelivery.Application.Interfaces.Services
{
    public interface IJwtTokenGenerator
    {
        Task<string> GenerateToken(User user);
        string GenerateRefreshToken();
    }
    

}