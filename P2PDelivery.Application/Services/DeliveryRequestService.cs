using Microsoft.EntityFrameworkCore;
using P2PDelivery.Application.DTOs.DeliveryRequestDTOs;
using AutoMapper;
using P2PDelivery.Application.DTOs;
using P2PDelivery.Application.Interfaces;
using P2PDelivery.Application.Interfaces.Services;
using P2PDelivery.Application.Response;
using P2PDelivery.Domain.Entities;
using P2PDelivery.Domain.Enums;

namespace P2PDelivery.Application.Services
{
    public class DeliveryRequestService : IDeliveryRequestService
    {
        private readonly IRepository<DeliveryRequest> _requestRepository;
        private readonly IMapper _mapper;
        private readonly IApplicationService _applicationSevice;

        public DeliveryRequestService(IRepository<DeliveryRequest> requestRepository, IMapper mapper, IApplicationService applicationSevice)
        {
            _requestRepository = requestRepository;
            _mapper = mapper;
            _applicationSevice = applicationSevice;
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



        
        public async Task<RequestResponse<DeliveryRequestDetailsDTO>> GetDeliveryRequestDetailsAsync(int deliveryId, int userID)
        {
            // validation
            if (!await IsDeliveryRequestExist(deliveryId))
            {
                return RequestResponse<DeliveryRequestDetailsDTO>.Failure(ErrorCode.DeliveryRequestNotExist, "This Delivery Request is not found.");
            }

            var deliveryRequestDTO = _requestRepository.GetAll(x => x.Id == deliveryId)
                .Select(x => new DeliveryRequestDetailsDTO
                {
                    Description = x.Description,
                    DropOffLocation = x.DropOffLocation,
                    PickUpLocation = x.PickUpLocation,
                    PickUpDate = x.PickUpDate,
                    MaxPrice = x.MaxPrice,
                    MinPrice = x.MinPrice,
                    Status = x.Status.ToString(),
                    Title = x.Title,
                    TotalWeight = x.TotalWeight,
                    UserName=x.User.FullName,
                    UserId = x.UserId
                }).FirstOrDefault();

            if (deliveryRequestDTO.UserId == userID)
            {
                var response = await _applicationSevice.GetApplicationByRequestAsync(deliveryId);
                if (!response.IsSuccess)
                {
                    return RequestResponse<DeliveryRequestDetailsDTO>.Failure(response.ErrorCode, response.Message);
                }
                deliveryRequestDTO.ApplicationDTOs = response.Data;
            }

            return RequestResponse<DeliveryRequestDetailsDTO>.Success(deliveryRequestDTO);
        }

        public async Task<bool> IsDeliveryRequestExist(int deliveryId)
        {
            var isExist = await _requestRepository.IsExistAsync(deliveryId);
            if (!isExist)
            {
                return false;
            }
            return true;

        private readonly IMapper _mapper;

        public DeliveryRequestService(IRepository<DeliveryRequest> requestRepository,
            IMapper mapper)
        {
            _requestRepository = requestRepository;
            _mapper = mapper;
        }
        
        public async Task<RequestResponse<DeliveryRequest>> UpdateAsync(int id, DeliveryRequestUpdateDto deliveryRequestUpdateDto)
        {
            var deliveryRequest = await _requestRepository.GetByIDAsync(id);
            if (deliveryRequest == null)
                return RequestResponse<DeliveryRequest>.Failure(ErrorCode.DeliveryRequestNotExist,
                    "Delivery request not found");
            
            _mapper.Map(deliveryRequestUpdateDto, deliveryRequest);

            _requestRepository.SaveChangesAsync();
            
            return RequestResponse<DeliveryRequest>.Success(deliveryRequest, "Successfully updated delivery request");
        }

        public async Task<RequestResponse<bool>> DeleteAsync(int id)
        {
            var deliveryRequest = await _requestRepository.GetByIDAsync(id);
            if (deliveryRequest == null)
                return RequestResponse<bool>.Failure(ErrorCode.DeliveryRequestNotExist,
                    "Delivery request not found");
            
            _requestRepository.Delete(deliveryRequest);

            _requestRepository.SaveChangesAsync();

            return RequestResponse<bool>.Success(true, "Successfully deleted delivery request");
        }
    }
}
