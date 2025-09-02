using Microsoft.EntityFrameworkCore;
using UsersManagement.Context;
using UsersManagement.Models;

namespace UsersManagement.Repositories
{
    public class IUserRepository : IUsersRepository
    {
        private readonly AppDbContext _context;

        public IUserRepository(AppDbContext context)
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

        public async Task<UserModelDTO?> UpdateUser(int id, UserModelDTO userDTO)
        {
            _context.UsersDTO.Update(userDTO);
            await _context.SaveChangesAsync();
            return userDTO;
        }

        public async Task DeleteUserById(int id)
        { 
            var userToDelete = await GetUserById(id);

            _context.Users.Remove(userToDelete);
            await _context.SaveChangesAsync();
        }
    }
}
