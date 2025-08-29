using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices.Marshalling;
using UsersManagement.Context;
using UsersManagement.Models;
using UsersManagement.Repositories;

namespace UsersManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepository _user;
        public UsersController(IUsersRepository user)
        {
            _user = user;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserModel>>> GetUsers()
        {
            var users = await _user.GetUsers();

            if (!users.Any())
            {
                return NotFound("Nenhum usuário cadastrado!");
            }

            return Ok(users.OrderBy(users => users.Name));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserModel>> GetUserById(int id)
        {
            var user = await _user.GetUserById(id);

            if (user is null) {
                return NotFound($"Usuário com o id {id} não encontrado!");
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<UserModel>> AddUser([FromBody] UserModel user)
        {
            await _user.AddUser(user);

            return CreatedAtAction(
                nameof(GetUserById),
                new { id = user.Id },
                user);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserModel>> UpdateUser(int id, [FromBody] UserModelDTO user)
        {
            var newUser = await _user.UpdateUser(id, user);

            if (newUser is null)
            {
                return NotFound();
            }

            return Ok(newUser);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        { 
            var userToDelete = await _user.GetUserById(id);

            if (userToDelete is null)
            {
                return NotFound();
            }

            await _user.DeleteUserById(id);
            return NoContent();
        }
        
    }
}
