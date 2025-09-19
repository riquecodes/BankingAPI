using Microsoft.AspNetCore.Mvc;
using BankingAPI.Models;
using BankingAPI.Services;
using Microsoft.AspNetCore.Authorization;

namespace BankingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var authResponse = await _authService.Login(loginDTO);

            return Ok(authResponse);
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterDTO userRegister)
        {
            var createdUser = await _authService.Register(userRegister);
            return Ok(new { message = "User registered successfully!" });
        }

        {
            return Ok("Register endpoint");
        }
    }
}
