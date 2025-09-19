using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BankingAPI.Models;
using BankingAPI.Services;

namespace BankingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponseDTO>>> GetUsers()
        {
            var users = await _userService.GetUsers();

            return Ok(users);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("id/{id}")]
        public async Task<ActionResult<UserResponseDTO>> GetUserById(int id)
        {
            var user = await _userService.GetUserById(id);

            return Ok(user);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("cpf/{cpf}")]
        public async Task<ActionResult<UserResponseDTO>> GetUserByCpf(string cpf)
        {
            var user = await _userService.GetUserByCpf(cpf);

            return Ok(user);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult<UserResponseDTO>> CreateUser([FromBody] RegisterDTO userRegister)
        {
            var newUser = await _userService.CreateUser(userRegister);

            return CreatedAtAction(
                nameof(GetUserById),
                new { id = newUser.Id },
                newUser);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<UserResponseDTO>> UpdateUser(int id, [FromBody] UserModelDTO userDTO)
        {
            var updatedUser = await _userService.UpdateUser(id, userDTO);

            return Ok(updatedUser);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        { 
            await _userService.DeleteUserById(id);

            return NoContent();
        }
    }
}