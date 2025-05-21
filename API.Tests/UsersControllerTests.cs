using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Controllers;
using API.DTOs;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace API.Tests
{
    public class UsersControllerTests
    {
        private readonly Mock<IUserService> _mockService;
        private readonly UsersController _controller;
        
        public UsersControllerTests()
        {
            _mockService = new Mock<IUserService>();
            _controller = new UsersController(_mockService.Object);
        }
        
        [Fact]
        public async Task GetUsers_ReturnsOkResult_WithListOfUsers()
        {
            // Arrange
            var users = new List<UserResponseDto>
            {
                new UserResponseDto { Id = 1, Username = "user1" },
                new UserResponseDto { Id = 2, Username = "user2" }
            };
            
            _mockService.Setup(service => service.GetAllUsersAsync())
                .ReturnsAsync(users);
                
            // Act
            var result = await _controller.GetUsers();
            
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedUsers = Assert.IsAssignableFrom<IEnumerable<UserResponseDto>>(okResult.Value);
            Assert.Equal(2, returnedUsers.Count());
        }
        
        [Fact]
        public async Task GetUser_WithExistingId_ReturnsOkResult_WithUser()
        {
            // Arrange
            var userId = 1;
            var user = new UserResponseDto 
            { 
                Id = userId, 
                Username = "testuser",
                Email = "test@example.com"
            };
            
            _mockService.Setup(service => service.GetUserByIdAsync(userId))
                .ReturnsAsync(user);
                
            // Act
            var result = await _controller.GetUser(userId);
            
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedUser = Assert.IsType<UserResponseDto>(okResult.Value);
            Assert.Equal(userId, returnedUser.Id);
            Assert.Equal("testuser", returnedUser.Username);
        }
        
        [Fact]
        public async Task GetUser_WithNonExistingId_ReturnsNotFound()
        {
            // Arrange
            var userId = 999;
            
            _mockService.Setup(service => service.GetUserByIdAsync(userId))
                .ReturnsAsync((UserResponseDto)null);
                
            // Act
            var result = await _controller.GetUser(userId);
            
            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }
        
        [Fact]
        public async Task CreateUser_WithValidModel_ReturnsCreatedAtAction()
        {
            // Arrange
            var createDto = new UserCreateDto
            {
                Username = "newuser",
                Email = "newuser@example.com"
            };
            
            var createdUser = new UserResponseDto
            {
                Id = 1,
                Username = "newuser",
                Email = "newuser@example.com"
            };
            
            _mockService.Setup(service => service.CreateUserAsync(createDto))
                .ReturnsAsync(createdUser);
                
            // Act
            var result = await _controller.CreateUser(createDto);
            
            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedUser = Assert.IsType<UserResponseDto>(createdAtActionResult.Value);
            Assert.Equal(1, returnedUser.Id);
            Assert.Equal("newuser", returnedUser.Username);
            Assert.Equal("GetUser", createdAtActionResult.ActionName);
            Assert.Equal(1, createdAtActionResult.RouteValues["id"]);
        }
        
        [Fact]
        public async Task UpdateUser_WithExistingId_ReturnsOkResult_WithUpdatedUser()
        {
            // Arrange
            var userId = 1;
            var updateDto = new UserUpdateDto
            {
                Username = "updateduser",
                Email = "updated@example.com"
            };
            
            var updatedUser = new UserResponseDto
            {
                Id = userId,
                Username = "updateduser",
                Email = "updated@example.com"
            };
            
            _mockService.Setup(service => service.UpdateUserAsync(userId, updateDto))
                .ReturnsAsync(updatedUser);
                
            // Act
            var result = await _controller.UpdateUser(userId, updateDto);
            
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedUser = Assert.IsType<UserResponseDto>(okResult.Value);
            Assert.Equal(userId, returnedUser.Id);
            Assert.Equal("updateduser", returnedUser.Username);
        }
        
        [Fact]
        public async Task UpdateUser_WithNonExistingId_ReturnsNotFound()
        {
            // Arrange
            var userId = 999;
            var updateDto = new UserUpdateDto
            {
                Username = "updateduser",
                Email = "updated@example.com"
            };
            
            _mockService.Setup(service => service.UpdateUserAsync(userId, updateDto))
                .ReturnsAsync((UserResponseDto)null);
                
            // Act
            var result = await _controller.UpdateUser(userId, updateDto);
            
            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }
        
        [Fact]
        public async Task DeleteUser_WithExistingId_ReturnsNoContent()
        {
            // Arrange
            var userId = 1;
            
            _mockService.Setup(service => service.DeleteUserAsync(userId))
                .ReturnsAsync(true);
                
            // Act
            var result = await _controller.DeleteUser(userId);
            
            // Assert
            Assert.IsType<NoContentResult>(result);
        }
        
        [Fact]
        public async Task DeleteUser_WithNonExistingId_ReturnsNotFound()
        {
            // Arrange
            var userId = 999;
            
            _mockService.Setup(service => service.DeleteUserAsync(userId))
                .ReturnsAsync(false);
                
            // Act
            var result = await _controller.DeleteUser(userId);
            
            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
} 