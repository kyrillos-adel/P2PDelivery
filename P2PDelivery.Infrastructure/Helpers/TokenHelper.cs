using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using P2PDelivery.Domain.Enums;
using P2PDelivery.Infrastructure.Configurations;

namespace P2PDelivery.Infrastructure.Helpers
{
    public class TokenHelper
    {
        public static string GenerateToken(int userID, string name, Role role)
        {
            var key = Encoding.UTF8.GetBytes(JwtSettings.SecretKey);
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier , userID.ToString()),
                    new Claim(ClaimTypes.Name , name),
                    new Claim(ClaimTypes.Role , role.ToString())
                }),
                Issuer = JwtSettings.Issuer,
                Audience = JwtSettings.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Expires = DateTime.UtcNow.AddDays(2),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
