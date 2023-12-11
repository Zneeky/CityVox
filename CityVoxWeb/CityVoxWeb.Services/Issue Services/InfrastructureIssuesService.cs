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
        private readonly INotificationService _notificationService;

        public InfrastructureIssuesService(CityVoxDbContext dbContext, IMapper mapper, INotificationService notificationService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _notificationService = notificationService;
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

                await _notificationService.CreateNotificationForInfrastructureIssueAsync(updateInfIssueDto.Status, "infIssue", infIssue);
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

        public async Task<ExportInfIssueDto> GetByIdAsync(string infIssueId)
        {
            try
            {
                var infIssue = await _dbContext.InfrastructureIssues
                    .Include(i => i.Municipality)
                    .Include(i => i.User)
                    .FirstOrDefaultAsync(i => i.Id.ToString() == infIssueId)
                    ?? throw new Exception("Invalid report Id!");

                var exportInfIssueDto = _mapper.Map<ExportInfIssueDto>(infIssue);
                return exportInfIssueDto;
            }
            catch (Exception ex)
            {
                throw new Exception("The operation concluded with an exeption!", ex);
            }
        }

        public async Task<ICollection<ExportInfIssueDto>> GetByMunicipalityAsync(string municipalityId)
        {
            try
            {
                var infIssues = await _dbContext.InfrastructureIssues
                    .Include(i => i.Municipality)
                    .Include(i => i.User)
                    .Where(i => i.Municipality.Id.ToString() == municipalityId && i.Status != 0)
                    .ToListAsync();

                var exportInfIssues = _mapper.Map<List<ExportInfIssueDto>>(infIssues);
                return exportInfIssues;
            }
            catch (Exception ex)
            {
                throw new Exception("Invalid municipality id!", ex);
            }
        }

        public async Task<ICollection<ExportInfIssueDto>> GetRequestsAsync(int page, int count)
        {
            try
            {
                var notApprovedInfIssues = await _dbContext.InfrastructureIssues
                    .Include(i => i.Municipality)
                    .Include(i => i.User)
                    .Where(i => i.Status == 0)
                    .Skip(page * count)
                    .Take(count)
                    .ToListAsync();

                var exportInfIssues = _mapper.Map<List<ExportInfIssueDto>>(notApprovedInfIssues);
                return exportInfIssues;
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
                var requestCount = await _dbContext.InfrastructureIssues
                    .Where(e => e.Status == 0)
                    .CountAsync();

                return requestCount;
            }
            catch (Exception ex)
            {
                throw new Exception("Error!", ex);
            }
        }

        public async Task<ICollection<ExportInfIssueDto>> GetByUserIdAsync(string userId)
        {
            try
            {
                var infIssues = await _dbContext.InfrastructureIssues
                    .Include(i => i.Municipality)
                    .Include(i => i.User)
                    .Where(i => i.UserId.ToString() == userId && i.Status != 0)
                    .ToListAsync();

                var exportInfIssues = _mapper.Map<List<ExportInfIssueDto>>(infIssues);
                return exportInfIssues;

            }
            catch (Exception ex)
            {
                throw new Exception("Action conlcuded with error!", ex);
            }
        }
    }
}
