using BankingAPI.Models;
using BankingAPI.Repositories;

namespace BankingAPI.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUserRepository _userRepository;

        public AccountService(IAccountRepository accountRepository, IUserRepository userRepository)
        {
            _accountRepository = accountRepository;
            _userRepository = userRepository;
        }

        public async Task<AccountModel?> GetAccountById(int id)
        {
            var account = await _accountRepository.GetAccountById(id);
            if (account is null)
            {
                throw new ArgumentException("Account not found.");
            }

            return account;
        }

        public async Task<IEnumerable<AccountModel>> GetAccountsByUserId(int userId)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user is null)
            {
                throw new ArgumentException("User not found.");
            }
            
            var account = await _accountRepository.GetAccountsByUserId(userId);
            if (!account.Any())
            {
                return Enumerable.Empty<AccountModel>();
            }

            return account;
        }

        public async Task<decimal> GetBalanceById(int id)
        {
            var balance = await _accountRepository.GetBalanceById(id);
            return balance;
        }

        public async Task<AccountModel> CreateAccount(AccountModel account)
        {
            var user = await _userRepository.GetFullUserById(account.UserId);
            if (user is null)
            { 
                throw new ArgumentException("User not found.");
            }

            var existingAccount = await _accountRepository.GetAccountsByUserId(account.UserId);
            if (existingAccount.Any(a => a.AccountType == account.AccountType))
            { 
                throw new ArgumentException("User already has an account of this type.");
            }

            var newAccount = new AccountModel
            {
                UserId = account.UserId,
                AccountNumber = GenerateAccountNumber(),
                AccountType = account.AccountType,
                User = user
            };


            return account;
        }

        public string GenerateAccountNumber()
        {
            var random = new Random();
            return random.Next(10000000, 99999999).ToString();
        }
    }
}
