using Microsoft.AspNetCore.Mvc;
using P2PDelivery.Application.DTOs.DeliveryRequestDTOs;
using P2PDelivery.Application.Interfaces.Services;
using P2PDelivery.Application.DTOs;
using P2PDelivery.Application.Response;

namespace P2PDelivery.API.Controllers;


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
