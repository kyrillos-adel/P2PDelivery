using AutoMapper;
using P2PDelivery.Application.DTOs;
using P2PDelivery.Application.DTOs.MatchDTO;
using P2PDelivery.Application.Interfaces;
using P2PDelivery.Application.Interfaces.Services;
using P2PDelivery.Application.Response;
using P2PDelivery.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Match = P2PDelivery.Domain.Entities.Match;

namespace P2PDelivery.Application.Services
{
    public class MatchService : ImatchService
    {
        private readonly IRepository<Match> _matchrepository;
        private readonly IMapper _mapper;

        public MatchService(IRepository<Match> matchrepository, IMapper mapper)
        {
           _matchrepository = matchrepository;
            _mapper = mapper;
        }
        public async Task<RequestResponse<bool>> Addmatch(MatchDTO dTO)
        {
            var entity = _mapper.Map<Match>(dTO);
            await _matchrepository.AddAsync(entity);

            await _matchrepository.SaveChangesAsync();
            return RequestResponse<bool>.Success(true);

        }

        public async Task<bool> deletematch(int id)
        {
            var respond= await _matchrepository.GetByIDAsync(id);
            await _matchrepository.DeleteAsync(respond);
            return true;
           
        }

        public async Task<Match> GetByDelivery(int delivery)
        {
           var respond= _matchrepository.GetAll(x => x.DeliveryRequestId == delivery).FirstOrDefault();
            if (respond == null) 
                return respond;
            return respond;

        }


    }
}
