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

        public EmergenciesService(CityVoxDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
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
                await _dbContext.SaveChangesAsync();

                var exportEmergencyDto = _mapper.Map<ExportEmergencyDto>(emergency);
                return exportEmergencyDto;
            }
            catch (Exception ex)
            {
                throw new Exception("The operation concluded with an exeption!", ex);
            }
        }

        public Task<ExportEmergencyDto> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<ExportEmergencyDto>> GetByMunicipalityAsync(string municipalityId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<ExportEmergencyDto>> GetByUserIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<ExportEmergencyDto>> GetRequestsAsync(int page, int count)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetRequestsCountAsync()
        {
            throw new NotImplementedException();
        }
    }
}
