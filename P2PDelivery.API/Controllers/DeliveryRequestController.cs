using Microsoft.AspNetCore.Mvc;
using P2PDelivery.Application.DTOs;
using P2PDelivery.Application.Interfaces.Services;
using P2PDelivery.Application.Response;

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

        [HttpPut("{id}")]
        public async Task<ActionResult<RequestResponse<DeliveryRequestUpdateDto>>> Update(int id, [FromBody] DeliveryRequestUpdateDto deliveryRequestUpdateDto)
        {
            var requestResponse = await _deliveryRequestService.UpdateAsync(id, deliveryRequestUpdateDto);
            if (requestResponse.ErrorCode == ErrorCode.DeliveryRequestNotExist)
                return NotFound(requestResponse);
            
            return Ok(requestResponse);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<RequestResponse<bool>>> Delete(int id)
        {
            var requestResponse = await _deliveryRequestService.DeleteAsync(id);
            if (requestResponse.ErrorCode == ErrorCode.DeliveryRequestNotExist)
                return NotFound(requestResponse);

            return Ok(requestResponse);
        }
    }
}
