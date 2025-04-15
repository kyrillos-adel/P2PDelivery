using Microsoft.AspNetCore.Identity;
using P2PDelivery.Application.DTOs;
using P2PDelivery.Application.Interfaces;
using P2PDelivery.Application.Interfaces.Services;
using P2PDelivery.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace P2PDelivery.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        public AuthService(UserManager<User> userManager,RoleManager<IdentityRole<int>> roleManager, IJwtTokenGenerator jwtTokenGenerator)
        {
            _userManager = userManager;
            _jwtTokenGenerator = jwtTokenGenerator;
            _roleManager = roleManager;
        }
        public async Task<LoginResponseDTO> LoginAsync(LoginDTO loginDto)
        {
            bool isEmail = new EmailAddressAttribute().IsValid(loginDto.Identifier);
            var user = isEmail
                ? await _userManager.FindByEmailAsync(loginDto.Identifier)
                : await _userManager.FindByNameAsync(loginDto.Identifier);

            if (user == null)
                throw new UnauthorizedAccessException("User not found.");

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!isPasswordValid)
                throw new UnauthorizedAccessException("Invalid password.");

            // Generate JWT token
            var token = await _jwtTokenGenerator.GenerateToken(user);
            var roles = await _userManager.GetRolesAsync(user);

            return new LoginResponseDTO
            {
                Token = token,
                Expiration = DateTime.Now.AddHours(1),
                UserName = user.UserName,
                Email = user.Email,
                Role = roles.ToList()
            };
        }
    }
    
}
