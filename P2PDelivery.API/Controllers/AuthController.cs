using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P2PDelivery.Application.DTOs;
using P2PDelivery.Application.Interfaces.Services;
using P2PDelivery.Application.Response;
using System.Security.Claims;
namespace P2PDelivery.API.Controllers
{
    [Route("api/user")]
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



        [Authorize]
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
        [HttpDelete("delete")]
        public async Task<ActionResult<RequestResponse<string>>> DeleteAccount()
        {
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            var respond = await _authService.DeleteUser(userName);

            if (respond.IsSuccess)
                return Ok(respond);

            return BadRequest(respond);
        }
        [Authorize]
        [HttpPut("update")]
        public async Task<ActionResult<RequestResponse<string>>> UpdateUser([FromForm] UserProfile userProfile)
        {
            var UserName = User.FindFirstValue(ClaimTypes.Name);
            var response = await _authService.EditUserInfo(UserName, userProfile);

            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response);
        }
        [Authorize]
        [HttpGet("profile")]
        public async Task<ActionResult<UserProfile>> GetUserProfile()
        {
            var userName = User.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrEmpty(userName))
                return Unauthorized("invalid user");

            var profile = await _authService.GetUserProfile(userName);
            if (profile == null)
                return NotFound(profile);

            return Ok(profile);
        }
      
        [HttpPut("Recover")]
        public async Task<ActionResult<RequestResponse<string>>> RecoverAccount([FromQuery] string user)
        {
            if (string.IsNullOrEmpty(user))
                return NotFound("user not found ");
            else
            {
              var respond = await _authService.RecoverMyAccount(user);
                if (respond.IsSuccess)
                {
                    return Ok(respond);
                }
                return BadRequest(respond);
            }
        }


        [HttpPost("refresh-token")]
        public async Task<ActionResult<RequestResponse<string>>> RefreshToken([FromBody] string refreshToken)
        {
            var result = await _authService.RefreshTokenAsync(refreshToken);
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

    }
}
