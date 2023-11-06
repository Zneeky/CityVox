using CityVoxWeb.Data.Models.IssueEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityVoxWeb.Data.Configurations
{
    public class EmergencyConfiguration : IEntityTypeConfiguration<Emergency>
    {
        public void Configure(EntityTypeBuilder<Emergency> builder)
        {
            builder.HasOne(e => e.User)
                   .WithMany(u => u.Emergencies)
                   .HasForeignKey(e => e.UserId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(e => e.Municipality)
                   .WithMany(m => m.Emergencies)
                   .HasForeignKey(e => e.MunicipalityId)
                   .OnDelete(DeleteBehavior.NoAction);

            /*builder.HasOne(e => e.Post)
                   .WithOne(p => p.Emergency)
                   .HasForeignKey<Emergency>(e => e.PostId)
                   .OnDelete(DeleteBehavior.NoAction);*/
        }
    }
}
