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
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder
                .HasOne(e => e.User)
                .WithMany(u => u.Events)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
               .HasOne(e => e.Municipality)
               .WithMany(u => u.Events)
               .HasForeignKey(e => e.MunicipalityId)
               .OnDelete(DeleteBehavior.NoAction);

            /*builder.HasOne(e => e.Post)
                   .WithOne(p => p.Event)
                   .HasForeignKey<Event>(e => e.PostId)
                   .OnDelete(DeleteBehavior.NoAction);*/
        }
    }
}
