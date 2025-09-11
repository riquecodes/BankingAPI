using BankingAPI.Models;

namespace BankingAPI.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserModel>> GetUsers();
        Task<UserModel?> GetUserById(int id);
        Task<UserModel?> GetUserByCPF(string cpf);
        Task<UserModel> CreateUser(UserModel user);
        Task<UserModel?> UpdateUser(int id, UserModel user);
        Task<bool> DeleteUserById(int id);
    }
}
