using Microsoft.AspNetCore.Mvc;
using UsersManagement.Models;
using UsersManagement.Services;

namespace UsersManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserModel>>> GetUsers()
        {
            var users = await _userService.GetUsers();

            if (!users.Any())
            {
                return NotFound(new { message = "No users found!" });
            }

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserModel>> GetUserById(int id)
        {
            var user = await _userService.GetUserById(id);

            if (user is null) {
                return NotFound(new { message = $"User with id {id} not found!" });
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<UserModel>> CreateUser([FromBody] UserModelDTO userDTO)
        {
            var newUser = await _userService.CreateUser(userDTO);

            return CreatedAtAction(
                nameof(GetUserById),
                new { id = newUser.Id },
                newUser);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserModel>> UpdateUser(int id, [FromBody] UserModelDTO user)
        {
            var newUser = await _userService.UpdateUser(id, user);

            if (newUser is null)
            {
                return NotFound();
            }

            return Ok(newUser);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        { 
            var userToDelete = await _userService.GetUserById(id);

            if (userToDelete is null)
            {
                return NotFound();
            }

            await _userService.DeleteUserById(id);
            return NoContent();
        }
        
    }
}
