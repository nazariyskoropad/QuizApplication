using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using QuizApplication.Contracts.DTOs;
using QuizApplication.Contracts.Entities;
using QuizApplication.Contracts.Interfaces;

namespace QuizApplication.BusinessLogic.Services
{
    public class AccountService
    {
        private readonly IRepository<Admin> _adminRepository;
        private readonly IConfiguration _config;

        public AccountService(IConfiguration config, IRepository<Admin> repository)
        {
            _config = config;
            _adminRepository = repository;
        }

        public async Task<AuthenticateResponseDto> Login(AdminLoginDto adminLogin)
        {
            var user = await AuthenticateUserAsync(adminLogin);

            if (user != null)
            {
                var jwtToken = GenerateJSONWebToken(user);
                var refreshToken = GenerateRefreshToken(user);
                user.RefreshTokens.Add(refreshToken);
                var updatedUser = await _adminRepository.Update(user);

                return new AuthenticateResponseDto(updatedUser, jwtToken, refreshToken.Token);
            }

            return null;
        }

        public async Task<AuthenticateResponseDto> RefreshTokenAsync(string token)
        {
            var admins = await _adminRepository.GetWithInclude<Admin>(
                        a => true,
                        t => t.Include(t => t.RefreshTokens)).ToListAsync();

            var admin = admins.FirstOrDefault(a => a.RefreshTokens.Any(t => t.Token == token));

            if (admin == null)
                return null;

            var refreshToken = admin.RefreshTokens.Single(x => x.Token == token);

            if (!refreshToken.IsActive)
                return null;

            var newRefreshToken = GenerateRefreshToken(admin);
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.ReplacedByToken = newRefreshToken.Token;
            admin.RefreshTokens.Add(newRefreshToken);

            var updatedAdmin = await _adminRepository.Update(admin);

            var jwtToken = GenerateJSONWebToken(updatedAdmin);

            return new AuthenticateResponseDto(updatedAdmin, jwtToken, newRefreshToken.Token);
        }

        public async Task<bool> RevokeTokenAsync(string token)
        {
            var admins = await _adminRepository.GetWithInclude<Admin>(
                                    a => true,
                                    t => t.Include(t => t.RefreshTokens)).ToListAsync();

            var admin = admins.FirstOrDefault(a => a.RefreshTokens.Any(t => t.Token == token));

            if (admin == null)
                return false;

            var refreshToken = admin.RefreshTokens.Single(x => x.Token == token);

            if (!refreshToken.IsActive)
                return false;

            refreshToken.Revoked = DateTime.UtcNow;

            await _adminRepository.UpdateAsync(admin);

            return true;
        }

        private RefreshToken GenerateRefreshToken(Admin admin)
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return new RefreshToken
                {
                    AdminId = admin.Id,
                    Token = Convert.ToBase64String(randomBytes),
                    Expires = DateTime.UtcNow.AddDays(7),
                    Created = DateTime.UtcNow,
                };
            }
        }

        private string GenerateJSONWebToken(Admin admin)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Email, admin.Email),
            };

            var token = new JwtSecurityToken(
              _config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<Admin> AuthenticateUserAsync(AdminLoginDto adminLogin)
        {
            var user = await _adminRepository
                .GetAsync(a => a.Email == adminLogin.Email && a.Password == adminLogin.Password);

            if (user != null)
            {
                return user;
            }

            return null;
        }
    }
}
