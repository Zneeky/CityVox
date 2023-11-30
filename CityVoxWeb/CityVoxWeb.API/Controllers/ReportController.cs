using CityVoxWeb.DTOs.Issues.Reports;
using CityVoxWeb.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityVoxWeb.API.Controllers
{
    [Route("api/reports")]
    [ApiController]
    [Authorize]
    public class ReportController : ControllerBase
    {
        private readonly IGenericIssuesService<CreateReportDto, ExportReportDto, UpdateReportDto> _reportService;
        public ReportController(IGenericIssuesService<CreateReportDto, ExportReportDto, UpdateReportDto> reportService)
        {
            _reportService = reportService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReport([FromBody] CreateReportDto createReportDto)
        {
            var report = await _reportService.CreateAsync(createReportDto);

            return Ok(report.Id);
        }

        [HttpGet("{reportId}")]
        public async Task<IActionResult> GetReport(string reportId)
        {
            var report = await _reportService.GetByIdAsync(reportId);

            return Ok(report);
        }

        [HttpPatch("{reportId}")]
        public async Task<IActionResult> UpdateReport(string reportId, [FromBody] UpdateReportDto updateReportDto)
        {
            var report = await _reportService.UpdateAsync(reportId, updateReportDto);

            return Ok(report);
        }

        [HttpDelete("{reportId}")]
        public async Task<IActionResult> DeleteReport(string reportId)
        {
            await _reportService.DeleteAsync(reportId);

            return Ok();
        }

        [HttpGet("municipalities/{municipalityId}")]
        public async Task<IActionResult> GetReportsByMunicipality(string municipalityId)
        {
            var reports = await _reportService.GetByMunicipalityAsync(municipalityId);

            return Ok(reports);
        }

        [HttpGet("requests")]
        public async Task<IActionResult> GetReportRequests(int page, int count)
        {
            var reports = await _reportService.GetRequestsAsync(page, count);

            return Ok(reports);
        }

        [HttpGet("requests/count")]
        public async Task<IActionResult> GetReportRequestsCount()
        {
            var reportsCount = await _reportService.GetRequestsCountAsync();

            return Ok(reportsCount);
        }

        [HttpGet("valid/users/{userId}")]
        public async Task<IActionResult> GetReportsByUser(string userId)
        {
            var reports = await _reportService.GetByUserIdAsync(userId);

            return Ok(reports);
        }

        [HttpPost("submission")]
        public async Task<IActionResult> SubmitReportToSofiaCall([FromBody] ExportReportDto exportReportDto)
        {

            return Ok();
        }
    }
}
