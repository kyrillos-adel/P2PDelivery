using Microsoft.AspNetCore.Mvc;
using P2PDelivery.Application.Interfaces.Services;
using P2PDelivery.Application.DTOs;
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
        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> CreateDeliveryRequest([FromBody]CreateDeliveryRequestDTO dto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _deliveryRequestService.CreateDeliveryRequestAsync(dto);

            if ( !result.IsSuccess)
            {
                return BadRequest("Failed to create delivery request");
            }
            return CreatedAtAction(nameof(GetDeliveryRequestById), new { id = result.Data.Id }, result.Data);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDeliveryRequestById(int id)
        {
            var result = await _deliveryRequestService.GetDeliveryRequestByIdAsync(id);
            if (!result.IsSuccess)
            {
                return NotFound(result.Message);
            }
            return Ok(result.Data);
        }

        [HttpGet("user/{userId}")]

        public async Task<IActionResult> GetDeliveryRequestsByUserId(int userId)
        {
            var result = await _deliveryRequestService.GetDeliveryRequestsByUserIdAsync(userId);
            if (!result.IsSuccess)
            {
                return NotFound(result.Message);
            }
            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeliveryRequest(int id)
        {
            var result = await _deliveryRequestService.DeleteDeliveryRequestAsync(id);
            if (!result.IsSuccess)
            {
                return NotFound(result.Message);
            }
            return Content("Delivery Request Deleted Succussefully");
        }



    }
}
