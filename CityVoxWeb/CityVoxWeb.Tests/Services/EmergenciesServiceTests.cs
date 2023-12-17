using AutoMapper;
using CityVoxWeb.Data;
using CityVoxWeb.Services.Interfaces;
using CityVoxWeb.Services.Issue_Services;
using CityVoxWeb.Services.MappingProfiles.Issue_Profiles;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityVoxWeb.Tests.Services
{
    public class EmergenciesServiceTests
    {
        private CityVoxDbContext _dbContext;
        private IMapper _mapper;
        private EmergenciesService _emergenciesService;
        private Mock<INotificationService> _notificationService;

        public EmergenciesServiceTests()
        {

            var options = new DbContextOptionsBuilder<CityVoxDbContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;

            _dbContext = new CityVoxDbContext(options);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<EmergencyProfile>(); // actual AutoMapper profile
            });
            _mapper = config.CreateMapper();
            _notificationService = new Mock<INotificationService>();
            _emergenciesService = new EmergenciesService(_dbContext, _mapper , _notificationService.Object);
        }
    }
}
