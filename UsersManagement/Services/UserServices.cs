using Microsoft.EntityFrameworkCore;
using UsersManagement.Context;
using UsersManagement.Models;
using UsersManagement.Repositories;

namespace UsersManagement.Services
{
    public class UserServices(IUserRepository userRepository, AppDbContext dbContext)
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly AppDbContext _context = dbContext;

        public Task<IEnumerable<UserModel>> GetUsers()
        {
            return _userRepository.GetUsers();
        }

        public Task<UserModel> GetUserById(int id)
        {
            return _userRepository.GetUserById(id);
        }

        public async Task AddUser(UserModel user)
        {
            await _userRepository.AddUser(user);
        }

        public async Task<UserModel?> UpdateUser(int id, UserModelDTO userDTO)
        {

            var userToUpdate = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (userToUpdate is null)
            {
                return null;
            }

            userToUpdate.Name = userDTO.Name;
            userToUpdate.Cpf = userDTO.Cpf;
            userToUpdate.Celphone = userDTO.Celphone;
            userToUpdate.Email = userDTO.Email;

            if (string.IsNullOrEmpty(userDTO.Name)
                || string.IsNullOrEmpty(userDTO.Cpf))
            {
                throw new Exception("Nome e CPF são campos obrigatórios!");
            }

            return await _userRepository.UpdateUser(id, userDTO);
        }
    }
}
