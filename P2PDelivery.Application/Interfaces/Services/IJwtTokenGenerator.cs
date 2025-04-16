using P2PDelivery.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PDelivery.Application.Interfaces.Services
{
    public interface IJwtTokenGenerator
    {
        Task<string> GenerateToken(User user);
    }
}
