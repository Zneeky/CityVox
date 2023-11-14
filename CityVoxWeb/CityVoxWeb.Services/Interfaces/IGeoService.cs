using CityVoxWeb.DTOs.Geo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityVoxWeb.Services.Interfaces
{
    public interface IGeoService
    {
        Task<ICollection<RegionExportDto>> GetRegionsAsync();

        Task<ICollection<MunicipalityExportDto>> GetMunicipalitiesByRegionIdAsync(string RegionId);
    }
}
