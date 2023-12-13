using CityVoxWeb.API.Controllers;
using CityVoxWeb.DTOs.Users;
using CityVoxWeb.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CityVoxWeb.Tests.Controllers
{
    public class UsersControllerTests
    {
        private readonly Mock<IUsersService> _mockUserService;
        private readonly Mock<IJwtUtils> _mockJwtUtils;
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            _mockUserService = new Mock<IUsersService>();
            _mockJwtUtils = new Mock<IJwtUtils>();
            _controller = new UsersController(_mockUserService.Object, _mockJwtUtils.Object);
        }

        [Fact]
        public async Task GetUsers_ShouldReturnListOfUsers()
        {
            // Arrange
            var expectedUsers = new List<UserDefaultDto>
            {
                new UserDefaultDto() { /* sample properties */ },
                new UserDefaultDto() { /* sample properties */ }
            };

            _mockUserService.Setup(service => service.GetUsersAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(expectedUsers);

            // Act
            var result = await _controller.GetUsers(1, 10);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedUsers, okResult.Value);
        }
    }
}
