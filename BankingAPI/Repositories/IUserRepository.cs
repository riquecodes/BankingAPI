using BankingAPI.Models;

namespace BankingAPI.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserResponseDTO>> GetUsers();
        Task<UserResponseDTO?> GetUserById(int id);
        Task<UserResponseDTO?> GetUserByCpf(string cpf);
        Task<UserModel?> GetFullUserById(int id);
        Task<UserModel?> GetFullUserByCpf(string cpf);
        Task<UserModel> CreateUser(UserModel user);
        Task<UserModel?> UpdateUser(int id, UserModel user);
        Task<UserSecurityModel> CreateUserSecurity(UserSecurityModel userSecurity);
        Task<UserSecurityModel?> UpdateUserSecurity(int userId, UserSecurityModel userSecurity);
        Task<bool> DeleteUserById(int id);
    }
}
