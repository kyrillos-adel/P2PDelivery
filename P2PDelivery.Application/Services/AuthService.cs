using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNetCore.Identity;
using P2PDelivery.Application.DTOs;
using P2PDelivery.Application.Interfaces;
using P2PDelivery.Application.Interfaces.Services;
using P2PDelivery.Application.Response;
using P2PDelivery.Domain.Entities;

namespace P2PDelivery.Application.Services
{
    public class AuthService : IAuthService
    {
        UserManager<User> _userManager;
        public AuthService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public async Task<RequestResponse<RegisterDTO>> RegisterAsync(RegisterDTO registerDTO )
        {
            IdentityResult result = null;
            var usersinded = _userManager.FindByNameAsync(registerDTO.UserName);
            if (usersinded.Result == null)
            {
                var user = new User
                {
                    UserName = registerDTO.UserName,
                    FullName = registerDTO.FullName,
                    Email = registerDTO.Email,
                    Address = registerDTO.Address,
                    NatId = registerDTO.NatId,
                    PhoneNumber = registerDTO.Phone,
                    CreatedAt = DateTime.Now
                };

                 result = await _userManager.CreateAsync(user, registerDTO.Password);
                if (result.Succeeded)
                {
                  
                    return RequestResponse<RegisterDTO>.Success(registerDTO, "User registered successfully.");
                }
                else  
                { var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
                    return RequestResponse<RegisterDTO>.Failure(ErrorCode.IdentityError, errorMessage);
                }

            }
            else
            {
                return RequestResponse<RegisterDTO>.Failure(ErrorCode.EmailExist, "user is exist");


            }
        }

    }
}
