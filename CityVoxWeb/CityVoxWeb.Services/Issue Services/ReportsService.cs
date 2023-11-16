using AutoMapper;
using CityVoxWeb.Data;
using CityVoxWeb.Data.Models.IssueEntities;
using CityVoxWeb.DTOs.Issues.Reports;
using CityVoxWeb.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityVoxWeb.Services.Issue_Services
{
    public class ReportsService : IGenericIssuesService<CreateReportDto, ExportReportDto, UpdateReportDto>
    {
        private readonly CityVoxDbContext _dbContext;
        private readonly IMapper _mapper;

        public ReportsService(CityVoxDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ExportReportDto> CreateAsync(CreateReportDto createReportDto)
        {
            try
            {
                Report report = _mapper.Map<Report>(createReportDto);
                await _dbContext.Reports.AddAsync(report);
                await _dbContext.SaveChangesAsync();

                ExportReportDto exportReportDto = _mapper.Map<ExportReportDto>(report);
                return exportReportDto;
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to create a report", ex);
            }
        }

        public async Task<ExportReportDto> UpdateAsync(string reportId, UpdateReportDto reportDto)
        {
            try
            {
                var report = await _dbContext.Reports
                    .Include(r => r.Municipality)
                    .Include(r => r.User)
                    .FirstOrDefaultAsync(r => r.Id.ToString() == reportId)
                    ?? throw new Exception("Invalid id!");

                _mapper.Map(reportDto, report);
                await _dbContext.SaveChangesAsync();

                var exportReportDto = _mapper.Map<ExportReportDto>(report);
                return exportReportDto;
            }
            catch (Exception ex)
            {
                throw new Exception("The operation concluded with an exeption!", ex);
            }
        }

        public async Task<bool> DeleteAsync(string reportId)
        {
            try
            {
                var report = await _dbContext.Reports
                    .FirstOrDefaultAsync(r => r.Id.ToString() == reportId) ?? throw new Exception("Invalid id!");

                _dbContext.Reports.Remove(report);
                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("The report was not deleted", ex);
            }
        }

        public async Task<ExportReportDto> GetByIdAsync(string reportId)
        {
            try
            {
                var report = await _dbContext.Reports
                    .Include(r => r.Municipality)
                    .Include(r => r.User)
                    .FirstOrDefaultAsync(r => r.Id.ToString() == reportId)
                    ?? throw new Exception("Invalid report Id!");

                var exportReport = _mapper.Map<ExportReportDto>(report);
                return exportReport;
            }
            catch (Exception ex)
            {
                throw new Exception("The operation concluded with an exeption!", ex);
            }
        }

        public async Task<ICollection<ExportReportDto>> GetByMunicipalityAsync(string municipalityId)
        {
            try
            {
                var reports = await _dbContext.Reports
                    .Include(r => r.Municipality)
                    .Include(r => r.User)
                    .Where(r => r.Municipality.Id.ToString() == municipalityId && r.Status != 0)
                    .ToListAsync();

                var exportReports = _mapper.Map<List<ExportReportDto>>(reports);
                return exportReports;
            }
            catch (Exception ex)
            {
                throw new Exception("Invalid municipality id!", ex);
            }
        }

        public async Task<ICollection<ExportReportDto>> GetRequestsAsync(int page, int count)
        {
            try
            {
                var notApprovedReports = await _dbContext.Reports
                    .Include(r => r.Municipality)
                    .Include(r => r.User)
                    .Where(r => r.Status == 0)
                    .Skip(page * count)
                    .Take(count)
                    .ToListAsync();

                var exportReports = _mapper.Map<List<ExportReportDto>>(notApprovedReports);
                return exportReports;
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
                var requestsCount = await _dbContext.Reports
                    .Where(r => r.Status == 0)
                    .CountAsync();

                return requestsCount;
            }
            catch (Exception ex)
            {
                throw new Exception("Action conlcuded with error", ex);
            }
        }

        public async Task<ICollection<ExportReportDto>> GetByUserIdAsync(string userId)
        {
            try
            {
                var reports = await _dbContext.Reports
                    .Include(r => r.Municipality)
                    .Include(r => r.User)
                    .Where(r => r.UserId.ToString() == userId && r.Status != 0)
                    .ToListAsync();

                var exportReports = _mapper.Map<List<ExportReportDto>>(reports);
                return exportReports;

            }
            catch (Exception ex)
            {
                throw new Exception("Action conlcuded with error!", ex);
            }
        }

    }
}
