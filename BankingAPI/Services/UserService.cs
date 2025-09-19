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
                CreatePasswordHash(userRegister.Password, out byte[] hash, out byte[] salt);

                var newUser = new UserModel
                {
                    Name = userRegister.Name,
                    Cpf = userRegister.Cpf,
                    Celphone = userRegister.Celphone,
                    Email = userRegister.Email,
                    PasswordHash = hash,
                    PasswordSalt = salt,
                    Role = userRegister.Role
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

            private void ValidateRegisterDTO(RegisterDTO userRegister)
            {
                if (string.IsNullOrEmpty(userRegister.Name)
                    || string.IsNullOrEmpty(userRegister.Cpf))
                {
                    throw new ArgumentException("Name, CPF and Password are required fields!");
                }

            var userExists = await _userRepository.GetUserByCpf(userRegister.Cpf);

            if (userExists is not null)
            {
                throw new ArgumentException("A user with this CPF already exists.");
            }
            }

            private void ValidateUserDTO(UserModelDTO userDTO)
            {
                if (string.IsNullOrEmpty(userDTO.Name)
                    || string.IsNullOrEmpty(userDTO.Cpf))
                {
                    throw new ArgumentException("Name and CPF are required fields!");
                }
            }
        }
    }
