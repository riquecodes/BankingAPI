using UsersManagement.Models;

namespace UsersManagement.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserModel>> GetUsers();
        Task<UserModel?> GetUserById(int id);
        Task<UserModel> CreateUser(UserModelDTO userDTO);
        Task<UserModelDTO?> UpdateUser(int id, UserModelDTO userDTO);
        Task<bool> DeleteUserById(int id);
    }
}
