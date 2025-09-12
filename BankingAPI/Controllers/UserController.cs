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

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserModel>>> GetUsers()
        {
            var users = await _userService.GetUsers();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserModel>> GetUserById(int id)
        {
            var user = await _userService.GetUserById(id);

            return Ok(user);
        }

        //[Authorize(Roles = "admin")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<UserModel>> CreateUser([FromBody] UserModelDTO userDTO)
        {
            var newUser = await _userService.CreateUser(userDTO);

            return CreatedAtAction(
                nameof(GetUserById),
                new { id = newUser.Id },
                newUser);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<UserModel>> UpdateUser(int id, [FromBody] UserModelDTO userDTO)
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