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
    public class InfrastructureIssueConfiguration : IEntityTypeConfiguration<InfrastructureIssue>
    {
        public void Configure(EntityTypeBuilder<InfrastructureIssue> builder)
        {
            builder
                .HasOne(i => i.User)
                .WithMany(u => u.InfrastructureIssues)
                .HasForeignKey(i => i.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(i => i.Municipality)
                .WithMany(m => m.InfrastructureIssues)
                .HasForeignKey(i => i.MunicipalityId)
                .OnDelete(DeleteBehavior.NoAction);
            /*builder
                .HasOne(i => i.Post)
                .WithOne(p => p.InfrastructureIssue)
                .HasForeignKey<InfrastructureIssue>(i => i.PostId)
                .OnDelete(DeleteBehavior.NoAction);*/
        }
    }
}
