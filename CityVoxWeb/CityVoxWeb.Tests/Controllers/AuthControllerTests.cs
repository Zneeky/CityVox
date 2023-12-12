using CityVoxWeb.API.Controllers;
using CityVoxWeb.DTOs.Users;
using CityVoxWeb.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace CityVoxWeb.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IUsersService> _mockUserService;
        private readonly Mock<IJwtUtils> _mockJwtUtils;
        private readonly Mock<IRefreshTokenService> _mockRefreshTokenService;
        private readonly Mock<IConfiguration> _mockConfig;
        private readonly AuthController _controller;
        private readonly Mock<IEmailService> _mockEmailService;

        public AuthControllerTests()
        {
            _mockUserService = new Mock<IUsersService>();
            _mockJwtUtils = new Mock<IJwtUtils>();
            _mockRefreshTokenService = new Mock<IRefreshTokenService>();
            _mockConfig = new Mock<IConfiguration>();
            _mockEmailService = new Mock<IEmailService>();
            _controller = new AuthController(_mockUserService.Object, _mockJwtUtils.Object, _mockRefreshTokenService.Object, _mockConfig.Object, _mockEmailService.Object);
        }

        [Fact]
        public async Task Register_ExistingUser_ShouldReturnBadRequest()
        {
            // Arrange
            var registerDto = new RegisterDto();
            _mockUserService.Setup(u => u.RegisterUserAsync(registerDto)).ReturnsAsync((UserWithIdDto)null);

            // Act
            var result = await _controller.Register(registerDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = badRequestResult.Value;

            var messageProperty = returnValue.GetType().GetProperty("message");
            Assert.NotNull(messageProperty);

            var messageValue = messageProperty.GetValue(returnValue).ToString();
            Assert.Equal("User already exists!", messageValue);
        }

        [Fact]
        public async Task Login_InvalidCredentials_ShouldReturnBadRequest()
        {
            // Arrange
            var loginDto = new LoginDto();
            _mockUserService.Setup(u => u.AuthenticateUserAsync(loginDto)).ReturnsAsync((UserWithIdDto)null);

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = badRequestResult.Value;

            var messageProperty = returnValue.GetType().GetProperty("message");
            Assert.NotNull(messageProperty);

            var messageValue = messageProperty.GetValue(returnValue).ToString();
            Assert.Equal("Invalid credentials!", messageValue);
        }

        [Fact]
        public async Task LogInWithRefreshToken_InvalidToken_ShouldReturnBadRequest()
        {
            // Arrange
            string jwtToken = "invalidToken";


            var mockCookieCollection = new Mock<IRequestCookieCollection>();
            mockCookieCollection.Setup(m => m["refreshToken"]).Returns((string)null);


            var mockRequest = new Mock<HttpRequest>();
            mockRequest.Setup(r => r.Cookies).Returns(mockCookieCollection.Object);


            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(c => c.Request).Returns(mockRequest.Object);

            _controller.ControllerContext.HttpContext = mockHttpContext.Object;

            _mockRefreshTokenService.Setup(r => r.IsValid(It.IsAny<string>(), jwtToken)).ReturnsAsync((false, "invalidUserId"));

            // Act
            var result = await _controller.LogInWithRefreshToken();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = badRequestResult.Value;

            var messageProperty = returnValue.GetType().GetProperty("message");
            Assert.NotNull(messageProperty);

            var messageValue = messageProperty.GetValue(returnValue).ToString();
            Assert.Equal("Invalid tokens! Sing in again!", messageValue);
        }

        [Fact]
        public async Task Logout_NoRefreshToken_ShouldReturnBadRequest()
        {
            // Arrange
            string jwtToken = "invalidToken";


            var mockCookieCollection = new Mock<IRequestCookieCollection>();
            mockCookieCollection.Setup(m => m["refreshToken"]).Returns((string)null);


            var mockRequest = new Mock<HttpRequest>();
            mockRequest.Setup(r => r.Cookies).Returns(mockCookieCollection.Object);


            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(c => c.Request).Returns(mockRequest.Object);

            _controller.ControllerContext.HttpContext = mockHttpContext.Object;

            _mockRefreshTokenService.Setup(r => r.IsValid(It.IsAny<string>(), jwtToken)).ReturnsAsync((false, "invalidUserId"));

            // Act
            var result = await _controller.Logout();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = badRequestResult.Value;

            var messageProperty = returnValue.GetType().GetProperty("message");
            Assert.NotNull(messageProperty);

            var messageValue = messageProperty.GetValue(returnValue).ToString();
            Assert.Equal("Invalid tokens! Sing in again!", messageValue);
        }
    }
}
