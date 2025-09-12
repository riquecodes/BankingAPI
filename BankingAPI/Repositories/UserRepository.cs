using Microsoft.EntityFrameworkCore;
using BankingAPI.Context;
using BankingAPI.Models;

namespace BankingAPI.Repositories
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

        public async Task<UserModel?> GetUserByCPF(string cpf)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Cpf == cpf);
        }

        public async Task<UserModel> CreateUser(UserModel user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<UserModel?> UpdateUser(int id, UserModel user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
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
