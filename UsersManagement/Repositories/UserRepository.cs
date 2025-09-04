using Microsoft.EntityFrameworkCore;
using UsersManagement.Context;
using UsersManagement.Models;

namespace UsersManagement.Repositories
{
    public class UserRepository : IUserRepository
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

        public async Task<UserModel?> GetUserById(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<UserModelDTO> CreateUser(UserModel user)
        {
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();

            return new UserModelDTO
            {
                Name = user.Name,
                Cpf = user.Cpf,
                Celphone = user.Celphone,
                Email = user.Email
            };
        }

        public async Task<UserModelDTO?> UpdateUser(int id, UserModelDTO userDTO)
        {
            _context.UsersDTO.Update(userDTO);
            await _context.SaveChangesAsync();
            return userDTO;
        }

        public async Task<bool> DeleteUserById(int id)
        { 
            var userToDelete = await GetUserById(id);
            
            if (userToDelete is null)
            {
                return false;
            }

            _context.Users.Remove(userToDelete);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
