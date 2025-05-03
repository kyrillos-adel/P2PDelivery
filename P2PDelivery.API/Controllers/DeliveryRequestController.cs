using Microsoft.AspNetCore.Mvc;
using P2PDelivery.Application.DTOs.DeliveryRequestDTOs;
using P2PDelivery.Application.Interfaces.Services;
using P2PDelivery.Application.DTOs;
using P2PDelivery.Application.Response;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using P2PDelivery.Domain.Helpers;
using P2PDelivery.Domain;

namespace P2PDelivery.API.Controllers;


[Route("api/deliveryrequest")]
[ApiController]
public class DeliveryRequestController : ControllerBase
{
    private readonly IDeliveryRequestService _deliveryRequestService;

    public DeliveryRequestController(IDeliveryRequestService deliveryRequestService)
    {
        _deliveryRequestService = deliveryRequestService;
    }


    private int GetUserIdFromToken()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(userIdClaim, out var userId) ? userId : 0;
    }
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<RequestResponse<DeliveryRequestDTO>>> CreateDeliveryRequest([FromForm] CreateDeliveryRequestDTO dto)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors)
                                          .Select(e => e.ErrorMessage)
                                          .ToList();
            string errorMessage = string.Join("; ", errors);
            var response = RequestResponse<DeliveryRequestDTO>.Failure(ErrorCode.ValidationError, errorMessage);
            return BadRequest(response);
        }

        dto.UserId = GetUserIdFromToken(); //  Use here

        var requestResponse = await _deliveryRequestService.CreateDeliveryRequestAsync(dto);

        if (requestResponse.ErrorCode == ErrorCode.DeliveryRequestAlreadyExist)
            return Conflict(requestResponse);

        if (!requestResponse.IsSuccess)
            return BadRequest(requestResponse);

        return CreatedAtAction(nameof(GetDeliveryRequestById), new { id = requestResponse.Data.Id }, requestResponse);
    }
    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<RequestResponse<DeliveryRequestDTO>>> GetDeliveryRequestById(int id)
    {
        var result = await _deliveryRequestService.GetDeliveryRequestByIdAsync(id);

        if (result.ErrorCode == ErrorCode.DeliveryRequestNotExist)
        {
            return NotFound(result);
        }

        return Ok(result);
    }

    [Authorize]
    [HttpGet("my")]
    public async Task<ActionResult<RequestResponse<List<DeliveryRequestDTO>>>> GetMyDeliveryRequests()
    {
        int userId = GetUserIdFromToken(); // Extract userId from the token

        var result = await _deliveryRequestService.GetDeliveryRequestsByUserIdAsync(userId);

        if (result.ErrorCode == ErrorCode.UserNotFound || result.ErrorCode == ErrorCode.DeliveryRequestNotExist)
        {
            return NotFound(result);
        }

        return Ok(result);
    }


  

    [HttpGet]

    public async Task<ActionResult<RequestResponse<PageList<DeliveryRequestDTO>>>> GetAllDeliveryRequests([FromQuery] DeliveryRequestParams deliveryRequestParams, int pageNumber)
    {
        int userId = GetUserIdFromToken();
        deliveryRequestParams.PageNumber = pageNumber;
        var result = await _deliveryRequestService.GetAllDeliveryRequestsAsync(deliveryRequestParams, userId);

        if (!result.IsSuccess)
        {
            return NotFound(result);
        }

        return Ok(result);
    }




    [HttpGet("details/{deliveryID}")]
    public async Task<ActionResult<RequestResponse<DeliveryRequestDetailsDTO>>> GetRequestDetails(int deliveryID)
    {
        var userID = GetUserIdFromToken();

        var response = await _deliveryRequestService.GetDeliveryRequestDetailsAsync(deliveryID,userID);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return NotFound(response);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult<RequestResponse<DeliveryRequestUpdateDto>>> Update(int id, [FromBody] DeliveryRequestUpdateDto deliveryRequestUpdateDto)
    {
        var requestResponse = await _deliveryRequestService.UpdateAsync(id, deliveryRequestUpdateDto);
        if (requestResponse.ErrorCode == ErrorCode.DeliveryRequestNotExist)
            return NotFound(requestResponse);
        
        return Ok(requestResponse);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult<RequestResponse<bool>>> Delete(int id)
    {
        var requestResponse = await _deliveryRequestService.DeleteAsync(id);
        if (requestResponse.ErrorCode == ErrorCode.DeliveryRequestNotExist)
            return NotFound(requestResponse);

        return Ok(requestResponse);
    }
}
