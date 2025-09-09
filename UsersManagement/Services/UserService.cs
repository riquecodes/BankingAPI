using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UsersManagement.Context;
using UsersManagement.Models;
using UsersManagement.Repositories;

namespace UsersManagement.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly AppDbContext _context;

        public UserService(IUserRepository userRepository, AppDbContext dbContext)
        {
            _userRepository = userRepository;
            _context = dbContext;
        }

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

            var newUser = new UserModel
            {
                Name = userDTO.Name,
                Cpf = userDTO.Cpf,
                Celphone = userDTO.Celphone,
                Email = userDTO.Email
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

            if (string.IsNullOrEmpty(userDTO.Name)
                || string.IsNullOrEmpty(userDTO.Cpf))
            {
                throw new ArgumentException("Name and CPF are required fields!");
            }

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
