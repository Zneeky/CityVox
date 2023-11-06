using CityVoxWeb.Data.Models.IssueEntities;
using CityVoxWeb.Data.Models.SocialEntities;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using static CityVoxWeb.Common.EntityValidationConstants.ApplicationUserValidations;

namespace CityVoxWeb.Data.Models.UserEntities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ApplicationUser() : base()
        {
            this.Reports = new HashSet<Report>();
            this.Emergencies = new HashSet<Emergency>();
            this.InfrastructureIssues = new HashSet<InfrastructureIssue>();
            this.Events = new HashSet<Event>();

            this.Notifications = new HashSet<Notification>();
            this.Posts = new HashSet<Post>();
            this.Comments = new HashSet<Comment>();
            this.VotePosts = new HashSet<VotePost>();

            this.RefreshTokens = new HashSet<RefreshToken>();
        }

        [Required]
        [MaxLength(FirstNameMaxLength)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(LastNameMaxLength)]
        public string LastName { get; set; } = null!;

        [MaxLength(ProfilePictureUrlMaxLength)]
        public string? ProfilePictureUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastLoginTime { get; set; }

        public virtual ICollection<Report> Reports { get; set; } = null!;
        public virtual ICollection<Emergency> Emergencies { get; set; } = null!;
        public virtual ICollection<InfrastructureIssue> InfrastructureIssues { get; set; } = null!;
        public virtual ICollection<Event> Events { get; set; } = null!;

        // For keeping track of notifications that have been sent to the user
        public virtual ICollection<Notification> Notifications { get; set; } = null!;

        //Social nav props
        public virtual ICollection<Post> Posts { get; set; } = null!;
        public virtual ICollection<Comment> Comments { get; set; } = null!;
        public virtual ICollection<VotePost> VotePosts { get; set; } = null!;

        //Token System
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = null!;
    }
}
