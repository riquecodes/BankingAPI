using BankingAPI.Models;

namespace BankingAPI.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserModel>> GetUsers();
        Task<UserResponseDTO?> GetUserById(int id);
        Task<UserResponseDTO?> GetUserByCpf(string cpf);
        Task<UserModel?> GetFullUserById(int id);
        Task<UserModel?> GetFullUserByCpf(string cpf);
        Task<UserModel> CreateUser(UserModel user);
        Task<UserModel?> UpdateUser(int id, UserModel user);
        Task<bool> DeleteUserById(int id);
    }
}
