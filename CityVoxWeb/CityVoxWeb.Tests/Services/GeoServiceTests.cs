using AutoMapper;
using CityVoxWeb.Data;
using CityVoxWeb.Data.Models.GeoEntities;
using CityVoxWeb.Services;
using CityVoxWeb.Services.Mapping_Profiles;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CityVoxWeb.Tests.Services
{
    public class GeoServiceTests
    {
        private CityVoxDbContext _dbContext;
        private IMapper _mapper;
        private GeoService _geoService;


        public GeoServiceTests()
        {
            var options = new DbContextOptionsBuilder<CityVoxDbContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;

            _dbContext = new CityVoxDbContext(options);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<GeoProfile>(); // actual AutoMapper profile
            });

            _mapper = config.CreateMapper();
            _geoService = new GeoService(_dbContext, _mapper);
        }

        [Fact]
        public async Task GetMunicipalitiesByRegionIdAsync_GivenValidRegionId_ShouldReturnMunicipalitiesForThatRegion()
        {
            // Arrange
            Guid regionId = Guid.NewGuid();

            var municipalities = new List<Municipality>
            {
                new Municipality { Id = Guid.NewGuid(), MunicipalityName = "Municipality1", RegionId = regionId , OpenStreetMapCode = "3759432", },
                new Municipality { Id = Guid.NewGuid(), MunicipalityName = "Municipality2", RegionId = regionId , OpenStreetMapCode = "3259439",},
                new Municipality { Id = Guid.NewGuid(), MunicipalityName = "Municipality3", RegionId = Guid.NewGuid() , OpenStreetMapCode = "4759439", }  // This one has a different region Id
            };

            await _dbContext.Municipalities.AddRangeAsync(municipalities);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _geoService.GetMunicipalitiesByRegionIdAsync(regionId.ToString());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetRegionsAsync_ShouldReturnAllRegions()
        {
            // Arrange
            var regions = new List<Region>
            {
                new Region { Id = Guid.NewGuid(), RegionName = "Region1" , OpenStreetMapCode = "231231" },
                new Region { Id = Guid.NewGuid(), RegionName = "Region2" , OpenStreetMapCode = "223231" },
            };

            await _dbContext.Regions.AddRangeAsync(regions);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _geoService.GetRegionsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);

        }
    }
}
