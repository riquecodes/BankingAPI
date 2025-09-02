using UsersManagement.Models;

namespace UsersManagement.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserModel>> GetUsers();
        Task<UserModel> GetUserById(int id);
        Task AddUser(UserModel user);
        Task<UserModel?> UpdateUser(int id, UserModelDTO userDTO);
        Task DeleteUserById(int id);
    }
}
