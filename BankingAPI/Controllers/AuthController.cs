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
            var registerResponse = await _authService.Register(userRegister);
            return Ok(registerResponse);
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordDTO changePasswordDTO)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            await _authService.ChangePassword(userId, changePasswordDTO.CurrentPassword, changePasswordDTO.NewPassword);
            return Ok(new { message = "Password updated successfully!" });
        }

        [Authorize]
        [HttpPost("{userId}/set-transaction-pin")]
        public async Task<ActionResult> SetTransactionPin(int userId, [FromBody] TransactionPinDTO transactionPin)
        {
            var authenticatedUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            if (userId != authenticatedUserId)
            {
                return Forbid();
            }

            await _authService.SetTransactionPin(userId, transactionPin.TransactionPin);
            return Ok(new { message = "Transaction PIN set successfully!" });
        }

        [Authorize]
        [HttpPost("{userId}/change-transaction-pin")]
        public async Task<ActionResult> ChangeTransactionPin(int userId, [FromBody] ChangeTransactionPinDTO changeTransactionPinDTO)
        {
            var authencticatedUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            if (userId != authencticatedUserId)
            {
                return Forbid();
            }

            await _authService.ChangeTransactionPin(userId, changeTransactionPinDTO.CurrentPin, changeTransactionPinDTO.NewPin);
            return Ok(new { message = "Transaction PIN changed successfully!" });
        }
    }
}
