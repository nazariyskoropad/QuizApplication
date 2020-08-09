using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApplication.BusinessLogic.Services;
using QuizApplication.Contracts.DTOs;

namespace QuizApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly AccountService _accountService;

        public LoginController(AccountService accountService)
        {
            _accountService = accountService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody]AdminLoginDto adminLogin)
        {
            var tokenString = await _accountService.Login(adminLogin);

            if (tokenString != null)
            {
                return Ok(new { token = tokenString });
            }

            return Unauthorized();
        }
    }
}