using BankingAPI.Models;

namespace BankingAPI.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> Login(LoginDTO loginDTO);
        Task<UserResponseDTO> Register(RegisterDTO userRegister);
        Task ChangePassword(int userId, string currentPassword, string newPassword);
        Task SetTransactionPin(int userId, string pin);
    }
}