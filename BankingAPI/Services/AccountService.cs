using BankingAPI.Models;
using BankingAPI.Repositories;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

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
            var user = await _userRepository.GetUserById(account.UserId);
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
            };


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

        public string GenerateAccountNumber()
        {
            var random = new Random();
            return random.Next(10000000, 99999999).ToString();
        }
    }
}
