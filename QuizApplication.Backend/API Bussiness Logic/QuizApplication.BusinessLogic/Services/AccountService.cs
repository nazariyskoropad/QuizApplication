using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
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

        public async Task<string> Login(AdminLoginDto adminLogin)
        {
            var user = await AuthenticateUserAsync(adminLogin);

            if (user != null)
            {
                var tokenString = GenerateJSONWebToken(user);
                return tokenString;
            }

            return null;
        }

        private string GenerateJSONWebToken(Admin adminLogin)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Email, adminLogin.Email),
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
