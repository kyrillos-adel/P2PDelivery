using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json.Linq;
using P2PDelivery.Application.DTOs;
using P2PDelivery.Application.Interfaces;
using P2PDelivery.Application.Interfaces.Services;
using P2PDelivery.Application.Response;
using P2PDelivery.Domain.Entities;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
                    return RequestResponse<RegisterDTO>.Failure(ErrorCode.UnexpectedError, errorMessage);
                }

            }
            else
            {
                return RequestResponse<RegisterDTO>.Failure(ErrorCode.Userexist, "user is exist");


            }
        }

        public async Task<RequestResponse<string>> DeleteAccount(string UserName)
        {
            var user = await _userManager.FindByNameAsync(UserName);

            if (user == null)
            {
                return RequestResponse<string>.Failure(ErrorCode.UserNotExist, "User does not exist.");
            }

            user.IsDeleted = true;
            user.DeletedAt = DateTime.Now;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return RequestResponse<string>.Success("User accoun deleted.");
            }
            else
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return RequestResponse<string>.Failure(ErrorCode.IdentityError, "Failed to delete user: " + errors);
            }
        }

        public async Task<RequestResponse<string>> GetByName(string username)
        {
            var founded = await _userManager.FindByNameAsync(username);
            if (founded == null)
                return RequestResponse<string>.Failure(ErrorCode.Userexist, "user not exist: ");
           
            else
            {

                return RequestResponse<string>.Success(founded.FullName ," exist.");
            }

        }
    }
}
