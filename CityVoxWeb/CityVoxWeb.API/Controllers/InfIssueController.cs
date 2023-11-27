using CityVoxWeb.DTOs.Issues.InfIssues;
using CityVoxWeb.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityVoxWeb.API.Controllers
{
    [Route("api/infIssues")]
    [ApiController]
    [Authorize]
    public class InfIssueController : ControllerBase
    {
        private readonly IGenericIssuesService<CreateInfIssueDto, ExportInfIssueDto, UpdateInfIssueDto> _infIssuesService;

        public InfIssueController(IGenericIssuesService<CreateInfIssueDto, ExportInfIssueDto, UpdateInfIssueDto> infIssuesService)
        {
            _infIssuesService = infIssuesService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateInfIssue([FromBody] CreateInfIssueDto createInfIssueDto)
        {
            var infIssue = await _infIssuesService.CreateAsync(createInfIssueDto);

            return Ok(infIssue.Id);
        }

        [HttpGet("{infIssueId}")]
        public async Task<IActionResult> GetInfIssue(string infIssueId)
        {
            var infIssue = await _infIssuesService.GetByIdAsync(infIssueId);

            return Ok(infIssue);
        }

        [HttpPatch("{infIssueId}")]
        public async Task<IActionResult> UpdateInfIssue(string infIssueId, [FromBody] UpdateInfIssueDto updateInfIssueDto)
        {
            var infIssue = await _infIssuesService.UpdateAsync(infIssueId, updateInfIssueDto);

            return Ok(infIssue);
        }

        [HttpDelete("{infIssueId}")]
        public async Task<IActionResult> DeleteInfIssue(string infIssueId)
        {
            await _infIssuesService.DeleteAsync(infIssueId);

            return Ok();
        }

        [HttpGet("municipalities/{municipalityId}")]
        public async Task<IActionResult> GetInfIssuesByMunicipality(string municipalityId)
        {
            var infIssues = await _infIssuesService.GetByMunicipalityAsync(municipalityId);

            return Ok(infIssues);
        }

        [HttpGet("requests")]
        public async Task<IActionResult> GetInfIssueRequests(int page, int count)
        {
            var infIssues = await _infIssuesService.GetRequestsAsync(page, count);

            return Ok(infIssues);
        }

        [HttpGet("requests/count")]
        public async Task<IActionResult> GetInfIssueRequestsCount()
        {
            var count = await _infIssuesService.GetRequestsCountAsync();

            return Ok(count);
        }

        [HttpGet("valid/users/{userId}")]
        public async Task<IActionResult> GetReportsByUser(string userId)
        {
            var infIssues = await _infIssuesService.GetByUserIdAsync(userId);

            return Ok(infIssues);
        }
    }
}
