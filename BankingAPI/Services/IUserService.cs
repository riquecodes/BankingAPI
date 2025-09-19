using BankingAPI.Models;

namespace BankingAPI.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponseDTO>> GetUsers();
        Task<UserResponseDTO?> GetUserById(int id);
        Task<UserResponseDTO?> GetUserByCpf(string cpf);
        Task<UserResponseDTO> CreateUser(RegisterDTO userRegister);
        Task<UserResponseDTO?> UpdateUser(int id, UserModelDTO userDTO);
        Task<bool> DeleteUserById(int id);
    }
}
