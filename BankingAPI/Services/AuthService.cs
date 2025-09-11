using System.Security.Cryptography;
using System.Text;
using BankingAPI.Models;
using BankingAPI.Repositories;

namespace BankingAPI.Services
{
    public class AuthService(UserRepository userRepository) : IAuthService
    {
        private readonly UserRepository _userRepository = userRepository;

        public async Task<UserModel> Login(LoginDTO loginDTO)
        {
            var user = await _userRepository.GetUserByCPF(loginDTO.Cpf);

            if (user is null)
            {
                throw new UnauthorizedAccessException("Invalid CPF or Password!");
            }
            
            if (!VerifyPassword(loginDTO.Password, user.PasswordHash, user.PasswordSalt))
            {
                throw new UnauthorizedAccessException("Invalid CPF or Password!");
            }

            return user;
        }

        private bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
        {
            using (var hmac = new HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(storedHash);
            }
        }
    }
}
