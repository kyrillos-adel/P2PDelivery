using AutoMapper;
using Microsoft.EntityFrameworkCore;
using P2PDelivery.Application.Interfaces;
using P2PDelivery.Application.Interfaces.Services;
using P2PDelivery.Domain.Entities;
using P2PDelivery.Application.DTOs;
using P2PDelivery.Domain.Enums;
using P2PDelivery.Application.Response;

namespace P2PDelivery.Application.Services
{
    public class DeliveryRequestService : IDeliveryRequestService
    {
        private readonly IRepository<DeliveryRequest> _requestRepository;
        private readonly IMapper _mapper;
       

        public DeliveryRequestService(IRepository<DeliveryRequest> requestRepository, IMapper mapper)
        {
            _requestRepository = requestRepository;
            _mapper = mapper;
            
        }

        public async Task<RequestResponse<DeliveryRequestDTO>> CreateDeliveryRequestAsync(CreateDeliveryRequestDTO dto)
        {
            var entity=_mapper.Map<DeliveryRequest>(dto);
            await _requestRepository.AddAsync(entity);

            await _requestRepository.SaveChangesAsync();


            var resultDto=_mapper.Map<DeliveryRequestDTO>(entity);
            return RequestResponse<DeliveryRequestDTO>.Success(resultDto,"Created Successfully");

        }

        public  async Task<RequestResponse<DeliveryRequestDTO>> GetDeliveryRequestByIdAsync(int id)
        {
            var entity = await _requestRepository.GetByIDAsync(id);
            if (entity==null)
            {
                return RequestResponse<DeliveryRequestDTO>.Failure(ErrorCode.None, "Delivery Request not found");
            }
            var dto = _mapper.Map<DeliveryRequestDTO>(entity);
            return RequestResponse<DeliveryRequestDTO>.Success(dto);
        }



        public async Task<RequestResponse<List<DeliveryRequestDTO>>> GetDeliveryRequestsByUserIdAsync(int userId)
        {
            var query = _requestRepository.GetAll(x => x.UserId == userId);
            var entities = await query.ToListAsync();

            if (entities == null || !entities.Any())
            {
                return RequestResponse<List<DeliveryRequestDTO>>.Failure(ErrorCode.None, "No delivery requests found for this user");
            }

            var dtos = _mapper.Map<List<DeliveryRequestDTO>>(entities);
            return RequestResponse<List<DeliveryRequestDTO>>.Success(dtos);
        }

        //Delete Delivery Request
        public async Task<RequestResponse<DeliveryRequestDTO>> DeleteDeliveryRequestAsync(int id)
        {
            var entity = await _requestRepository.GetByIDAsync(id);
            if (entity == null)
            {
                return RequestResponse<DeliveryRequestDTO>.Failure(ErrorCode.None, "Delivery Request not found");
            }
            _requestRepository.Delete(entity);
            await _requestRepository.SaveChangesAsync();
            var dto = _mapper.Map<DeliveryRequestDTO>(entity);
            return RequestResponse<DeliveryRequestDTO>.Success(dto, "Deleted Successfully");
        }







    }
}
