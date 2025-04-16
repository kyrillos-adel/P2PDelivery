using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using P2PDelivery.Application.DTOs.DeliveryRequestDTOs;
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

        [HttpGet("details/{deliveryID}")]
        public async Task<ActionResult<DeliveryRequestDetailsDTO>> GetRequestDetails(int deliveryID)
        {
            //var userID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var userID = 4;

            var response = await _deliveryRequestService.GetDeliveryRequestDetailsAsync(deliveryID,userID);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return NotFound(response);
        }



    }
}
