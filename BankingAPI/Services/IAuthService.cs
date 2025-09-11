using BankingAPI.Models;

namespace BankingAPI.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> Login(LoginDTO loginDTO);
    }
}
