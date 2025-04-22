using Azure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using P2PDelivery.Application.DTOs;
using P2PDelivery.Application.Interfaces.Services;
using P2PDelivery.Application.Response;
using System.Security.Claims;
namespace P2PDelivery.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<RequestResponse<LoginResponseDTO>>> Login([FromBody] LoginDTO loginDto)
        {
            var respond = await _authService.LoginAsync(loginDto);
            if (respond.IsSuccess)
                return Ok(respond);
            return BadRequest(respond);
        }

        [HttpPost("Register")]
        public async Task<ActionResult<RequestResponse<RegisterDTO>>> Register(RegisterDTO registerDTO)
        {

            var respond = await _authService.RegisterAsync(registerDTO);
            if (respond.IsSuccess)
                return Ok(respond);
            return BadRequest(respond);
        }



        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("findbyname")]
        public async Task<ActionResult<RequestResponse<RegisterDTO>>> FindByName(string Name)
        {
            if (string.IsNullOrWhiteSpace(Name))
                return BadRequest("Name parameter is required.");
            var respond = await _authService.GetByName(Name);
            if (respond.IsSuccess)
                return Ok(respond);
            return BadRequest(respond);

        }

        [Authorize]
        [HttpDelete("delete-account")]
        public async Task<ActionResult<RequestResponse<string>>> DeleteAccount()
        {
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            var respond = await _authService.DeleteUserNameIdAsync(userName);

            if (respond.IsSuccess)
                return Ok(respond);

            return BadRequest(respond);
        }

        [Authorize]
        [HttpPut("update-profile")]
        public async Task<ActionResult<RequestResponse<string>>> UpdateUser([FromBody]UserProfile userProfile)
        {
            var UserName = User.FindFirstValue(ClaimTypes.Name);
            var response = await _authService.EditUserInfo(UserName, userProfile);

            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response);
        }

        [Authorize]
        [HttpGet("get-user-profile")]
        public async Task<ActionResult<RequestResponse<UserProfile>>> GetUserProfile()
        {
            var userName = User.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrEmpty(userName))
                return Unauthorized("invalid user");

            var profile = await _authService.GetUserProfile(userName);
            if (profile == null)
                return NotFound(profile);

            return Ok(profile);
        }

    }
}
