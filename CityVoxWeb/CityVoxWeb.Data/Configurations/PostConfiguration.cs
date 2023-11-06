using CityVoxWeb.Data.Models.SocialEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace CityVoxWeb.Data.Configurations
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder
                .HasOne(p => p.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(p => p.Report)
                .WithOne(r => r.Post)
                .HasForeignKey<Post>(p=>p.ReportId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(p => p.Event)
                .WithOne(e => e.Post)
                .HasForeignKey<Post>(p => p.EventId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(p => p.InfrastructureIssue)
                .WithOne(i => i.Post)
                .HasForeignKey<Post>(p => p.InfrastructureIssueId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(p => p.Emergency)
                .WithOne(e => e.Post)
                .HasForeignKey<Post>(p => p.EmergencyId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasMany(p => p.Comments)
                .WithOne(c => c.Post)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(p => p.Votes)
                .WithOne(v => v.Post)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
