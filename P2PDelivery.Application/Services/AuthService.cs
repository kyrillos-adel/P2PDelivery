using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        
        LoginResponseDTO _respond;
        public LoginResponseDTO respond => _respond;
         


        public AuthService(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager, IJwtTokenGenerator jwtTokenGenerator)
        {
            _userManager = userManager;
            _jwtTokenGenerator = jwtTokenGenerator;
            _roleManager = roleManager;
        }



        public async Task<RequestResponse<RegisterDTO>> RegisterAsync(RegisterDTO registerDTO)
        {
            var existingUser = await _userManager.FindByNameAsync(registerDTO.UserName);
            if (existingUser != null)
            {
                return RequestResponse<RegisterDTO>.Failure(ErrorCode.Userexist, "User already exists.");
            }

            string? imagePath = null;
            if (registerDTO.ProfileImage != null)
            {
                imagePath = await SaveProfileImageAsync(registerDTO.ProfileImage);
            }

            var user = new User
            {
                UserName = registerDTO.UserName,
                FullName = registerDTO.FullName,
                Email = registerDTO.Email,
                Address = registerDTO.Address,
                NatId = registerDTO.NatId,
                PhoneNumber = registerDTO.Phone,
                CreatedAt = DateTime.Now,
                ProfileImageUrl = imagePath
            };

            var result = await _userManager.CreateAsync(user, registerDTO.Password);
            if (result.Succeeded)
            {
                return RequestResponse<RegisterDTO>.Success(registerDTO, "User registered successfully.");
            }

            var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
            return RequestResponse<RegisterDTO>.Failure(ErrorCode.UnexpectedError, errorMessage);
        }

        public async Task<RequestResponse<UserProfile>> GetByName(string username)
        {
            var founded = await _userManager.FindByNameAsync(username);
            if (founded == null)
                return RequestResponse<UserProfile>.Failure(ErrorCode.UserNotFound, "user not exist: ");
           
            else
            {
                var user = new UserProfile
                {
                    UserName = founded.UserName,
                    FullName = founded.FullName ,
                    Address = founded.Address,
                    Phone =founded.PhoneNumber,
                    Email = founded.Email,
                    ProfileImageUrl=founded.ProfileImageUrl
                };
                return RequestResponse<UserProfile>.Success(user, " exist.");
            }
        }

        public async Task<RequestResponse<LoginResponseDTO>> LoginAsync(LoginDTO loginDto)
        {
            bool isEmail = new EmailAddressAttribute().IsValid(loginDto.Identifier);
            var user = isEmail
                ? await _userManager.FindByEmailAsync(loginDto.Identifier)
                : await _userManager.FindByNameAsync(loginDto.Identifier);

            if (user == null)
                return RequestResponse<LoginResponseDTO>.Failure(ErrorCode.UserNotFound, "Wrong email or user-name");

            if (user.IsDeleted)
                return RequestResponse<LoginResponseDTO>.Failure(ErrorCode.UserDeleted, "Account has been deleted.");

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!isPasswordValid)
                return RequestResponse<LoginResponseDTO>.Failure(ErrorCode.IncorrectPassword, "Wrong password");

            // Generate JWT token
            var token = await _jwtTokenGenerator.GenerateToken(user);

            // Generate refresh token
            var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();

            // Get user roles
            var roles = await _userManager.GetRolesAsync(user);

            // Save the RefreshToken into user data (optional but highly recommended)
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7); // Adjust the expiry as needed
            await _userManager.UpdateAsync(user);

            // Create the LoginResponseDTO
            var response = new LoginResponseDTO
            {
                Token = token,
                Expiration = DateTime.Now.AddHours(1),
                UserName = user.UserName,
                Email = user.Email,
                Role = roles.ToList(),
                RefreshToken = refreshToken,
                RefreshTokenExpiration = DateTime.Now.AddDays(7), // Adjust the expiry as needed
                ProfileImageUrl = user.ProfileImageUrl  // Add the profile image URL here
            };

            return RequestResponse<LoginResponseDTO>.Success(response, "Login successful.");
        }



        public async Task<RequestResponse<string>> DeleteUser(string UserName)
        {
            var user = await _userManager.FindByNameAsync(UserName);
            if (user == null)
                return RequestResponse<string>.Failure(ErrorCode.UserNotFound, "User not found.");

            user.IsDeleted = true;
            user.DeletedAt = DateTime.Now;
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
                return RequestResponse<string>.Success("User soft deleted successfully.");

            return RequestResponse<string>.Failure(ErrorCode.DeleteFailed, "Failed to soft delete user.");
        }


        public async Task<RequestResponse<string>> EditUserInfo(string UserName,  UserProfile userProfile)
        {
            var user = await _userManager.FindByNameAsync(UserName);
            if (user == null || user.IsDeleted)
                return RequestResponse<string>.Failure(ErrorCode.UserNotFound, "User not found");

            if (!string.IsNullOrWhiteSpace(userProfile.Email) && userProfile.Email != user.Email)
            {
                var emailExists = await _userManager.FindByEmailAsync(userProfile.Email);
                if (emailExists != null && emailExists.UserName != user.UserName)
                    return RequestResponse<string>.Failure(ErrorCode.EmailExist, "Email is already taken.");

                user.Email = userProfile.Email;
            }

            if (!string.IsNullOrWhiteSpace(userProfile.UserName) && userProfile.UserName != user.UserName)
            {
                var userNameExists = await _userManager.FindByNameAsync(userProfile.UserName);
                if (userNameExists != null && userNameExists.UserName != user.UserName)
                    return RequestResponse<string>.Failure(ErrorCode.Userexist, "Username is already taken.");

                user.UserName = userProfile.UserName;
            }

            if (!string.IsNullOrWhiteSpace(userProfile.FullName))
                user.FullName = userProfile.FullName;

            if (!string.IsNullOrWhiteSpace(userProfile.Phone))
                user.PhoneNumber = userProfile.Phone;

            if (!string.IsNullOrWhiteSpace(userProfile.Address))
                user.Address = userProfile.Address;

            user.UpdatedAt = DateTime.Now;

            // Handle Profile Image upload
            if (userProfile.ProfileImage != null)
            {
                var imagePath = await SaveProfileImageAsync(userProfile.ProfileImage);
                user.ProfileImageUrl = imagePath;
            }

            var updatedByUser = await _userManager.FindByNameAsync(userProfile.UserName);
            if (updatedByUser != null)
            {
                user.UpdatedBy = updatedByUser.Id;
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return RequestResponse<string>.Failure(ErrorCode.UpdateFailed, $"Update failed: {errors}");
            }

            return RequestResponse<string>.Success("Profile updated successfully.");
        }


        public async Task<UserProfile> GetUserProfile(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null || user.IsDeleted)
                return null;

            return new UserProfile
            {
                UserName = user.UserName,
                FullName = user.FullName,
                Email = user.Email,
                Address = user.Address,
                NatId = user.NatId,
                Phone = user.PhoneNumber,
               ProfileImageUrl=user.ProfileImageUrl
            };
        }

        public async Task<RequestResponse<string>> RecoverMyAccount(string username)
        {
            var user =await _userManager.FindByNameAsync(username);
            if (user == null)
                return RequestResponse<string>.Failure(ErrorCode.UserNotExist, "user do not exist");
            else if ((DateTime.Now.Date - user.DeletedAt.Value.Date).TotalDays > 30)
                return RequestResponse<string>.Failure(ErrorCode.CanNotRecover, "Sorry You con not Recover this Account Please Try to Register ");
            else
            {
                user.IsDeleted = false;
                user.DeletedAt = null;
                user.DeletedBy = null;
                var resspond =  await _userManager.UpdateAsync(user);
                return RequestResponse<string>.Success("Recover Successful");

            }
        }


        public async Task<RequestResponse<LoginResponseDTO>> RefreshTokenAsync(string refreshToken)
        {
            // Find the user who owns this refresh token
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

            if (user == null || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return RequestResponse<LoginResponseDTO>.Failure(ErrorCode.InvalidRefreshToken, "Invalid or expired refresh token.");
            }

            var newToken = await _jwtTokenGenerator.GenerateToken(user);
            var newRefreshToken = _jwtTokenGenerator.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            await _userManager.UpdateAsync(user);

            var roles = await _userManager.GetRolesAsync(user);

            var response = new LoginResponseDTO
            {
                Token = newToken,
                Expiration = DateTime.Now.AddHours(1),
                UserName = user.UserName,
                Email = user.Email,
                Role = roles.ToList(),
                RefreshToken = newRefreshToken,
                RefreshTokenExpiration = DateTime.Now.AddDays(7)
            };

            return RequestResponse<LoginResponseDTO>.Success(response, "Token refreshed successfully.");
        }
        private async Task<string> SaveProfileImageAsync(IFormFile image)
        {
            // Validate image size (max 2MB)
            if (image.Length > 2 * 1024 * 1024)
            {
                throw new Exception("File size exceeds the limit of 2MB.");
            }

            // Validate file type (only allow image formats)
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(image.FileName).ToLower();

            if (!allowedExtensions.Contains(fileExtension))
            {
                throw new Exception("Invalid file type. Only images are allowed.");
            }

            var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "users");
            Directory.CreateDirectory(folder);

            var fileName = Guid.NewGuid().ToString() + fileExtension;
            var filePath = Path.Combine(folder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return $"/images/users/{fileName}";
        }







    }

}
