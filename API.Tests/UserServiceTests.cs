using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Models;
using API.Repositories;
using API.Services;
using Moq;
using Xunit;

namespace API.Tests
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _mockRepository;
        private readonly UserService _userService;
        
        public UserServiceTests()
        {
            _mockRepository = new Mock<IUserRepository>();
            _userService = new UserService(_mockRepository.Object);
        }
        
        [Fact]
        public async Task GetAllUsersAsync_ReturnsAllUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = 1, Username = "user1", Email = "user1@example.com" },
                new User { Id = 2, Username = "user2", Email = "user2@example.com" }
            };
            
            _mockRepository.Setup(repo => repo.GetAllUsersAsync())
                .ReturnsAsync(users);
                
            // Act
            var result = await _userService.GetAllUsersAsync();
            
            // Assert
            var usersList = result.ToList();
            Assert.Equal(2, usersList.Count);
            Assert.Equal(1, usersList[0].Id);
            Assert.Equal("user1", usersList[0].Username);
            Assert.Equal(2, usersList[1].Id);
            Assert.Equal("user2", usersList[1].Username);
        }
        
        [Fact]
        public async Task GetUserByIdAsync_WithExistingId_ReturnsUser()
        {
            // Arrange
            var userId = 1;
            var user = new User 
            { 
                Id = userId, 
                Username = "testuser", 
                Email = "test@example.com",
                FirstName = "Test",
                LastName = "User"
            };
            
            _mockRepository.Setup(repo => repo.GetUserByIdAsync(userId))
                .ReturnsAsync(user);
                
            // Act
            var result = await _userService.GetUserByIdAsync(userId);
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
            Assert.Equal("testuser", result.Username);
            Assert.Equal("test@example.com", result.Email);
            Assert.Equal("Test", result.FirstName);
            Assert.Equal("User", result.LastName);
        }
        
        [Fact]
        public async Task GetUserByIdAsync_WithNonExistingId_ReturnsNull()
        {
            // Arrange
            var userId = 999;
            
            _mockRepository.Setup(repo => repo.GetUserByIdAsync(userId))
                .ReturnsAsync((User)null);
                
            // Act
            var result = await _userService.GetUserByIdAsync(userId);
            
            // Assert
            Assert.Null(result);
        }
        
        [Fact]
        public async Task CreateUserAsync_CreatesAndReturnsUser()
        {
            // Arrange
            var createDto = new UserCreateDto
            {
                Username = "newuser",
                Email = "newuser@example.com",
                FirstName = "New",
                LastName = "User"
            };
            
            var createdUser = new User
            {
                Id = 1,
                Username = createDto.Username,
                Email = createDto.Email,
                FirstName = createDto.FirstName,
                LastName = createDto.LastName
            };
            
            _mockRepository.Setup(repo => repo.CreateUserAsync(It.IsAny<User>()))
                .ReturnsAsync(createdUser);
                
            // Act
            var result = await _userService.CreateUserAsync(createDto);
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("newuser", result.Username);
            Assert.Equal("newuser@example.com", result.Email);
            Assert.Equal("New", result.FirstName);
            Assert.Equal("User", result.LastName);
        }
        
        [Fact]
        public async Task UpdateUserAsync_WithExistingId_UpdatesAndReturnsUser()
        {
            // Arrange
            var userId = 1;
            var existingUser = new User
            {
                Id = userId,
                Username = "olduser",
                Email = "old@example.com",
                FirstName = "Old",
                LastName = "User",
                IsActive = true
            };
            
            var updateDto = new UserUpdateDto
            {
                Username = "updateduser",
                Email = "updated@example.com",
                FirstName = "Updated",
                LastName = "User",
                IsActive = true
            };
            
            var updatedUser = new User
            {
                Id = userId,
                Username = updateDto.Username,
                Email = updateDto.Email,
                FirstName = updateDto.FirstName,
                LastName = updateDto.LastName,
                IsActive = updateDto.IsActive
            };
            
            _mockRepository.Setup(repo => repo.GetUserByIdAsync(userId))
                .ReturnsAsync(existingUser);
                
            _mockRepository.Setup(repo => repo.UpdateUserAsync(userId, It.IsAny<User>()))
                .ReturnsAsync(updatedUser);
                
            // Act
            var result = await _userService.UpdateUserAsync(userId, updateDto);
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
            Assert.Equal("updateduser", result.Username);
            Assert.Equal("updated@example.com", result.Email);
            Assert.Equal("Updated", result.FirstName);
            Assert.Equal("User", result.LastName);
            Assert.True(result.IsActive);
        }
        
        [Fact]
        public async Task UpdateUserAsync_WithNonExistingId_ReturnsNull()
        {
            // Arrange
            var userId = 999;
            var updateDto = new UserUpdateDto
            {
                Username = "updateduser",
                Email = "updated@example.com"
            };
            
            _mockRepository.Setup(repo => repo.GetUserByIdAsync(userId))
                .ReturnsAsync((User)null);
                
            // Act
            var result = await _userService.UpdateUserAsync(userId, updateDto);
            
            // Assert
            Assert.Null(result);
        }
        
        [Fact]
        public async Task DeleteUserAsync_WithExistingId_ReturnsTrue()
        {
            // Arrange
            var userId = 1;
            
            _mockRepository.Setup(repo => repo.DeleteUserAsync(userId))
                .ReturnsAsync(true);
                
            // Act
            var result = await _userService.DeleteUserAsync(userId);
            
            // Assert
            Assert.True(result);
        }
        
        [Fact]
        public async Task DeleteUserAsync_WithNonExistingId_ReturnsFalse()
        {
            // Arrange
            var userId = 999;
            
            _mockRepository.Setup(repo => repo.DeleteUserAsync(userId))
                .ReturnsAsync(false);
                
            // Act
            var result = await _userService.DeleteUserAsync(userId);
            
            // Assert
            Assert.False(result);
        }
    }
} 