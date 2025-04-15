using Microsoft.AspNetCore.Identity;
using P2PDelivery.Application.DTOs;
using P2PDelivery.Application.Interfaces.Services;
using P2PDelivery.Application.Response;
using P2PDelivery.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace P2PDelivery.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        public AuthService(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager, IJwtTokenGenerator jwtTokenGenerator)
        {
            _userManager = userManager;
            _jwtTokenGenerator = jwtTokenGenerator;
            _roleManager = roleManager;
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

        
        public async Task<RequestResponse<LoginResponseDTO>> LoginAsync(LoginDTO loginDto)
        {
            bool isEmail = new EmailAddressAttribute().IsValid(loginDto.Identifier);
            var user = isEmail
                ? await _userManager.FindByEmailAsync(loginDto.Identifier)
                : await _userManager.FindByNameAsync(loginDto.Identifier);

            if (user == null)
                return RequestResponse<LoginResponseDTO>.Failure(ErrorCode.UserNotFound, "User not found.");

            if (user.IsDeleted)
                return RequestResponse<LoginResponseDTO>.Failure(ErrorCode.UserDeleted, "Account has been deleted.");

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!isPasswordValid)
                return RequestResponse<LoginResponseDTO>.Failure(ErrorCode.UserNotFound, "Wrong password");
            // Generate JWT token
            var token = await _jwtTokenGenerator.GenerateToken(user);
            var roles = await _userManager.GetRolesAsync(user);

            var loginResponse = new LoginResponseDTO
            {
                Token = token,
                Expiration = DateTime.Now.AddHours(1),
                UserName = user.UserName,
                Email = user.Email,
                Role = roles.ToList()
            };

            return RequestResponse<LoginResponseDTO>.Success(loginResponse, "Login successful.");
        }


        public async Task<RequestResponse<string>> DeleteUserByIdAsync(string UserName)
        {
            var user = await _userManager.FindByNameAsync(UserName);
            if (user == null)
                return RequestResponse<string>.Failure(ErrorCode.UserNotFound, "User not found.");

            user.IsDeleted = true;
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
                return RequestResponse<string>.Success("User soft deleted successfully.");

            return RequestResponse<string>.Failure(ErrorCode.DeleteFailed, "Failed to soft delete user.");
        }


    }
    
}
