using Microsoft.EntityFrameworkCore;
using UsersManagement.Context;
using UsersManagement.Models;

namespace UsersManagement.Repositories
{
    public class UserRepository : IUsersRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserModel>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<UserModel> GetUserById(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user is null)
            {                 
                throw new Exception($"Usuário com o id {id} não encontrado!");
            }

            return user;
        }

        public async Task AddUser(UserModel user)
        {
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();
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


            if (string.IsNullOrEmpty(userToUpdate.Name))
            {
                throw new Exception("O nome não pode ser vazio.");
            }

            if (string.IsNullOrEmpty(userToUpdate.Cpf))
            {
                throw new Exception("O CPF não pode ser vazio.");
            }

            await _context.SaveChangesAsync();
            return userToUpdate;
        }

        public async Task DeleteUserById(int id)
        { 
            var userToDelete = await GetUserById(id);

            _context.Users.Remove(userToDelete);
            await _context.SaveChangesAsync();
        }
    }
}
