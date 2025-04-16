using P2PDelivery.Application.DTOs.DeliveryRequestDTOs;
using P2PDelivery.Application.Interfaces;
using P2PDelivery.Application.Interfaces.Services;
using P2PDelivery.Application.Response;
using P2PDelivery.Domain.Entities;

namespace P2PDelivery.Application.Services
{
    public class DeliveryRequestService : IDeliveryRequestService
    {
        private readonly IRepository<DeliveryRequest> _requestRepository;
        private readonly IApplicationService _applicationSevice;

        public DeliveryRequestService(IRepository<DeliveryRequest> requestRepository, IApplicationService applicationSevice)
        {
            _requestRepository = requestRepository;
            _applicationSevice = applicationSevice;
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
        }


    }
}
