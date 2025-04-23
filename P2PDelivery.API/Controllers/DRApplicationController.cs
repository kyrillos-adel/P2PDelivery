using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P2PDelivery.Application.DTOs.ApplicationDTOs;
using P2PDelivery.Application.Interfaces.Services;
using P2PDelivery.Application.Response;
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


    }
}
