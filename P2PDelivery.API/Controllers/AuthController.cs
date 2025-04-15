using Azure;
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
        public async Task<RequestResponse<LoginResponseDTO>> Login([FromBody] LoginDTO loginDto)
        {
            if (!ModelState.IsValid)
                return RequestResponse<LoginResponseDTO>.Failure(ErrorCode.ValidationError, "Invalid data provided.");

            var response = await _authService.LoginAsync(loginDto);

            if (response.IsSuccess)
            {
                return RequestResponse<LoginResponseDTO>.Success(response.Data, "User logged in successfully.");
            }
            else
            {
                return RequestResponse<LoginResponseDTO>.Failure(response.ErrorCode, response.Message);
            }
        }

        [HttpPost("Register")]
        public async Task<RequestResponse<RegisterDTO>> Register(RegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
            {
                string modelErrors = string.Join("; ", ModelState.Values
                                            .SelectMany(v => v.Errors)
                                            .Select(e => e.ErrorMessage));

                return RequestResponse<RegisterDTO>.Failure(ErrorCode.ValidationError, modelErrors);
            }
            else
            {
             var respond=   await _authService.RegisterAsync(registerDTO);
                if (respond.IsSuccess)
                {

                    return RequestResponse<RegisterDTO>.Success(respond.Data, "User registered successfully.");
                }
                else
                {
                    return RequestResponse<RegisterDTO>.Failure(respond.ErrorCode, respond.Message);
                }
            }
        }



        [Authorize]
        [HttpDelete("delete-account")]
        public async Task<IActionResult> DeleteAccount()
        {
            var userName =  User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(userName))
                return Unauthorized("User not authenticated.");

            var result = await _authService.DeleteUserNameIdAsync(userName);

            if (result.IsSuccess)
                return Ok(result.Message);

            return BadRequest(result.Message);
        }
    }
}
