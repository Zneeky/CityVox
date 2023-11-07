using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityVoxWeb.DTOs.Geo
{
    public  class MunicipalityExportDto
    {
        public string Id { get; set; } = null!;
        public string MunicipalityName { get; set; } = null!;
        public string OpenStreetMapCode { get; set; } = null!;
        public string RegionId { get; set; } = null!;
    }
}
