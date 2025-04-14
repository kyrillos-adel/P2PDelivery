using Microsoft.AspNetCore.Mvc;
using P2PDelivery.Application.Interfaces.Services;

namespace P2PDelivery.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryRequestController : ControllerBase
    {
        private readonly IDeliveryRequestService _deliveryRequestService;

        public DeliveryRequestController(IDeliveryRequestService deliveryRequestService)
        {
            _deliveryRequestService = deliveryRequestService;
        }



    }
}
