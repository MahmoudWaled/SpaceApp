using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Space.Application.DTOs.Users;
using Space.Application.Interfaces.Email;
using Space.Application.Interfaces.Repositories;
using Space.Application.Interfaces.Services;
using Space.Domain.Identity;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Space.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;
        private readonly IEmailService emailService;
        private readonly IFileService fileService;

        public UserService(IUserRepository userRepository, IConfiguration configuration, IMapper mapper, IEmailService emailService, IFileService fileService)
        {
            this.userRepository = userRepository;
            this.configuration = configuration;
            this.mapper = mapper;
            this.emailService = emailService;
            this.fileService = fileService;
        }
        public async Task<bool> DeleteUserAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new KeyNotFoundException("User not found to delete");
            var result = await userRepository.DeleteAsync(userId);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new ValidationException($"Failed to delete user: {errors}");
            }
            return result.Succeeded;
        }

        public async Task<UserDto> GetUserProfileAsync(string userId)
        {
            var user = await userRepository.FindByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User not found.");
            return mapper.Map<UserDto>(user);
        }

        public async Task<string> LoginAsync(LoginUserDto dto)
        {
            var user = await userRepository.FindByEmailAsync(dto.Email);
            if (user == null || !await userRepository.CheckPasswordAsync(user, dto.Password))
                throw new ValidationException("Invalid email or password.");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = configuration["Jwt:Issuer"],
                Audience = configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<UserDto> RegisterAsync(RegisterUserDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));
            var user = mapper.Map<ApplicationUser>(dto);

            if (dto.profileImagePath != null && dto.profileImagePath.Length > 0)
            {
                user.ProfileImagePath = await fileService.SaveFileAsync(dto.profileImagePath, "images");
            }
            else
            {
                user.ProfileImagePath = null;
            }

            var result = await userRepository.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new ArgumentException($"Failed to register user: {errors}");
            }
            return mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> UpdateUserProfileAsync(string userId, UpdateUserDto dto)
        {
            if (string.IsNullOrEmpty(userId))
                throw new NullReferenceException("User ID cannot be null or empty.");
            if (dto == null)
                throw new NullReferenceException("update user cannot be null.");
            var existingUser = await userRepository.FindByIdAsync(userId);
            if (existingUser == null)
                throw new KeyNotFoundException("User not found.");

            mapper.Map(dto, existingUser);

            if (dto.profileImagePath != null && dto.profileImagePath.Length > 0)
            {
                existingUser.ProfileImagePath = await fileService.SaveFileAsync(dto.profileImagePath, "images");
            }

            var result = await userRepository.UpdateAsync(existingUser);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new ArgumentException($"Failed to update user: {errors}");
            }
            return mapper.Map<UserDto>(existingUser);
        }

        public async Task RequestPasswordResetAsync(RequestResetPasswordDto dto)
        {
            var user = await userRepository.FindByEmailAsync(dto.Email);
            if (user == null)
                return;
            var token = await userRepository.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebUtility.UrlEncode(token);
            var resetLink = $"{configuration["App:FrontendUrl"]}/reset-password?userId={user.Id}&token={encodedToken}";
            var emailBody = $@"<h2>Reset Your Password</h2>
                              <p>Please click the link below to reset your password:</p>
                              <a href='{resetLink}'>Reset Password</a>
                              <p>If you did not request this, please ignore this email.</p>";
            await emailService.SendEmailAsync(dto.Email, "Password Reset Request", emailBody);
        }

        public async Task ResetPasswordAsync(ResetPasswordDto dto)
        {
            var user = await userRepository.FindByIdAsync(dto.UserId);
            if (user == null)
                throw new KeyNotFoundException("User not found.");
            var decodedToken = WebUtility.UrlDecode(dto.Token);
            var result = await userRepository.ResetPasswordAsync(user, decodedToken, dto.NewPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new ArgumentException($"failed to reset password: {errors}");
            }
        }
    }
}