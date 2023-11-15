using CityVoxWeb.DTOs.Issues.Emergencies;
using CityVoxWeb.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityVoxWeb.Services.Issue_Services
{
    public class EmergenciesService : IGenericIssuesService<CreateEmergencyDto, ExportEmergencyDto, UpdateEmergencyDto>
    {
        public Task<ExportEmergencyDto> CreateAsync(CreateEmergencyDto createDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(string id)
        {
            throw new NotImplementedException();
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

        public Task<ExportEmergencyDto> UpdateAsync(string id, UpdateEmergencyDto updateDto)
        {
            throw new NotImplementedException();
        }
    }
}
