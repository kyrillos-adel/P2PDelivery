using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PDelivery.Application.DTOs
{
    public class LoginDTO
    {
        [Required]
        public string Identifier { get; set; }

        [Required]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

    }

    public class LoginResponseDTO
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<string> Role { get; set; }
    }
}
