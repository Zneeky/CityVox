using CityVoxWeb.Data.Models.UserEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityVoxWeb.Data.Configurations
{
    public class MunicipalityRepresentativeConfiguration : IEntityTypeConfiguration<MunicipalityRepresentative>
    {
        public void Configure(EntityTypeBuilder<MunicipalityRepresentative> builder)
        {
            builder
                .HasOne(mr => mr.User)
                .WithOne()
                .HasForeignKey<MunicipalityRepresentative>(mr => mr.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(mr => mr.Municipality)
                .WithMany(m=> m.MunicipalityRepresentatives)
                .HasForeignKey(mr => mr.MunicipalityId)
                .OnDelete(DeleteBehavior.NoAction);
        }



    }
}
