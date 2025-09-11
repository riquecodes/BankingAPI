using BankingAPI.Models;

namespace BankingAPI.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserModel>> GetUsers();
        Task<UserModel?> GetUserById(int id);
        Task<UserModel> CreateUser(UserModelDTO userDTO);
        Task<UserModel?> UpdateUser(int id, UserModelDTO userDTO);
        Task<bool> DeleteUserById(int id);
    }
}
