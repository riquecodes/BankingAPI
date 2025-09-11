using BankingAPI.Context;
    using BankingAPI.Models;
    using BankingAPI.Repositories;

    namespace BankingAPI.Services
    {
        public class UserService(IUserRepository userRepository) : IUserService
        {
            private readonly IUserRepository _userRepository = userRepository;

            public async Task<IEnumerable<UserModel>> GetUsers()
            {
                var users = await _userRepository.GetUsers();

                if (!users.Any())
                { 
                    throw new KeyNotFoundException("No users found!");
                }

                return users.OrderBy(u => u.Name);
            }

            public async Task<UserModel?> GetUserById(int id)
            {
                var user = await _userRepository.GetUserById(id);

                if (user is null) 
                { 
                    throw new KeyNotFoundException($"User with id {id} not found!");
                }

                return user;
            }

            public async Task<UserModel> CreateUser(UserModelDTO userDTO)
            {
                ValidateUserDTO(userDTO);

                CreatePasswordHash(userDTO.Password, out byte[] hash, out byte[] salt);

                var newUser = new UserModel
                {
                    Name = userDTO.Name,
                    Cpf = userDTO.Cpf,
                    Celphone = userDTO.Celphone,
                    Email = userDTO.Email,
                    PasswordHash = hash,
                    PasswordSalt = salt,
                    Role = userDTO.Role
                };

                var createdUser = await _userRepository.CreateUser(newUser);
                return createdUser;
            }
        
            public async Task<UserModel?> UpdateUser(int id, UserModelDTO userDTO)
            {

                var userToUpdate = await _userRepository.GetUserById(id);

                if (userToUpdate is null)
                {
                    throw new KeyNotFoundException($"User with id {id} not found!");
                }

                ValidateUserDTO(userDTO);

                userToUpdate.Name = userDTO.Name;
                userToUpdate.Cpf = userDTO.Cpf;
                userToUpdate.Celphone = userDTO.Celphone;
                userToUpdate.Email = userDTO.Email;

                return await _userRepository.UpdateUser(id, userToUpdate);
            }

            public async Task<bool> DeleteUserById(int id)
            {
                var userToDelete = await _userRepository.GetUserById(id);

                if (userToDelete is null)
                {
                    throw new KeyNotFoundException($"User with id {id} not found!");
                }

                return await _userRepository.DeleteUserById(id);

            }

            private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
            {
                using (var hmac = new System.Security.Cryptography.HMACSHA512())
                {
                    passwordSalt = hmac.Key;
                    passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                }
            }

            private void ValidateUserDTO(UserModelDTO userDTO)
            {
                if (string.IsNullOrEmpty(userDTO.Name)
                    || string.IsNullOrEmpty(userDTO.Cpf))
                {
                    throw new ArgumentException("Name, CPF and Password are required fields!");
                }
            }
        }
    }
