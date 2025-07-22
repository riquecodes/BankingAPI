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
            return Ok(users);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<UserModel>> GetUserById(int id)
        {
            var user = await _user.GetUserById(id);
            return user is null ? NotFound() : Ok(user);
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
    }
}
