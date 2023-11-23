using CityVoxWeb.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityVoxWeb.API.Controllers
{
    [Route("api/map")]
    [ApiController]
    [Authorize]
    public class MapController : ControllerBase
    {
        private readonly IGeoService _geoService;
        public MapController(IGeoService geoService)
        {
            _geoService = geoService;
        }

        [HttpGet("regions")]
        public async Task<IActionResult> GetRegions()
        {
            var regions = await _geoService.GetRegionsAsync();
            return Ok(regions);
        }

        [HttpGet("municipalities/{regionId}")]
        public async Task<IActionResult> GetMunicipalitiesByRegionId(string regionId)
        {
            try
            {
                var municipalities = await _geoService.GetMunicipalitiesByRegionIdAsync(regionId);
                return Ok(municipalities);

            }
            catch
            {
                return BadRequest("Invalid region id!");
            }
        }
    }
}
