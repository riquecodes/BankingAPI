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
                return NotFound("Nenhum usu�rio cadastrado!");
            }

            return Ok(users.OrderBy(users => users.Name));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserModel>> GetUserById(int id)
        {
            var user = await _user.GetUserById(id);

            if (user is null) {
                return NotFound($"Usu�rio com o id {id} n�o encontrado!");
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<UserModel>> AddUser(UserModel user)
        {
            await _user.AddUser(user);

            return CreatedAtAction(
                nameof(GetUserById),
                new { id = user.Id },
                user);
        }

        [HttpPatch]
        public async Task<ActionResult<UserModel>> UpdateUser(UserModel user)
        {
            var newUser = await _user.UpdateUser(user);

            if (newUser is null)
            {
                return NotFound();
            }

            return Ok(newUser);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        { 
            var userDeleted = await _user.GetUserById(id);

            if (userDeleted is null)
            {
                return NotFound();
            }

            await _user.DeleteUserById(id);
            return Ok("Deletado com Sucesso!");
        }
        
    }
}
