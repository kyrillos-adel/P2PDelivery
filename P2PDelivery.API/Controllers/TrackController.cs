using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P2PDelivery.Application.DTOs.Track;
using P2PDelivery.Application.Interfaces.Services;
using P2PDelivery.Application.Response;
using TechTalk.SpecFlow.CommonModels;

namespace P2PDelivery.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackController : ControllerBase
    {
        private readonly ITrackingService _trackingService;
        public TrackController(ITrackingService trackingService)
        {
            _trackingService = trackingService;
        }

        private int GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdClaim, out var userId) ? userId : 0;
        }

        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<RequestResponse<TrackDTO>> Get(int Id)
        {
            int userId = GetUserIdFromToken();
            if (userId == 0) 
            {
                return Unauthorized(RequestResponse<TrackDTO>.Failure(ErrorCode.Unauthorized, "unauthorized user"));
            }
            var result =  _trackingService.GetLastTrack(userId, Id);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<RequestResponse<bool>>> Add(AddTrackDTO track)
        {
            int userId = GetUserIdFromToken();
            if (userId == 0)
            {
                return Unauthorized(RequestResponse<TrackDTO>.Failure(ErrorCode.Unauthorized, "unauthorized user"));
            }
            else
            {
                track.UserId = userId;
            }
            var result = await _trackingService.AddNewStatus(track);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
