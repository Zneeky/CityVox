using CityVoxWeb.Data.Models.GeoEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityVoxWeb.Data.Configurations
{
    public class RegionConfiguration : IEntityTypeConfiguration<Region>
    {
        public void Configure(EntityTypeBuilder<Region> builder)
        {
            builder.HasData(
                new Region
                {
                    Id=Guid.Parse("D0282A05-BDB5-4B8D-B90C-AB62A3543ED8"),
                    RegionName = "Sofia",
                    OpenStreetMapCode = "4283101",
                }
                
           );
        }
    }
}
