using CityVoxWeb.DTOs.Issues.InfIssues;
using CityVoxWeb.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityVoxWeb.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
    }
}
