using Microsoft.AspNetCore.Identity;﻿
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
        public async Task<RequestResponse<RegisterDTO>> GetByName(string username)
        {
            var founded = await _userManager.FindByNameAsync(username);
            if (founded == null)
                return RequestResponse<RegisterDTO>.Failure(ErrorCode.UserNotFound, "user not exist: ");
           
            else
            {
                var user = new RegisterDTO
                {
                    UserName = founded.UserName,
                    FullName = founded.FullName ,
                    Address = founded.Address,
                    Phone =founded.PhoneNumber
                };
                return RequestResponse<RegisterDTO>.Success(user, " exist.");
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
            var roles = await _userManager.GetRolesAsync(user);

            _respond = new LoginResponseDTO
            {
                Token = token,
                Expiration = DateTime.Now.AddHours(1),
                UserName = user.UserName,
                Email = user.Email,
                Role = roles.ToList()
            };

            return RequestResponse<LoginResponseDTO>.Success(_respond, "Login successful.");
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


        public async Task<RequestResponse<string>> EditUserInfo(string UserName, UserProfile userProfile)
        {
            var user = await _userManager.FindByNameAsync(UserName);

            if (user == null || user.IsDeleted)
                return RequestResponse<string>.Failure(ErrorCode.UserNotFound, "user not found");

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

            if (!string.IsNullOrWhiteSpace(userProfile.Email))
                user.Email = userProfile.Email;

            if (!string.IsNullOrWhiteSpace(userProfile.Phone))
                user.PhoneNumber = userProfile.Phone;

            if (!string.IsNullOrWhiteSpace(userProfile.Address))
                user.Address = userProfile.Address;

            user.UpdatedAt = DateTime.Now;
            var editableUser = await _userManager.FindByNameAsync(userProfile.UserName);
            if (editableUser != null)
            {
                user.UpdatedBy = editableUser.Id;
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
                Phone = user.PhoneNumber
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
    }
    
}
