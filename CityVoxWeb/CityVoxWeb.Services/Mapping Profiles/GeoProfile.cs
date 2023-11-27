using AutoMapper;
using CityVoxWeb.Data.Models.GeoEntities;
using CityVoxWeb.DTOs.Geo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityVoxWeb.Services.Mapping_Profiles
{
    public class GeoProfile : Profile
    {
        public GeoProfile()
        {
            CreateMap<Region, RegionExportDto>();
            CreateMap<Municipality, MunicipalityExportDto>();
        }
    }
}
