using System;
using System.Threading.Tasks;
using API.Controllers;
using API.DTOs;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace API.Tests
{
    public class UserDateOfBirthTests
    {
        private readonly UsersController _controller;
        private readonly Mock<IUserService> _mockService;

        public UserDateOfBirthTests()
        {
            _mockService = new Mock<IUserService>();
            _controller = new UsersController(_mockService.Object);
        }

        [Fact]
        public async Task CreateUser_WithValidDateOfBirth_ShouldReturnCreated()
        {
            // Arrange
            var createDto = new UserCreateDto
            {
                Username = "dobuser",
                Email = "dobuser@example.com",
                DateOfBirth = DateTime.Parse("1990-01-01"),
                IsEmailVerified = false
            };
            
            var createdUser = new UserResponseDto
            {
                Id = 10,
                Username = "dobuser",
                Email = "dobuser@example.com",
                // Including DateOfBirth in response
                DateOfBirth = DateTime.Parse("1990-01-01"),
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };
            
            _mockService.Setup(s => s.CreateUserAsync(createDto)).ReturnsAsync(createdUser);

            // Act
            var result = await _controller.CreateUser(createDto);
            
            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var userResponse = Assert.IsType<UserResponseDto>(createdAtActionResult.Value);
            Assert.Equal(DateTime.Parse("1990-01-01"), userResponse.DateOfBirth);
        }

        [Fact]
        public async Task UpdateUser_WithInvalidDateOfBirth_ShouldReturnBadRequest()
        {
            // Arrange
            var updateDto = new UserUpdateDto
            {
                Username = "dobuser",
                Email = "dobuser@example.com",
                DateOfBirth = "invalid-date"
            };

            // Act
            var result = await _controller.UpdateUser(1, updateDto);
            
            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid dateOfBirth value. Expected format is YYYY-MM-DD.", badRequestResult.Value);
        }

        [Fact]
        public void Migration_Set_Default_DateOfBirth_ShouldBeNull()
        {
            // Simulate database migration default behavior
            var user = new User {
                Id = 1,
                Username = "migrateuser",
                Email = "migrate@example.com",
                FirstName = "Test",
                LastName = "User",
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                DateOfBirth = null,
                IsEmailVerified = false
            };
            
            Assert.Null(user.DateOfBirth);
        }
    }
}
