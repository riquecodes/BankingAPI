using BankingAPI.Controllers;
using BankingAPI.Models;
using BankingAPI.Repositories;
using BankingAPI.Utils;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace BankingAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;

        public AuthService(IUserRepository userRepository, IConfiguration configuration, ILogger<AuthController> logger, IAccountRepository accountRepository)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _logger = logger;
            _accountRepository = accountRepository;
        }

        public async Task<AuthResponseDTO> Login(LoginDTO loginDTO)
        {
            var user = await _userRepository.GetFullUserByCpf(loginDTO.Cpf);

            if (user is null)
            {
                _logger.LogWarning("Login failed for CPF {Cpf}: user not found", loginDTO.Cpf);
                throw new UnauthorizedAccessException("Invalid CPF or Password!");
            }
            
            if (!SecurityUtils.VerifyPassword(loginDTO.Password, user.PasswordHash, user.PasswordSalt))
            {
                _logger.LogWarning("Login failed for CPF {Cpf}: incorrect password", loginDTO.Cpf);
                throw new UnauthorizedAccessException("Invalid CPF or Password!");
            }

            if (!user.IsActive)
            {
                _logger.LogWarning("Login failed for CPF {Cpf}: user inactive", loginDTO.Cpf);
                throw new UnauthorizedAccessException("User is inactive!");
            }

            var token = SecurityUtils.GenerateJwtToken(user, _configuration);

            return new AuthResponseDTO
            {
                Token = token,
                Id = user.Id,
                Name = user.Name,
                Cpf = user.Cpf,
                Role = user.Role,
                TemporaryPassword = user.TemporaryPassword
            };
        }

        public async Task<UserResponseDTO> Register(RegisterDTO userRegister)
        {
            var existingUser = await _userRepository.GetUserByCpf(userRegister.Cpf);

            if (existingUser is not null)
            {
                _logger.LogWarning("Register failed for CPF {Cpf}:  CPF already exists", existingUser.Cpf);
                throw new ArgumentException("A user with this CPF already exists.");
            }

            ValidateRegisterDTO(userRegister);

            SecurityUtils.ValidatePasswordStrength(userRegister.Password);

            SecurityUtils.CreatePasswordHash(userRegister.Password, out byte[] hash, out byte[] salt);

            var newUser = new UserModel
            {
                Name = userRegister.Name,
                Cpf = userRegister.Cpf,
                Email = userRegister.Email,
                Celphone = userRegister.Celphone,
                PasswordHash = hash,
                PasswordSalt = salt
            };

            var createdUser = await _userRepository.CreateUser(newUser);

            var newAccount = new AccountModel
            {
                AccountNumber = GenerateAccountNumber(),
                Balance = 0,
                IsActive = true,
                CreatedAt = DateTime.Now,
                UserId = createdUser.Id
            };

            await _accountRepository.CreateAccount(newAccount);

            createdUser.Accounts = new List<AccountModel> { newAccount };

            return new UserResponseDTO
            {
                Id = createdUser.Id,
                Name = createdUser.Name,
                Cpf = createdUser.Cpf,
                Email = createdUser.Email,
                Celphone = createdUser.Celphone,
                Role = createdUser.Role,
                IsActive = createdUser.IsActive,
                CreatedAt = createdUser.CreatedAt
            };
        }

        public async Task ChangePassword(int userId, string currentPassword, string newPassword)
        {
            SecurityUtils.ValidatePasswordStrength(newPassword);

            var user = await _userRepository.GetFullUserById(userId);

            if (user is null)
            {
                _logger.LogWarning("Change Password attempt failed for ID {id}: user not found", userId);
                throw new KeyNotFoundException("User not found!");
            }

            if (!SecurityUtils.VerifyPassword(currentPassword, user.PasswordHash, user.PasswordSalt))
            {
                _logger.LogWarning("Change Password attempt failed for ID {id}: incorrect current password", userId);
                throw new UnauthorizedAccessException("Current password is incorrect!");
            }

            using (var hmac = new HMACSHA512())
            {
                user.PasswordSalt = hmac.Key;
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(newPassword));
            }

            user.TemporaryPassword = false;
            await _userRepository.UpdateUser(user.Id, user);
        }

        public async Task SetTransactionPin(int userId, string transactionPin)
        {
            var user = await _userRepository.GetFullUserById(userId);

            if (user is null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            var account = await _accountRepository.GetAccountById(user.Accounts.First().Id);

            if (account is null)
            {
                throw new KeyNotFoundException("Account not found.");
            }

            var correctPin = Regex.IsMatch(transactionPin, @"^\d{4}$");

            if (!correctPin)
            {
                throw new ArgumentException("Pin must contain exactly 4 digits.");
            }

            if (user.UserSecurity is not null)
            {
                throw new InvalidOperationException("Transaction PIN is already set. Use 'Forgot my PIN' to update.");
            }

            SecurityUtils.CreateTransactionPinHash(transactionPin, out byte[] pinHash, out byte[] pinSalt);

            user.UserSecurity = new UserSecurityModel
            {
                TransactionPinHash = pinHash,
                TransactionPinSalt = pinSalt
            };
        }

        private void ValidateRegisterDTO(RegisterDTO userRegister)
        {
            if (string.IsNullOrEmpty(userRegister.Name)
                || string.IsNullOrEmpty(userRegister.Cpf))
            {
                throw new ArgumentException("Name and CPF are required fields!");
            }
        }

        public string GenerateAccountNumber()
        { 
            var random = new Random();
            return random.Next(10000000, 99999999).ToString();
        }
    }
}
