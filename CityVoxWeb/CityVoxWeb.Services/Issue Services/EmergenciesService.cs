using AutoMapper;
using CityVoxWeb.Data;
using CityVoxWeb.Data.Models.IssueEntities;
using CityVoxWeb.DTOs.Issues.Emergencies;
using CityVoxWeb.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityVoxWeb.Services.Issue_Services
{
    public class EmergenciesService : IGenericIssuesService<CreateEmergencyDto, ExportEmergencyDto, UpdateEmergencyDto>
    {

        private readonly CityVoxDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;

        public EmergenciesService(CityVoxDbContext dbContext, IMapper mapper, INotificationService notificationService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _notificationService = notificationService;
        }
        public async Task<ExportEmergencyDto> CreateAsync(CreateEmergencyDto createEmergencyDto)
        {
            try
            {
                Emergency emergency = _mapper.Map<Emergency>(createEmergencyDto);
                await _dbContext.Emergencies.AddAsync(emergency);
                await _dbContext.SaveChangesAsync();

                ExportEmergencyDto exportEmergencyDto = _mapper.Map<ExportEmergencyDto>(emergency);
                return exportEmergencyDto;
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to create an emergency", ex);
            }
        }

        public async Task<bool> DeleteAsync(string emergencyId)
        {
            try
            {
                var emergency = await _dbContext.Emergencies
                    .FirstOrDefaultAsync(e => e.Id.ToString() == emergencyId) ?? throw new Exception("Invalid id!");

                _dbContext.Emergencies.Remove(emergency);
                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("The emergency was not deleted", ex);
            }
        }

        public async Task<ExportEmergencyDto> UpdateAsync(string emergencyId, UpdateEmergencyDto emergencyDto)
        {
            try
            {
                var emergency = await _dbContext.Emergencies
                    .Include(e => e.Municipality)
                    .Include(e => e.User)
                    .FirstOrDefaultAsync(e => e.Id.ToString() == emergencyId)
                    ?? throw new Exception("Invalid id!");

                _mapper.Map(emergencyDto, emergency);
                //await _dbContext.SaveChangesAsync();

                await _notificationService.CreateNotificationForEmergencyAsync(emergencyDto.Status, "emergency", emergency);
                var exportEmergencyDto = _mapper.Map<ExportEmergencyDto>(emergency);
                await _dbContext.SaveChangesAsync();
                return exportEmergencyDto;
            }
            catch (Exception ex)
            {
                throw new Exception("The operation concluded with an exeption!", ex);
            }
        }
        public async Task<ExportEmergencyDto> GetByIdAsync(string emergencyId)
        {
            try
            {
                var emergency = await _dbContext.Emergencies
                    .Include(e => e.Municipality)
                    .Include(e => e.User)
                    .FirstOrDefaultAsync(e => e.Id.ToString() == emergencyId)
                    ?? throw new Exception("Invalid emergency Id!");

                var exportEmergency = _mapper.Map<ExportEmergencyDto>(emergency);
                return exportEmergency;
            }
            catch (Exception ex)
            {
                throw new Exception("The operation concluded with an exeption!", ex);
            }
        }
        public async Task<ICollection<ExportEmergencyDto>> GetByMunicipalityAsync(string municipalityId)
        {
            try
            {
                var emergencies = await _dbContext.Emergencies
                    .Include(e => e.Municipality)
                    .Include(e => e.User)
                    .Where(e => e.Municipality.Id.ToString() == municipalityId && e.Status != 0)
                    .ToListAsync();

                var exportEmergencies = _mapper.Map<List<ExportEmergencyDto>>(emergencies);
                return exportEmergencies;
            }
            catch (Exception ex)
            {
                throw new Exception("Invalid municipality id!", ex);
            }
        }
        public async Task<ICollection<ExportEmergencyDto>> GetRequestsAsync(int page, int count)
        {
            try
            {
                var notApprovedEmergencies = await _dbContext.Emergencies
                    .Include(e => e.Municipality)
                    .Include(e => e.User)
                    .Where(e => e.Status == 0)
                    .Skip(page * count)
                    .Take(count)
                    .ToListAsync();

                var exportEmergencies = _mapper.Map<List<ExportEmergencyDto>>(notApprovedEmergencies);
                return exportEmergencies;
            }
            catch (Exception ex)
            {
                throw new Exception("Error!", ex);
            }
        }
        public async Task<int> GetRequestsCountAsync()
        {
            try
            {
                var requestCount = await _dbContext.Emergencies
                    .Where(e => e.Status == 0)
                    .CountAsync();

                return requestCount;
            }
            catch (Exception ex)
            {
                throw new Exception("Error!", ex);
            }
        }
        public async Task<ICollection<ExportEmergencyDto>> GetByUserIdAsync(string userId)
        {
            try
            {
                var emergencies = await _dbContext.Emergencies
                    .Include(e => e.Municipality)
                    .Include(e => e.User)
                    .Where(e => e.UserId.ToString() == userId && e.Status != 0)
                    .ToListAsync();

                var exportEmergencies = _mapper.Map<List<ExportEmergencyDto>>(emergencies);
                return exportEmergencies;

            }
            catch (Exception ex)
            {
                throw new Exception("Action conlcuded with error!", ex);
            }
        }
    }
}
