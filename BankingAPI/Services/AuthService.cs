using BankingAPI.Models;
using BankingAPI.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BankingAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<AuthResponseDTO> Login(LoginDTO loginDTO)
        {
            var user = await _userRepository.GetFullUserByCpf(loginDTO.Cpf);

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
                Role = user.Role,
                TemporaryPassword = user.TemporaryPassword
            };
        }

        public async Task<UserResponseDTO> Register(RegisterDTO userRegister)
        {
            var userExists = await _userRepository.GetUserByCpf(userRegister.Cpf);

            if (userExists is not null)
            {
                throw new ArgumentException("A user with this CPF already exists.");
            }

            ValidateRegisterDTO(userRegister);

            ValidatePasswordStrength(userRegister.Password);

            CreatePasswordHash(userRegister.Password, out byte[] hash, out byte[] salt);

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
            using var hmac = new HMACSHA512(storedSalt);
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(storedHash);
            }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private void ValidateRegisterDTO(RegisterDTO userRegister)
        {
            if (string.IsNullOrEmpty(userRegister.Name)
                || string.IsNullOrEmpty(userRegister.Cpf))
            {
                throw new ArgumentException("Name and CPF are required fields!");
            }
        }

        private void ValidatePasswordStrength(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty.");

            if (password.Length < 8)
                throw new ArgumentException("Password must be at least 8 characters long.");

            if (!password.Any(char.IsUpper))
                throw new ArgumentException("Password must contain at least one uppercase letter.");

            if (!password.Any(char.IsLower))
                throw new ArgumentException("Password must contain at least one lowercase letter.");

            if (!password.Any(char.IsDigit))
                throw new ArgumentException("Password must contain at least one number.");

            if (!password.Any(ch => "!@#$%^&*()_+-=[]{}|;:,.<>?".Contains(ch)))
                throw new ArgumentException("Password must contain at least one special character.");
        }
    }
}
