using Microsoft.AspNetCore.Mvc;
using UsersManagement.Models;

namespace UsersManagement.Repositories
{
    public interface IUsersRepository
    {
        Task<IEnumerable<UserModel>> GetUsers();
        Task<UserModel> GetUserById(int id);
        Task AddUser(UserModel user);
        Task<UserModel?> UpdateUser(UserModel user);
        Task DeleteUserById(int id);
    }
}
