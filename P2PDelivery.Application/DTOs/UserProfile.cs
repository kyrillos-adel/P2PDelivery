using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PDelivery.Application.DTOs
{
    public class UserProfile
    {
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? NatId { get; set; }     
        public string? ProfileImageUrl { get; set; }
        public IFormFile? ProfileImage { get; set; }

    }
}
