using Microsoft.AspNetCore.Mvc;
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
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task AddUser(UserModel user)
        {
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();
        }
    }
}
