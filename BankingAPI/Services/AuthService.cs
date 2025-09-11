using BankingAPI.Models;
using BankingAPI.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BankingAPI.Services
{
    public class AuthService(UserRepository userRepository, IConfiguration configuration) : IAuthService
    {
        private readonly UserRepository _userRepository = userRepository;
        private readonly IConfiguration _configuration = configuration;

        public async Task<AuthResponseDTO> Login(LoginDTO loginDTO)
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

            if (!user.IsActive)
            { 
                throw new UnauthorizedAccessException("User is inactive!");
            }

            var token = GenerateJwtToken(user);

            return new AuthResponseDTO
            {
                Token = token,
                Id = user.Id,
                Name = user.Name,
                Cpf = user.Cpf,
                Role = user.Role
            };
        }

        private string GenerateJwtToken(UserModel user)
        {
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:ExpireMinutes"]!)),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
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
