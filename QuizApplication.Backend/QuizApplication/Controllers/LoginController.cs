using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using QuizApplication.Contracts.Entities;
using QuizApplication.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace QuizApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private AdminRepository _adminRepository;
        private IConfiguration _config;

        public LoginController(IConfiguration config, AdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody]Admin adminLogin)
        {
            IActionResult response = Unauthorized();
            var user = await AuthenticateUser(adminLogin);

            if (user != null)
            {
                var tokenString = GenerateJSONWebToken(user);
                response = Ok(new { token = tokenString });
            }

            return response;
        }

        [HttpGet]
        [Authorize]
        public ActionResult<string> GetEmailFromClaim()
        {
            var currentUser = HttpContext.User.Identity as ClaimsIdentity;

            IEnumerable<Claim> claims = currentUser.Claims;

            var email = claims
                .Where(x => x.Type == ClaimTypes.Email)
                .FirstOrDefault().Value;

            return email;
        }

        private string GenerateJSONWebToken(Admin adminLogin)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] 
            {
                new Claim(JwtRegisteredClaimNames.Email, adminLogin.Email),
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<Admin> AuthenticateUser(Admin adminLogin)
        {
            var user = await _adminRepository.GetByIdAsync(1);
            if (user.Password == adminLogin.Password)
            {
                return user;
            }

            return null;
        }
    }
}