using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
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

        public async Task<UserModel?> UpdateUser(UserModel user)
        {
            var userToUpdate = await GetUserById(user.Id);

            if (userToUpdate is null)
            {
                return null;
            }

            userToUpdate.Name = user.Name;
            userToUpdate.Celphone = user.Celphone;
            userToUpdate.Email = user.Email;

            _context.Users.Update(userToUpdate);
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
