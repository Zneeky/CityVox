using CityVoxWeb.Data.Models.IssueEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityVoxWeb.Data.Configurations
{
    public class ReportConfiguration:IEntityTypeConfiguration<Report>
    {
        public void Configure(EntityTypeBuilder<Report> builder)
        {
            builder.HasOne(r => r.User)
                   .WithMany(u => u.Reports)
                   .HasForeignKey(r => r.UserId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(r => r.Municipality)
                   .WithMany(m => m.Reports)
                   .HasForeignKey(r => r.MunicipalityId)
                   .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
