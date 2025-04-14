using P2PDelivery.Application.Interfaces;
using P2PDelivery.Application.Interfaces.Services;
using P2PDelivery.Domain.Entities;

namespace P2PDelivery.Application.Services
{
    public class DeliveryRequestService:IDeliveryRequestService
    {
        private readonly IRepository<DeliveryRequest> _requestRepository;

        public DeliveryRequestService(IRepository<DeliveryRequest> requestRepository)
        {
            _requestRepository = requestRepository;
        }


    }
}
