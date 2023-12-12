using Xunit;
using Moq;
using CityVoxWeb.Services;
using CityVoxWeb.Data;
using Microsoft.AspNetCore.Identity;
using System;
using CityVoxWeb.Data.Models.UserEntities;
using CityVoxWeb.DTOs.Users;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using CityVoxWeb.Services.User_Services;
using CityVoxWeb.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace CityVoxWeb.Tests.Services
{
    public class UsersServiceTests : IDisposable
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
        private readonly Mock<RoleManager<IdentityRole<Guid>>> _mockRoleManager;
        private readonly UsersService _userService;
        private readonly CityVoxDbContext _dbContext;
        private readonly Mock<IEmailService> _mockEmailService;
        private readonly Mock<IConfiguration> _mockConfig;


        public UsersServiceTests()
        {
            _mockMapper = new Mock<IMapper>();
            var options = new DbContextOptionsBuilder<CityVoxDbContext>()
            .UseInMemoryDatabase(databaseName: $"{Guid.NewGuid}") // Use a unique name for each test
            .Options;

            _dbContext = new CityVoxDbContext(options);

            // Populate the DbSet with a dummy list
            var mockUsers = new List<ApplicationUser>().AsQueryable();

            // Mocking UserManager and RoleManager requires a little more setup due to their complexity.
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            _mockUserManager = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            var roleStore = new Mock<IRoleStore<IdentityRole<Guid>>>();
            _mockRoleManager = new Mock<RoleManager<IdentityRole<Guid>>>(roleStore.Object, null, null, null, null);

            // Setup for FindByNameAsync
            _mockUserManager.Setup(um => um.FindByNameAsync(It.IsAny<string>()))
                            .ReturnsAsync(new ApplicationUser());

            // Setup for CreateAsync
            _mockUserManager.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                            .ReturnsAsync(IdentityResult.Success);

            _userService = new UsersService(_mockMapper.Object, _dbContext, _mockUserManager.Object, _mockRoleManager.Object , _mockConfig.Object , _mockEmailService.Object );
            SeedDatabase(_dbContext);
        }

        private void SeedDatabase(CityVoxDbContext dbContext)
        {
            var user1 = new ApplicationUser
            {
                UserName = "Zneeky",
                Email = "zneeky@abv.bg",
                FirstName = "Arkan",
                LastName = "Ahmedov",
                ProfilePictureUrl = "https://i.imgur.com/3tj9h9X.jpg",
                PasswordHash = Guid.NewGuid().ToString(),
                SecurityStamp = Guid.NewGuid().ToString(),
                Id = Guid.NewGuid(),
                EmailConfirmed = true,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                LockoutEnd = null,
                PhoneNumber = null,
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                NormalizedEmail = "ZNEEKY@ABV.BG",
                NormalizedUserName = "ZNEEKY",
            };
            var user2 = new ApplicationUser
            {
                UserName = "SparkleStar",
                Email = "sparkle.star@email.com",
                FirstName = "Luna",
                LastName = "Silvermoon",
                ProfilePictureUrl = "https://i.imgur.com/xyz123.jpg",
                PasswordHash = Guid.NewGuid().ToString(),
                SecurityStamp = Guid.NewGuid().ToString(),
                Id = Guid.NewGuid(),
                EmailConfirmed = true,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                LockoutEnd = null,
                PhoneNumber = null,
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                NormalizedEmail = "SPARKLE.STAR@EMAIL.COM",
                NormalizedUserName = "SPARKLESTAR",
            };

            var user3 = new ApplicationUser
            {
                UserName = "TechNinja32",
                Email = "techninja32@gmail.com",
                FirstName = "Alex",
                LastName = "Johnson",
                ProfilePictureUrl = "https://i.imgur.com/abc456.jpg",
                PasswordHash = Guid.NewGuid().ToString(),
                SecurityStamp = Guid.NewGuid().ToString(),
                Id = Guid.NewGuid(),
                EmailConfirmed = true,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                LockoutEnd = null,
                PhoneNumber = null,
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                NormalizedEmail = "TECHNINJA32@GMAIL.COM",
                NormalizedUserName = "TECHNINJA32",
            };

            var user4 = new ApplicationUser
            {
                UserName = "GamerChick88",
                Email = "gamerchick88@hotmail.com",
                FirstName = "Emily",
                LastName = "Smith",
                ProfilePictureUrl = "https://i.imgur.com/def789.jpg",
                PasswordHash = Guid.NewGuid().ToString(),
                SecurityStamp = Guid.NewGuid().ToString(),
                Id = Guid.NewGuid(),
                EmailConfirmed = true,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                LockoutEnd = null,
                PhoneNumber = null,
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                NormalizedEmail = "GAMERCHICK88@HOTMAIL.COM",
                NormalizedUserName = "GAMERCHICK88",
            };

            var user5 = new ApplicationUser
            {
                UserName = "NatureLover123",
                Email = "naturelover123@yahoo.com",
                FirstName = "Ethan",
                LastName = "Greenwood",
                ProfilePictureUrl = "https://i.imgur.com/ghi910.jpg",
                PasswordHash = Guid.NewGuid().ToString(),
                SecurityStamp = Guid.NewGuid().ToString(),
                Id = Guid.NewGuid(),
                EmailConfirmed = true,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                LockoutEnd = null,
                PhoneNumber = null,
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                NormalizedEmail = "NATURELOVER123@YAHOO.COM",
                NormalizedUserName = "NATURELOVER123",
            };

            var user6 = new ApplicationUser
            {
                UserName = "FoodieExplorer",
                Email = "foodieexplorer@example.com",
                FirstName = "Sophie",
                LastName = "Anderson",
                ProfilePictureUrl = "https://i.imgur.com/jkl111.jpg",
                PasswordHash = Guid.NewGuid().ToString(),
                SecurityStamp = Guid.NewGuid().ToString(),
                Id = Guid.NewGuid(),
                EmailConfirmed = true,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                LockoutEnd = null,
                PhoneNumber = null,
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                NormalizedEmail = "FOODIEEXPLORER@EXAMPLE.COM",
                NormalizedUserName = "FOODIEEXPLORER",
            };
            // Add more mock entities as needed

            dbContext.Users.AddRange(user1, user2, user3, user4, user5, user6);
            dbContext.SaveChanges();
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
        }

        [Fact]
        public async Task RegisterUserAsync_WithValidInput_ReturnsUserWithIdDto()
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                Username = "Zneeky",
                Email = "zneeky@abv.bg",
                Password = "Zneeky2002",
                FirstName = "Arkan",
                LastName = "Ahmedov",
                ProfilePictureUrl = "https://i.imgur.com/3tj9h9X.jpg"
            };

            var predefinedApplicationUser = new ApplicationUser
            {
                UserName = "Zneeky",
                Email = "zneeky@abv.bg",
                FirstName = "Arkan",
                LastName = "Ahmedov",
                ProfilePictureUrl = "https://i.imgur.com/3tj9h9X.jpg",
                PasswordHash = Guid.NewGuid().ToString(),
                SecurityStamp = Guid.NewGuid().ToString(),
                Id = Guid.NewGuid(),
                EmailConfirmed = true,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                LockoutEnd = null,
                PhoneNumber = null,
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                NormalizedEmail = "ZNEEKY@ABV.BG",
                NormalizedUserName = "ZNEEKY",
            };

            var expectedUserDto = new UserWithIdDto
            {
                Username = predefinedApplicationUser.UserName,
                Email = predefinedApplicationUser.Email,
                FirstName = predefinedApplicationUser.FirstName,
                LastName = predefinedApplicationUser.LastName,
                ProfilePicture = predefinedApplicationUser.ProfilePictureUrl,
                Id = predefinedApplicationUser.Id,
                Role = "user" // Set the role as "user" since this is being set in the method being tested
            };

            _mockMapper.Setup(m => m.Map<ApplicationUser>(registerDto)).Returns(predefinedApplicationUser);

            _mockUserManager.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                            .ReturnsAsync(IdentityResult.Success);

            _mockUserManager.Setup(um => um.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                            .ReturnsAsync(IdentityResult.Success);

            _mockMapper.Setup(m => m.Map<UserWithIdDto>(predefinedApplicationUser)).Returns(expectedUserDto);

            // Act
            var result = await _userService.RegisterUserAsync(registerDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedUserDto.Username, result.Username);
            Assert.Equal(expectedUserDto.Email, result.Email);
            Assert.Equal(expectedUserDto.FirstName, result.FirstName);
            Assert.Equal(expectedUserDto.LastName, result.LastName);
            Assert.Equal(expectedUserDto.ProfilePicture, result.ProfilePicture);
            Assert.Equal(expectedUserDto.Id, result.Id);
            Assert.Equal(expectedUserDto.Role, result.Role);
            //... further assertions based on expected outcome
        }

        [Fact]
        public async Task AuthenticateUserAsync_ValidCredentials_ReturnsUserWithIdDto()
        {
            // Arrange
            string testEmail = "zneeky@abv.bg";
            string testPassword = "TestPassword"; // Note: set this to whatever is the correct password for the mock user
            string testRole = "user"; // Note: set this to whatever is the expected role for the user

            var loginDto = new LoginDto
            {
                Email = testEmail,
                Password = testPassword
            };

            var mockUser = new ApplicationUser
            {
                UserName = "Zneeky",
                Email = "zneeky@abv.bg",
                FirstName = "Arkan",
                LastName = "Ahmedov",
                ProfilePictureUrl = "https://i.imgur.com/3tj9h9X.jpg",
                PasswordHash = Guid.NewGuid().ToString(),
                SecurityStamp = Guid.NewGuid().ToString(),
                Id = Guid.NewGuid(),
                EmailConfirmed = true,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                LockoutEnd = null,
                PhoneNumber = null,
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                NormalizedEmail = "ZNEEKY@ABV.BG",
                NormalizedUserName = "ZNEEKY",
            };

            var userDtoExpected = new UserWithIdDto
            {
                //... set the expected values
            };

            // Mocking the FindByEmailAsync method to return our mock user
            _mockUserManager.Setup(um => um.FindByEmailAsync(testEmail))
                            .ReturnsAsync(mockUser);

            // Mocking the CheckPasswordAsync method to return true (indicating the password is correct)
            _mockUserManager.Setup(um => um.CheckPasswordAsync(mockUser, testPassword))
                            .ReturnsAsync(true);

            // Mocking the GetRolesAsync method to return a list with our test role
            _mockUserManager.Setup(um => um.GetRolesAsync(mockUser))
                            .ReturnsAsync(new List<string> { testRole });

            // Mocking the Mapper method
            _mockMapper.Setup(m => m.Map<UserWithIdDto>(mockUser))
                       .Returns(userDtoExpected);

            // Act
            var result = await _userService.AuthenticateUserAsync(loginDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userDtoExpected.Email, result.Email);
            // ... further assertions based on expected outcome
        }

        [Fact]
        public async Task AuthenticateUserAsync_InvalidCredentials_ReturnsNull()
        {
            // Arrange
            string testEmail = "invalid@abv.bg";
            string testPassword = "InvalidPassword";

            var loginDto = new LoginDto
            {
                Email = testEmail,
                Password = testPassword
            };

            // Mocking the FindByEmailAsync method to return null (indicating user not found)
            _mockUserManager.Setup(um => um.FindByEmailAsync(testEmail))
                            .ReturnsAsync((ApplicationUser)null);

            // Act
            var result = await _userService.AuthenticateUserAsync(loginDto);

            // Assert
            Assert.Null(result);
        }
    }
}
