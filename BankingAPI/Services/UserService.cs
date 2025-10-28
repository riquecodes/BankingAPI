using BankingAPI.Models;
using BankingAPI.Repositories;
using BankingAPI.Utils;

namespace BankingAPI.Services
{
    public class UserService(IUserRepository userRepository, IAccountRepository accountRepository) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IAccountRepository _accountRepository = accountRepository;

        public async Task<IEnumerable<UserResponseDTO>> GetUsers()
        {
            var users = await _userRepository.GetUsers();

            if (!users.Any())
            { 
                throw new KeyNotFoundException("No users found!");
            }

            return users.OrderBy(u => u.Name);
        }

        public async Task<UserResponseDTO?> GetUserById(int id)
        {
            var user = await _userRepository.GetUserById(id);

            if (user is null) 
            {
                throw new KeyNotFoundException($"User with id {id} not found!");
            }

            return user;
        }

        public async Task<UserResponseDTO?> GetUserByCpf(string cpf)
        {
            var user = await _userRepository.GetUserByCpf(cpf);

            if (user is null)
            {
                throw new KeyNotFoundException($"User with CPF {cpf} not found!");
            }

            return user;
        }

        public async Task<UserModel?> GetFullUserById(int id)
        {
            var user = await _userRepository.GetFullUserById(id);

            if (user is null)
            {
                throw new KeyNotFoundException($"User with id {id} not found!");
            }

            return user;
        }

        public async Task<UserResponseDTO> CreateUser(RegisterDTO userRegister)
        {
            await ValidateRegisterDTO(userRegister);

            SecurityUtils.CreatePasswordHash(userRegister.Password, out byte[] hash, out byte[] salt);

            var newUser = new UserModel
            {
                Name = userRegister.Name,
                Cpf = userRegister.Cpf,
                Celphone = userRegister.Celphone,
                Email = userRegister.Email,
                PasswordHash = hash,
                PasswordSalt = salt,
                Role = userRegister.Role,
                TemporaryPassword = true,
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
        
        public async Task<UserResponseDTO?> UpdateUser(int id, UserModelDTO userDTO)
        {

            var userToUpdate = await _userRepository.GetFullUserById(id);

            if (userToUpdate is null)
            {
                throw new KeyNotFoundException($"User with id {id} not found!");
            }

            userToUpdate.Name = userDTO.Name;
            userToUpdate.Cpf = userDTO.Cpf;
            userToUpdate.Celphone = userDTO.Celphone;
            userToUpdate.Email = userDTO.Email;
            userToUpdate.Role = userDTO.Role;
            userToUpdate.IsActive = userDTO.IsActive;
            userToUpdate.UpdatedAt = DateTime.Now;

            await _userRepository.UpdateUser(id, userToUpdate);

            var updatedResponse = new UserResponseDTO
            {
                Id = id,
                Name = userToUpdate.Name,
                Cpf = userToUpdate.Cpf,
                Celphone = userToUpdate.Celphone,
                Email = userToUpdate.Email,
                Role = userToUpdate.Role,
                IsActive = userToUpdate.IsActive,
                CreatedAt = userToUpdate.CreatedAt,
                UpdatedAt = DateTime.Now
            };

            ValidateUserDTO(updatedResponse);

            return updatedResponse;
        }

        public async Task<bool> DeleteUserById(int id)
        {
            var userToDelete = await _userRepository.GetFullUserById(id);

            if (userToDelete is null)
            {
                throw new KeyNotFoundException($"User with id {id} not found!");
            }

            return await _userRepository.DeleteUserById(id);

        }

        private async Task ValidateRegisterDTO(RegisterDTO userRegister)
        {
            if (string.IsNullOrEmpty(userRegister.Name)
                || string.IsNullOrEmpty(userRegister.Cpf)
                || string.IsNullOrEmpty(userRegister.Password))
            {
                throw new ArgumentException("Name, CPF and Password are required fields!");
            }

            var userExists = await _userRepository.GetUserByCpf(userRegister.Cpf);

            if (userExists is not null)
            {
                throw new ArgumentException("A user with this CPF already exists.");
            }
        }

        private void ValidateUserDTO(UserResponseDTO userDTO)
        {
            if (string.IsNullOrEmpty(userDTO.Name)
                || string.IsNullOrEmpty(userDTO.Cpf))
            {
                throw new ArgumentException("Name and CPF are required fields!");
            }
        }
    }
}