using CityVoxWeb.DTOs.Issues.Emergencies;
using CityVoxWeb.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityVoxWeb.API.Controllers
{
    [Route("api/emergencies")]
    [ApiController]
    [Authorize]
    public class EmergencyController : ControllerBase
    {
        private readonly IGenericIssuesService<CreateEmergencyDto, ExportEmergencyDto, UpdateEmergencyDto> _emergencyService;

        public EmergencyController(IGenericIssuesService<CreateEmergencyDto, ExportEmergencyDto, UpdateEmergencyDto> emergencyService)
        {
            _emergencyService = emergencyService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmergency([FromBody] CreateEmergencyDto createEmergencyDto)
        {
            var emergency = await _emergencyService.CreateAsync(createEmergencyDto);

            return Ok(emergency.Id);
        }

        [HttpGet("{emergencyId}")]
        public async Task<IActionResult> GetEmergency(string emergencyId)
        {
            var emergency = await _emergencyService.GetByIdAsync(emergencyId);

            return Ok(emergency);
        }

        [HttpPatch("{emergencyId}")]
        public async Task<IActionResult> UpdateEmergency(string emergencyId, [FromBody] UpdateEmergencyDto updateEmergencyDto)
        {
            var emergency = await _emergencyService.UpdateAsync(emergencyId, updateEmergencyDto);

            return Ok(emergency);
        }

        [HttpDelete("{emergencyId}")]
        public async Task<IActionResult> DeleteEmergency(string emergencyId)
        {
            await _emergencyService.DeleteAsync(emergencyId);

            return Ok();
        }

        [HttpGet("municipalities/{municipalityId}")]
        public async Task<IActionResult> GetEmergenciesByMunicipality(string municipalityId)
        {
            var emergencies = await _emergencyService.GetByMunicipalityAsync(municipalityId);

            return Ok(emergencies);
        }

        [HttpGet("requests")]
        public async Task<IActionResult> GetEmergencyRequests(int page, int count)
        {
            var emergencies = await _emergencyService.GetRequestsAsync(page, count);

            return Ok(emergencies);
        }

        [HttpGet("requests/count")]
        public async Task<IActionResult> GetEmergencyRequestsCount()
        {
            var count = await _emergencyService.GetRequestsCountAsync();

            return Ok(count);
        }

        [HttpGet("valid/users/{userId}")]
        public async Task<IActionResult> GetReportsByUser(string userId)
        {
            var infIssues = await _emergencyService.GetByUserIdAsync(userId);

            return Ok(infIssues);
        }
    }
}
