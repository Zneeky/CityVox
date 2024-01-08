using AutoMapper;
using CityVoxWeb.Data.Models.IssueEntities.Enumerators.Report;
using CityVoxWeb.Data.Models.IssueEntities;
using CityVoxWeb.Data.Models.SocialEntities.Enumerators;
using CityVoxWeb.Data.Models.SocialEntities;
using CityVoxWeb.Data.Models.UserEntities;
using CityVoxWeb.Data;
using CityVoxWeb.DTOs.Social;
using CityVoxWeb.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CityVoxWeb.Tests.Services
{
    public class SocialsServiceTests : IDisposable
    {
        private CityVoxDbContext _dbContext;
        private Mock<IMapper> _mockMapper;
        private SocialsService _service;


        public SocialsServiceTests()
        {
            var options = new DbContextOptionsBuilder<CityVoxDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique name for fresh DB each time
                .Options;

            _dbContext = new CityVoxDbContext(options);
            _mockMapper = new Mock<IMapper>();
            _service = new SocialsService(_dbContext, _mockMapper.Object);
        }

        private void SeedData()
        {

            // You can add more seeding for other entities like comments, votes, etc.

            _dbContext.SaveChanges();
        }

        private void SeedTestData(string municipalityId, string userId, string municipalityRepId)
        {
            var municipalityRep = new MunicipalityRepresentative
            {
                Id = Guid.Parse(municipalityRepId),
                Department = "Fake",
                UserId = Guid.NewGuid(),
                Position = "sdadd",
                StartDate = DateTime.UtcNow
            };

            var users = new List<ApplicationUser>
    {
        new ApplicationUser
        {
            Id = Guid.Parse(userId),
            FirstName = "Arkan",
            LastName = "Ahmedov",
            ProfilePictureUrl = null,
            CreatedAt = DateTime.MinValue,
            LastLoginTime = DateTime.MinValue,
            UserName = "Zneeky",
            NormalizedUserName = "ZNEEKY",
            Email = "keroch@abv.bg",
            NormalizedEmail = "KEROCH@ABV.BG",
            EmailConfirmed = false,
            PasswordHash = "AQAAAAEAACcQAAAAEJr6bPBa91oR3Nk4moQmv+vlUtrDEYL47lm7uHjBQDRmBzQpxq5qlg/7XARGanLvSw==",
            SecurityStamp = "YHFIDIOVKHGNKMSDBTKO65FWCIQXKPF3",
            ConcurrencyStamp = "b49d3524-5521-41d4-955f-773d7774f354",
            PhoneNumber = null,
            PhoneNumberConfirmed = false,
            TwoFactorEnabled = false,
            LockoutEnd = null,
            LockoutEnabled = false,
            AccessFailedCount = 0
        },
        new ApplicationUser
        {
            Id = Guid.Parse(municipalityRepId),
            FirstName = "Arka1n",
            LastName = "Ahmedov1",
            ProfilePictureUrl = null,
            CreatedAt = DateTime.MinValue,
            LastLoginTime = DateTime.MinValue,
            UserName = "Zneeky1",
            NormalizedUserName = "ZNEEKY1",
            Email = "keroch1@abv.bg",
            NormalizedEmail = "KEROCH1@ABV.BG",
            EmailConfirmed = false,
            PasswordHash = "AQAAAAEAACcQAAAAEJr6bPBa91oR3Nk4moQmv+vlUtrDEYL47lm7uHjBQDRmBzQpxq5qlg/7XARGanLvSw==",
            SecurityStamp = "YHFIDIOVKHGNKMSDBTKO65FWCIQXKPF3",
            ConcurrencyStamp = "b49d3524-5521-41d4-955f-773d7774f354",
            PhoneNumber = null,
            PhoneNumberConfirmed = false,
            TwoFactorEnabled = false,
            LockoutEnd = null,
            LockoutEnabled = false,
            AccessFailedCount = 0
        },
        // Add other user instances
    };

            var reports = new List<Report>
    {
        new Report
        {
            Id = Guid.Parse("F9E65917-EB1C-4670-85F1-08DB8DE3BF30"),
            Title = "asdasdadasda",
            Description = "dasdasdasdasdsadasda",
            ReportTime = DateTime.Parse("2023-07-26 14:23:35.6005374"),
            ResolvedTime = null,
            ImageUrl = null,
            Latitude = 42.6447549809156,
            Longitude = 23.3419101411693,
            Address = "Хляб и мръвка, Акад. Борис Стефанов 42, ж.к. Малинова долина, София 1734, България",
            UserId = Guid.Parse(userId),
            MunicipalityId = Guid.Parse(municipalityId),
            Type = ReportType.Littering,
            Status = ReportStatus.Reported
        },
        // Add other report instances
    };

            var posts = new List<Post>
    {
        new Post
        {
            Id = Guid.Parse("99179E59-AA5B-459F-D161-08DB96980BA2"),
            Text = "sdadasd",
            ImageUrls = null,
            CreatedAt = DateTime.Parse("2023-08-06 16:27:19.5740512"),
            PostType = PostType.Report,
            UserId = Guid.Parse(userId),
            ReportId = Guid.Parse("F9E65917-EB1C-4670-85F1-08DB8DE3BF30"),
            EmergencyId = null,
            EventId = null,
            InfrastructureIssueId = null
        },
        new Post
        {
            Id = Guid.Parse("19179E59-AA5B-459F-D161-08DB96980BA2"),
            Text = "sdadasd",
            ImageUrls = null,
            CreatedAt = DateTime.Parse("2023-08-06 16:27:19.5740512"),
            PostType = PostType.Report,
            UserId = Guid.Parse(municipalityRepId),
            ReportId =null,
            EmergencyId = null,
            EventId = null,
            InfrastructureIssueId = null
        },
        // Add other post instances
    };

            _dbContext.MunicipalityRepresentatives.AddRange(municipalityRep);
            _dbContext.Users.AddRange(users);
            _dbContext.Reports.AddRange(reports);
            _dbContext.Posts.AddRange(posts);

            _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        [Fact]
        public async Task CreatePostAsync_WhenCalled_ShouldAddPostToDbAndReturnTrue()
        {
            string issueId = Guid.NewGuid().ToString();
            string userId = Guid.NewGuid().ToString();
            // Arrange
            CreatePostDto createPostDto = new CreatePostDto
            {
                Text = "This is a sample post text.",
                ImageUrls = "https://image1.com;https://image2.com;https://image3.com",
                PostType = 2,
                IssueId = issueId,
                UserId = userId
            };

            Post mockPost = new Post
            {
                Id = Guid.NewGuid(),
                Text = "This is a sample post text.",
                ImageUrls = "https://image1.com;https://image2.com;https://image3.com",
                CreatedAt = DateTime.UtcNow,
                PostType = PostType.Report,
                UserId = Guid.Parse(userId),
                ReportId = Guid.Parse(issueId),
                EmergencyId = null,
                EventId = null,
                InfrastructureIssueId = null,
            };

            _mockMapper.Setup(m => m.Map<Post>(createPostDto)).Returns(mockPost);

            // Act
            var result = await _service.CreatePostAsync(createPostDto);

            // Assert
            Assert.True(result);
            Assert.Single(_dbContext.Posts); // xUnit's way to check if there's only one item in the collection
        }

        [Fact]
        public async Task GetPostsByMunicipalityIdAsync_GivenMunicipalityId_ShouldReturnRelevantPosts()
        {
            // Arrange
            var municipalityId = "1C752650-D449-49E2-B5AD-6EA4FFAF89F0".ToLower();
            var userId = "A0BC9D45-D406-4DD0-8230-08DB8E6DA18A".ToLower();
            var muniRepresentativeId = "B0BC9D45-D406-4DD0-8230-08DB8E6DA18A".ToLower();

            var mockPostDtos = new List<ExportPostDto>
            {
                 new ExportPostDto
                {
                     Id = userId,
                     IssueId = null,
                     Username = "A-2002-A",
                     Text = "Qko",
                     CreatedAt = DateTime.UtcNow.ToString(),
                     PostType = "Report",
                     PostTypeValue = 0,
                     UpVotesCount = 0,
                     IsUpVoted = false,
                     Comments = new HashSet<ExportCommentDto>(),

                }
            };

            _mockMapper.Setup(m => m.Map<ICollection<ExportPostDto>>(It.IsAny<object>(), It.IsAny<Action<IMappingOperationOptions<object, ICollection<ExportPostDto>>>>()))
           .Returns(mockPostDtos);

            // Seed data to in-memory database
            SeedTestData(municipalityId, userId, muniRepresentativeId);

            // Act
            var result = await _service.GetPostsByMunicipalityIdAsync(municipalityId, userId);

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(1, result.Count); // or any other expected count based on the mock data
        }

        [Fact]
        public async Task GetFormalPostsByMunicipalityIdAsync_GivenMunicipalityId_ShouldReturnRelevantFormalPosts()
        {
            // Arrange
            var municipalityId = "1C752650-D449-49E2-B5AD-6EA4FFAF89F0".ToLower();
            var userId = "A0BC9D45-D406-4DD0-8230-08DB8E6DA18A".ToLower();
            var muniRepresentativeId = "B0BC9D45-D406-4DD0-8230-08DB8E6DA18A".ToLower();
            var mockFormalPostDtos = new List<ExportFormalPostDto>
            {
                new ExportFormalPostDto
                {

                    Id = userId,
                    Username = "FormalUser",
                    Text = "FormalText",
                    CreatedAt = DateTime.UtcNow.ToString(),
                    UpVotesCount = 0,
                    IsUpVoted = false,
                    Comments = new HashSet<ExportCommentDto>(),
                }
            };

            _mockMapper.Setup(m => m.Map<ICollection<ExportFormalPostDto>>(It.IsAny<object>(), It.IsAny<Action<IMappingOperationOptions<object, ICollection<ExportFormalPostDto>>>>()))
            .Returns(mockFormalPostDtos);

            // Seed data to in-memory database
            SeedTestData(municipalityId, userId, muniRepresentativeId);  // You might need to adjust SeedTestData or create another method if you have a different setup for formal posts

            // Act
            var result = await _service.GetFormalPostsByMunicipalityIdAsync(municipalityId, userId);

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(1, result.Count);  // Adjust based on expected data
        }
    }
}
