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
        private readonly IDeliveryRequestService deliveryRequestService;

        public DeliveryRequestController(IDeliveryRequestService deliveryRequestService)
        {
            this.deliveryRequestService = deliveryRequestService;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DeliveryRequestUpdateDto deliveryRequestUpdateDto)
        {
            var deliveryRequest = await deliveryRequestService.UpdateAsync(id, deliveryRequestUpdateDto);
            if (deliveryRequest.ErrorCode == ErrorCode.DeliveryRequestNotExist)
                return NotFound();
            
            return Ok(deliveryRequest.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deliveryRequest = await deliveryRequestService.DeleteAsync(id);
            if (deliveryRequest.ErrorCode == ErrorCode.DeliveryRequestNotExist)
                return NotFound();

            return Ok(deliveryRequest.Data);
        }
    }
}
