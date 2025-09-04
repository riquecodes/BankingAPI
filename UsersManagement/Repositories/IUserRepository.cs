using UsersManagement.Models;

namespace UsersManagement.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserModel>> GetUsers();
        Task<UserModel?> GetUserById(int id);
        Task<UserModel> CreateUser(UserModel user);
        Task<UserModelDTO?> UpdateUser(int id, UserModelDTO user);
        Task<bool> DeleteUserById(int id);
    }
}
