using BankingAPI.Models;
using BankingAPI.Repositories;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace BankingAPI.Services
{
    public class AccountService : IAccountService
    {
        private readonly IConfiguration _configuration;
        private readonly IAccountRepository _accountRepository;
        private readonly IUserRepository _userRepository;
        public AccountService(IConfiguration configuration, IAccountRepository accountRepository, IUserRepository userRepository)
        {
            _configuration = configuration;
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
            await _accountRepository.CreateAccount(account);
            return account;
        }

        public async Task SetTransactionPin(int accountId, string transactionPin)
        {
            var account = await _accountRepository.GetAccountById(accountId);

            if (account is null)
            {
                throw new KeyNotFoundException("Account not found.");
            }

            var correctPin = Regex.IsMatch(transactionPin, @"^\d{4}$");

            if (!correctPin)
            {
                throw new ArgumentException("Pin must contain exactly 4 digits.");
            }

            if (account.AccountSecurity?.TransactionPinHash != null)
            {
                throw new InvalidOperationException("Transaction PIN is already set. Use 'Forgot my PIN' to update.");
            }

            CreateTransactionPinHash(transactionPin, out byte[] pinHash, out byte[] pinSalt);

            account.AccountSecurity = new AccountSecurityModel
            {
                TransactionPinHash = pinHash,
                TransactionPinSalt = pinSalt
            };
        }

        private void CreateTransactionPinHash(string pin, out byte[] pinHash, out byte[] pinSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                pinSalt = hmac.Key;
                pinHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(pin));
            }
        }
    }
}
