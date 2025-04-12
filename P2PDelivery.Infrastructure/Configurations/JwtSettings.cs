using Microsoft.Extensions.Configuration;

namespace P2PDelivery.Infrastructure.Configurations
{
    public class JwtSettings
    {
        public static string Issuer { get; set; }
        public static string Audience { get; set; }
        public static string SecretKey { get; set; }

        public static void Initialize(IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            Issuer = jwtSettings["Issuer"];
            Audience = jwtSettings["Audience"];
            SecretKey = jwtSettings["SecretKey"];
        }
    }
}
