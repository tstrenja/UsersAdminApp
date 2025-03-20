using Microsoft.AspNetCore.Mvc;
using Moq;
using UserAdmin.Core.Model;
using UserAdmin.Core.Service.Interface;
using Xunit;

namespace UserAdmin.Tests
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _controller = new UserController(_mockUserService.Object);
        }

        [Fact]
        public async Task GetUsers_ShouldReturnOkResult()
        {
            // Arrange
            var users = new List<UserDTO>
            {
                new UserDTO { Id = Guid.NewGuid(), UserName = "user1" },
                new UserDTO { Id = Guid.NewGuid(), UserName = "user2" }
            };
            _mockUserService.Setup(service => service.GetAll()).ReturnsAsync(users);

            // Act
            var result = await _controller.GetUsers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUsers = Assert.IsType<List<UserDTO>>(okResult.Value);
            Assert.Equal(2, returnedUsers.Count);
        }

        [Fact]
        public async Task GetById_ShouldReturnOkResult()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new UserDTO { Id = userId, UserName = "user1" };
            _mockUserService.Setup(service => service.GetById(userId)).ReturnsAsync(user);

            // Act
            var result = await _controller.GetById(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUser = Assert.IsType<UserDTO>(okResult.Value);
            Assert.Equal(userId, returnedUser.Id);
        }

        [Fact]
        public async Task Login_ShouldReturnTrue_WhenValidPassword()
        {
            // Arrange
            var loginDto = new LoginModel { Login = "user1", Password = "correctpassword" };
            var user = new UserDTO { Id = Guid.NewGuid(), UserName = "user1", Password = BCrypt.Net.BCrypt.HashPassword("correctpassword") };
            _mockUserService.Setup(service => service.GetByUsername("user1")).ReturnsAsync(user);

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsType<bool>(okResult.Value);
            Assert.True(value); 
        }

        [Fact]
        public async Task Login_ShouldReturnFalse_WhenInvalidPassword()
        {
            // Arrange
            var loginDto = new LoginModel { Login = "user1", Password = "wrongpassword" };
            var user = new UserDTO { Id = Guid.NewGuid(), UserName = "user1", Password = BCrypt.Net.BCrypt.HashPassword("correctpassword") };
            _mockUserService.Setup(service => service.GetByUsername("user1")).ReturnsAsync(user);

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsType<bool>(okResult.Value);
            Assert.False(value); 
        }

    }
}
