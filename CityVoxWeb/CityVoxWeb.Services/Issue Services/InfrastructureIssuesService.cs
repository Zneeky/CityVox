using CityVoxWeb.DTOs.Issues.InfIssues;
using CityVoxWeb.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityVoxWeb.Services.Issue_Services
{
    internal class InfrastructureIssuesService : IGenericIssuesService<CreateInfIssueDto, ExportInfIssueDto, UpdateInfIssueDto>
    {
        public Task<ExportInfIssueDto> CreateAsync(CreateInfIssueDto createDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ExportInfIssueDto> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<ExportInfIssueDto>> GetByMunicipalityAsync(string municipalityId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<ExportInfIssueDto>> GetByUserIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<ExportInfIssueDto>> GetRequestsAsync(int page, int count)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetRequestsCountAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ExportInfIssueDto> UpdateAsync(string id, UpdateInfIssueDto updateDto)
        {
            throw new NotImplementedException();
        }
    }
}
