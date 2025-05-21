using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Models;
using API.Repositories;

namespace API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return users.Select(MapUserToResponseDto);
        }

        public async Task<UserResponseDto?> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            return user != null ? MapUserToResponseDto(user) : null;
        }

        public async Task<UserResponseDto> CreateUserAsync(UserCreateDto userCreateDto)
        {
            var user = new User
            {
                Username = userCreateDto.Username,
                Email = userCreateDto.Email,
                FirstName = userCreateDto.FirstName,
                LastName = userCreateDto.LastName
            };

            var createdUser = await _userRepository.CreateUserAsync(user);
            return MapUserToResponseDto(createdUser);
        }

        public async Task<UserResponseDto?> UpdateUserAsync(int id, UserUpdateDto userUpdateDto)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(id);
            if (existingUser == null)
                return null;

            existingUser.Username = userUpdateDto.Username;
            existingUser.Email = userUpdateDto.Email;
            existingUser.FirstName = userUpdateDto.FirstName;
            existingUser.LastName = userUpdateDto.LastName;
            existingUser.IsActive = userUpdateDto.IsActive;

            var updatedUser = await _userRepository.UpdateUserAsync(id, existingUser);
            return updatedUser != null ? MapUserToResponseDto(updatedUser) : null;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            return await _userRepository.DeleteUserAsync(id);
        }

        private static UserResponseDto MapUserToResponseDto(User user)
        {
            return new UserResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                CreatedAt = user.CreatedAt,
                IsActive = user.IsActive
            };
        }
    }
} 