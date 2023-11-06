using CityVoxWeb.Data.Models.GeoEntities;
using CityVoxWeb.Data.Models.UserEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityVoxWeb.Data.Configurations
{
    public class IdentityRoleConfiguration : IEntityTypeConfiguration<IdentityRole<Guid>>
    {
        public void Configure(EntityTypeBuilder<IdentityRole<Guid>> builder)
        {
            builder.HasData(
               new IdentityRole<Guid>
               {
                   Id = Guid.Parse("99ae3bb9-5e34-42ce-92e0-ea07d45fe244"),
                   Name = "Admin",
                   NormalizedName = "ADMIN"
               },
               new IdentityRole<Guid>
               {
                    Id = Guid.Parse("446c78ea-0251-4b2c-809f-6f30ba8afcf9"),
                    Name = "User",
                    NormalizedName = "USER"
               },
               new IdentityRole<Guid>
               {
                   Id = Guid.Parse("f86b1ef8-08cd-49a0-8112-18ffb9fea577"),
                   Name = "Representative",
                   NormalizedName = "REPRESENTATIVE"
               }

          );
        }
    }
}
