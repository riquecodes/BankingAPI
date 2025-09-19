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

        public async Task<IEnumerable<UserResponseDTO>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return users.Select(u => new UserResponseDTO
            {
                Id = u.Id,
                Name = u.Name,
                Cpf = u.Cpf,
                Celphone = u.Celphone,
                Email = u.Email,
                Role = u.Role,
                IsActive = u.IsActive,
                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt
            });
        }

        public async Task<UserResponseDTO?> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user is null)
            {
                return null;
            }

            return new UserResponseDTO
            {
                Id = user.Id,
                Name = user.Name,
                Cpf = user.Cpf,
                Celphone = user.Celphone,
                Email = user.Email,
                Role = user.Role,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };
        }

        public async Task<UserResponseDTO?> GetUserByCpf(string cpf)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Cpf == cpf);

            if (user is null)
            {
                return null;
            }

            return new UserResponseDTO
            {
                Id = user.Id,
                Name = user.Name,
                Cpf = user.Cpf,
                Celphone = user.Celphone,
                Email = user.Email,
                Role = user.Role,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };
        }

        public async Task<UserModel?> GetFullUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user is null)
            {
                return null;
            }

            return user;
        }

        public async Task<UserModel?> GetFullUserByCpf(string cpf)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Cpf == cpf);

            if (user is null)
            {
                return null;
            }

            return user;
        }

        public async Task<UserModel> CreateUser(UserModel user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<UserModel?> UpdateUser(int id, UserModel user)
        {
            var updatedUser = await GetFullUserById(id);

            if (updatedUser is null)
            {
                return null;
            }

            updatedUser.Name = user.Name;
            updatedUser.Celphone = user.Celphone;
            updatedUser.Email = user.Email;
            updatedUser.Role = user.Role;
            updatedUser.IsActive = user.IsActive;
            updatedUser.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return updatedUser;
        }

        public async Task<bool> DeleteUserById(int id)
        { 
            var userToDelete = await GetFullUserById(id);
            
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
