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
        private readonly IRepository<DeliveryRequest> requestRepository;
        private readonly IMapper mapper;

        public DeliveryRequestService(IRepository<DeliveryRequest> requestRepository,
            IMapper mapper)
        {
            this.requestRepository = requestRepository;
            this.mapper = mapper;
        }
        
        public async Task<RequestResponse<DeliveryRequest>> UpdateAsync(int id, DeliveryRequestDto deliveryRequestDto)
        {
            var deliveryRequest = await requestRepository.GetByIDAsync(id);
            if (deliveryRequest == null)
                return RequestResponse<DeliveryRequest>.Failure(ErrorCode.DeliveryRequestNotExist,
                    "Delivery request not found");
            
            mapper.Map(deliveryRequestDto, deliveryRequest);

            requestRepository.SaveChangesAsync();
            
            return RequestResponse<DeliveryRequest>.Success(deliveryRequest, "Successfully updated delivery request");
        }

        public async Task<RequestResponse<DeliveryRequest>> DeleteAsync(int id)
        {
            var deliveryRequest = await requestRepository.GetByIDAsync(id);
            if (deliveryRequest == null)
                return RequestResponse<DeliveryRequest>.Failure(ErrorCode.DeliveryRequestNotExist,
                    "Delivery request not found");
            
            requestRepository.Delete(deliveryRequest);

            requestRepository.SaveChangesAsync();

            return RequestResponse<DeliveryRequest>.Success(deliveryRequest, "Successfully deleted delivery request");
        }
    }
}
