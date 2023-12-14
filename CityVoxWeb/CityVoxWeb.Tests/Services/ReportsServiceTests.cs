using AutoMapper;
using CityVoxWeb.Data;
using CityVoxWeb.Data.Models.GeoEntities;
using CityVoxWeb.Data.Models.IssueEntities;
using CityVoxWeb.Data.Models.IssueEntities.Enumerators.Report;
using CityVoxWeb.Data.Models.UserEntities;
using CityVoxWeb.DTOs.Issues.Reports;
using CityVoxWeb.Services.Interfaces;
using CityVoxWeb.Services.Issue_Services;
using CityVoxWeb.Services.MappingProfiles.Issue_Profiles;
using CityVoxWeb.Services.User_Services;
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
    public class ReportsServiceTests
    {
        private CityVoxDbContext _dbContext;
        private IMapper _mapper;
        private ReportsService _reportsService;
        private Mock<INotificationService> _notificationService;

        public ReportsServiceTests()
        {
            var options = new DbContextOptionsBuilder<CityVoxDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new CityVoxDbContext(options);
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ReportProfile>(); // actual AutoMapper profile
            });
            _mapper = config.CreateMapper();
            _notificationService = new Mock<INotificationService>();
            _reportsService = new ReportsService(_dbContext, _mapper, _notificationService.Object);
        }

        [Fact]
        public async Task CreateAsync_ValidReport_ShouldCreateReportAndReturnDto()
        {
            // Arrange
            var createDto = new CreateReportDto
            {
                CreatorId = Guid.NewGuid().ToString(),
                Title = "Title",
                Description = "Description",
                Latitude = 12.2,
                Longitude = 13.3,
                Address = "St Address",
                MunicipalityId = Guid.NewGuid().ToString(),
                Type = 2
            };



            // Act
            var result = await _reportsService.CreateAsync(createDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(createDto.Title, result.Title);
            Assert.Equal(createDto.Latitude, result.Latitude);
        }

        [Fact]
        public async Task UpdateAsync_ValidReport_ShouldUpdateReportAndReturnUpdatedDto()
        {
            // Arrange: seed a report into the database
            string reportId = Guid.NewGuid().ToString();
            Guid municipalityId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            var existingUser = new ApplicationUser
            {
                UserName = "Zneeky",
                Email = "zneeky@abv.bg",
                FirstName = "Arkan",
                LastName = "Ahmedov",
                ProfilePictureUrl = "https://i.imgur.com/3tj9h9X.jpg",
                PasswordHash = Guid.NewGuid().ToString(),
                SecurityStamp = Guid.NewGuid().ToString(),
                Id = userId,
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
            _dbContext.Users.Add(existingUser);

            var exisitngMuni = new Municipality
            {
                Id = municipalityId,
                MunicipalityName = "Pancharevo",
                OpenStreetMapCode = "3759439",
                RegionId = Guid.Parse("D0282A05-BDB5-4B8D-B90C-AB62A3543ED8"),
            };
            _dbContext.Municipalities.Add(exisitngMuni);

            var existingReport = new Report
            {
                Id = Guid.Parse(reportId),
                Title = "Title",
                Description = "Description",
                ReportTime = DateTime.UtcNow,
                ImageUrl = "imageUrl.jpg",
                Latitude = 12.5,
                Longitude = 13.5,
                Address = "St address",
                UserId = userId,
                MunicipalityId = municipalityId,
                Type =ReportType.Graffiti,
                Status = ReportStatus.Approved
            };
            _dbContext.Reports.Add(existingReport);
            await _dbContext.SaveChangesAsync();

            var updateDto = new UpdateReportDto
            {
                Title = "Updated Title",
                Description = "Updated Description",
                ImageUrl = null,
                Type = 0,
                Status = 1
            };


            // Act
            var result = await _reportsService.UpdateAsync(existingReport.Id.ToString(), updateDto);

            // Assert
            Assert.NotNull(result);
            // Additional assertions based on your mock data...
        }

        [Fact]
        public async Task DeleteAsync_ValidReportId_ShouldDeleteReportAndReturnTrue()
        {

            // Arrange
            string reportId = Guid.NewGuid().ToString();
            Guid municipalityId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            var existingReport = new Report
            {
                Id = Guid.Parse(reportId),
                Title = "Title",
                Description = "Description",
                ReportTime = DateTime.UtcNow,
                ImageUrl = "imageUrl.jpg",
                Latitude = 12.5,
                Longitude = 13.5,
                Address = "St address",
                UserId = userId,
                MunicipalityId = municipalityId,
                Type = ReportType.Graffiti,
                Status = ReportStatus.Approved
            };
            _dbContext.Reports.Add(existingReport);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _reportsService.DeleteAsync(existingReport.Id.ToString());

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetByMunicipalityAsync_ValidMunicipality_ShouldReturnReports()
        {
            //Arrange
            Guid userId = Guid.NewGuid();
            Guid municipalityId = Guid.NewGuid();
            Guid municipality2Id = Guid.NewGuid();

            var existingUser = new ApplicationUser
            {
                UserName = "Zneeky",
                Email = "zneeky@abv.bg",
                FirstName = "Arkan",
                LastName = "Ahmedov",
                ProfilePictureUrl = "https://i.imgur.com/3tj9h9X.jpg",
                PasswordHash = Guid.NewGuid().ToString(),
                SecurityStamp = Guid.NewGuid().ToString(),
                Id = userId,
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
            _dbContext.Users.Add(existingUser);

            var exisitngMuni = new Municipality
            {
                Id = municipalityId,
                MunicipalityName = "Pancharevo",
                OpenStreetMapCode = "3759439",
                RegionId = Guid.Parse("D0282A05-BDB5-4B8D-B90C-AB62A3543ED8"),
            };

            var exisitngMuni2 = new Municipality
            {
                Id = municipality2Id,
                MunicipalityName = "Ilinden",
                OpenStreetMapCode = "3759438",
                RegionId = Guid.Parse("D0282A05-BDB5-4B8D-B90C-AB62A3543ED8"),
            };
            _dbContext.Municipalities.AddRange(exisitngMuni, exisitngMuni2);

            var reports = new List<Report>
            {
                new Report {Id = Guid.NewGuid(), Title = "Title", Description = "Description", ImageUrl = "imageUrl.jpg", Latitude = 12.5, Longitude = 13.5, Address = "St address", UserId = userId, MunicipalityId = municipalityId, Type = ReportType.Graffiti,  Status = ReportStatus.Approved},
                new Report {Id = Guid.NewGuid(), Title = "Title2", Description = "Description2", ImageUrl = "imageUrl2.jpg", Latitude = 32.5, Longitude = 31.5, Address = "2St address", UserId = userId, MunicipalityId = municipalityId, Type = ReportType.Graffiti,  Status = ReportStatus.Approved},
                new Report {Id = Guid.NewGuid(), Title = "Title3", Description = "Description3", ImageUrl = "imageUrl3.jpg", Latitude = 32.5, Longitude = 31.5, Address = "3St address", UserId = userId, MunicipalityId = municipalityId, Type = ReportType.Graffiti,  Status = ReportStatus.Reported},
                new Report {Id = Guid.NewGuid(), Title = "Title4", Description = "Description4", ImageUrl = "imageUrl4.jpg", Latitude = 32.5, Longitude = 31.5, Address = "4St address", UserId = userId, MunicipalityId = municipality2Id, Type = ReportType.Graffiti,  Status = ReportStatus.Approved},

            };

            _dbContext.Reports.AddRange(reports);
            await _dbContext.SaveChangesAsync();

            //Act
            var result = await _reportsService.GetByMunicipalityAsync(municipalityId.ToString());

            //Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.True(result.All(r => r.Municipality == "Pancharevo"));
        }
    }
}
