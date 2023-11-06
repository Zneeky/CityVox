using CityVoxWeb.Data.Models.GeoEntities;
using CityVoxWeb.Data.Models.IssueEntities;
using CityVoxWeb.Data.Models.SocialEntities;
using CityVoxWeb.Data.Models;
using CityVoxWeb.Data.Models.UserEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CityVoxWeb.Data
{
    public class CityVoxDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public CityVoxDbContext(DbContextOptions<CityVoxDbContext> options)
           : base(options)
        {
        }

        public DbSet<Report> Reports { get; set; } = null!;
        public DbSet<InfrastructureIssue> InfrastructureIssues { get; set; } = null!;
        public DbSet<Emergency> Emergencies { get; set; } = null!;
        public DbSet<Notification> Notifications { get; set; } = null!;
        public DbSet<Municipality> Municipalities { get; set; } = null!;
        public DbSet<Region> Regions { get; set; } = null!;
        public DbSet<MunicipalityRepresentative> MunicipalityRepresentatives { get; set; } = null!;
        public DbSet<Event> Events { get; set; } = null!;
        public DbSet<Post> Posts { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;
        public DbSet<VotePost> PostsVotes { get; set; } = null!;
        public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

    }
}