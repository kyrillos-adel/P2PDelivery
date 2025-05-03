using P2PDelivery.Application.DTOs.MatchDTO;
using P2PDelivery.Application.Response;
using P2PDelivery.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PDelivery.Application.Interfaces.Services
{
   public interface  ImatchService
    {
        public Task<RequestResponse<bool>> Addmatch(MatchDTO matchDTO);
        public Task<Match> GetByDelivery(int delivery);
        public Task<bool> deletematch(  int id);
    }
}
