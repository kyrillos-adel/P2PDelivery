using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P2PDelivery.Application.DTOs.ApplicationDTOs;
using P2PDelivery.Application.Interfaces.Services;
using P2PDelivery.Application.Response;
using P2PDelivery.Domain.Enums;
using System.Security.Claims;

namespace P2PDelivery.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class DRApplicationController : ControllerBase
    {
        private readonly IApplicationService _applicationService;
       

        public DRApplicationController(IApplicationService applicationService )
        {
            _applicationService = applicationService;
            
        }
        private int GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdClaim, out var userId) ? userId : 0;
        }

        [HttpGet("GetMyApplications")]
        public async Task<ActionResult<RequestResponse<ICollection<DRApplicationDTO>>>> GetMyApplicationsAsync()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("User ID not found in token.");
            }

            var result = await _applicationService.GetMyApplicationsAsync(userId);
            if (result.ErrorCode == ErrorCode.ApplicationNotExist)
            {
                return NotFound(result);
            }

            return Ok(result);
        }
        [HttpPut("update")]
        public async  Task<ActionResult<RequestResponse<string>>> UpdateApplication(int id ,UpdateApplicatioDTO updateApplicatioDTO)
        {

           
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("User ID not found in token.");
            }
            else
            {
                var respond = await _applicationService.UpdateApplication(id, updateApplicatioDTO);
                if (respond.IsSuccess)
                    return Ok(respond);
                return BadRequest(respond);
            }

        }
        [Authorize]
        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult<RequestResponse<bool>>> DeleteApplication( int id)
        {
            var userID = GetUserIdFromToken();
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("User ID not found in token.");
            }
            else
            {
                var respond = await _applicationService.DeleteApplicationAsync(id, userID);
                if (respond.IsSuccess)
                    return Ok(respond);
                return BadRequest(respond);
            }

        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<RequestResponse<bool>>> AddApplication(AddApplicationDTO addApplicationDTO)
        {
            var userID = GetUserIdFromToken();
            if (userID == 0)
            {
                return Unauthorized(RequestResponse<bool>.Failure(ErrorCode.Unauthorized, "You don't have the permission to add an application for another user."));
            }
            var response = await _applicationService.AddApplicationAsync(addApplicationDTO, userID);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<RequestResponse<ApplicationDTO>>> GetApplications(int requestID)
        {
            var userID = GetUserIdFromToken();

            var response = await _applicationService.GetApplicationByRequestAsync(requestID,userID);
            if(response.ErrorCode == ErrorCode.Unauthorized)
            {
                return Unauthorized(response);
            }

            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpPut("updatestatus")]
        public async Task <ActionResult<RequestResponse<bool>>> UpdateStatus (ApplicationStatusDTO request)
        {
            var userID = GetUserIdFromToken();
            
            var response = await _applicationService.UpdateApplicationStatuseAsync(request.deleveryRequestId, request.Id, request.Status,userID);
            if (response.ErrorCode == ErrorCode.Unauthorized)
            {
                return Unauthorized(response);
            }

            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
