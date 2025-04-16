using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using P2PDelivery.Application.DTOs;
using P2PDelivery.Application.Interfaces.Services;
using P2PDelivery.Application.Response;
using P2PDelivery.Domain.Entities;
using System.Threading.Tasks;
using IdentityResult = Microsoft.AspNetCore.Identity.IdentityResult;

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

        [HttpPost("Register")]
        public async Task<RequestResponse<RegisterDTO>> Register(RegisterDTO registerDTO)
        {

            return await _authService.RegisterAsync(registerDTO);
        }
        [HttpGet("DeleteAccount")]
        public async Task<RequestResponse<string>> Delete(RegisterDTO registerDTO)
        {
          return await _authService.DeleteAccount(registerDTO.UserName);     
        }
        [HttpGet("findbyname")]
        public async Task<RequestResponse<string>> FindByName(string Name)
        {
            return await _authService.GetByName(Name);
        }

    }
}
