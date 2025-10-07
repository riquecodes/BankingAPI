using BankingAPI.Models;

namespace BankingAPI.Services
{
    public interface IAccountService
    {
        Task SetTransactionPassword(int accountId, string transactionPassword);
    }
}
