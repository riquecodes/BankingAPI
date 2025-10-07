using Microsoft.AspNetCore.Mvc;
using BankingAPI.Models;
using BankingAPI.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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
        public async Task<ActionResult<AuthResponseDTO>> Login([FromBody] LoginDTO loginDTO)
        {
            var authResponse = await _authService.Login(loginDTO);

            return Ok(authResponse);
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserResponseDTO>> Register([FromBody] RegisterDTO userRegister)
        {
            var createdUser = await _authService.Register(userRegister);
            return Ok(new { message = "User registered successfully!" });
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordDTO changePasswordDTO)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            await _authService.ChangePassword(userId, changePasswordDTO.CurrentPassword, changePasswordDTO.NewPassword);
            return Ok(new { message = "Password updated successfully!" });
        }
    }
}
