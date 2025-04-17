using AutoMapper;
using P2PDelivery.Application.DTOs;
using P2PDelivery.Application.Interfaces;
using P2PDelivery.Application.Interfaces.Services;
using P2PDelivery.Application.Response;
using P2PDelivery.Domain.Entities;

namespace P2PDelivery.Application.Services
{
    public class DeliveryRequestService : IDeliveryRequestService
    {
        private readonly IRepository<DeliveryRequest> _requestRepository;
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
