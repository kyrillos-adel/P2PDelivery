using Microsoft.AspNetCore.Mvc;
using P2PDelivery.Application.DTOs;
using P2PDelivery.Application.Interfaces.Services;

namespace P2PDelivery.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var response = await _authService.LoginAsync(loginDto);        
                    return Ok(response);
                }
                catch (UnauthorizedAccessException ex)
                {
                    return Unauthorized(new { message = ex.Message });
                }
            }

            return BadRequest(ModelState);
        }
    }
}
