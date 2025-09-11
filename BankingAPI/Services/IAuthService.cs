using BankingAPI.Models;

namespace BankingAPI.Services
{
    public interface IAuthService
    {
        Task<UserModel> Login(LoginDTO loginDTO);
    }
}
