using AutoMapper;
using CityVoxWeb.Data;
using CityVoxWeb.Data.Models.IssueEntities;
using CityVoxWeb.DTOs.Issues.InfIssues;
using CityVoxWeb.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityVoxWeb.Services.Issue_Services
{
    public class InfrastructureIssuesService : IGenericIssuesService<CreateInfIssueDto, ExportInfIssueDto, UpdateInfIssueDto>
    {

        private readonly CityVoxDbContext _dbContext;
        private readonly IMapper _mapper;

        public InfrastructureIssuesService(CityVoxDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<ExportInfIssueDto> CreateAsync(CreateInfIssueDto createInfIssueDto)
        {
            try
            {
                InfrastructureIssue infIssue = _mapper.Map<InfrastructureIssue>(createInfIssueDto);
                await _dbContext.InfrastructureIssues.AddAsync(infIssue);
                await _dbContext.SaveChangesAsync();

                ExportInfIssueDto exportInfIssueDto = _mapper.Map<ExportInfIssueDto>(infIssue);
                return exportInfIssueDto;
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to create an infrastructure issue", ex);
            }
        }

        public async Task<ExportInfIssueDto> UpdateAsync(string infIssueId, UpdateInfIssueDto updateInfIssueDto)
        {
            try
            {
                var infIssue = await _dbContext.InfrastructureIssues
                    .Include(i => i.Municipality)
                    .Include(i => i.User)
                    .FirstOrDefaultAsync(i => i.Id.ToString() == infIssueId)
                    ?? throw new Exception("Invalid id!");

                _mapper.Map(updateInfIssueDto, infIssue);
                await _dbContext.SaveChangesAsync();

                var exportInfIssueDto = _mapper.Map<ExportInfIssueDto>(infIssue);
                return exportInfIssueDto;
            }
            catch (Exception ex)
            {
                throw new Exception("The operation concluded with an exeption!", ex);
            }
        }
        public async Task<bool> DeleteAsync(string infIssueId)
        {
            try
            {
                var infIssue = await _dbContext.InfrastructureIssues
                    .FirstOrDefaultAsync(i => i.Id.ToString() == infIssueId) ?? throw new Exception("Invalid id!");

                _dbContext.InfrastructureIssues.Remove(infIssue);
                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("The infrastructure issue could not be deleted!", ex);
            }
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

        
    }
}
