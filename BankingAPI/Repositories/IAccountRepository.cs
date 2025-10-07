using BankingAPI.Models;
using System.Threading.Tasks;

namespace BankingAPI.Repositories
{
    public interface IAccountRepository
    {
        Task<AccountModel?> GetAccountById(int id);
        Task<IEnumerable<AccountModel>> GetAccountsByUserId(int userId);
        Task<decimal> GetBalanceById(int id);
        Task<AccountModel> CreateAccount(AccountModel account);
    }
}
