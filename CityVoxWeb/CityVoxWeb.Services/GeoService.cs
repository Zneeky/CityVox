using AutoMapper;
using CityVoxWeb.Data;
using CityVoxWeb.DTOs.Geo;
using CityVoxWeb.Services.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace CityVoxWeb.Services
{
    public class GeoService : IGeoService
    {
        private readonly CityVoxDbContext _dbContext;
        private readonly IMapper _mapper;

        public GeoService(CityVoxDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ICollection<MunicipalityExportDto>> GetMunicipalitiesByRegionIdAsync(string RegionId)
        {
            var municipalities = await _dbContext.Municipalities.Where(m => m.RegionId.ToString() == RegionId).ToListAsync();

            var municipalitiesExportDto = new List<MunicipalityExportDto>();
            foreach (var municipality in municipalities)
            {
                var municipalityDto = _mapper.Map<MunicipalityExportDto>(municipality);
                municipalitiesExportDto.Add(municipalityDto);
            }
            return municipalitiesExportDto;

        }

        public async Task<ICollection<RegionExportDto>> GetRegionsAsync()
        {
            var regions = await _dbContext.Regions.ToListAsync();

            var regionsExportDto = new List<RegionExportDto>();
            foreach (var region in regions)
            {
                var regionDto = _mapper.Map<RegionExportDto>(region);
                regionsExportDto.Add(regionDto);
            }

            return regionsExportDto;
        }
    }
}
