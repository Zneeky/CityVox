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
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder
                .HasMany(ap => ap.Reports)
                .WithOne(r => r.User)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasMany(ap => ap.Emergencies)
                .WithOne(e => e.User)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasMany(ap => ap.InfrastructureIssues)
                .WithOne(i => i.User)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasMany(ap => ap.Events)
                .WithOne(e => e.User)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasMany(ap => ap.Posts)
                .WithOne(p => p.User)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(ap => ap.Comments)
                .WithOne(c => c.User)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(ap => ap.VotePosts)
                .WithOne(c => c.User)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(ap => ap.Notifications)
                .WithOne(c => c.User)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(ap => ap.RefreshTokens)
                .WithOne(rt => rt.User)
                .OnDelete(DeleteBehavior.Cascade);


        }
    }
}
