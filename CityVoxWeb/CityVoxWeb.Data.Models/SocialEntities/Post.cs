using CityVoxWeb.Data.Models.IssueEntities;
using CityVoxWeb.Data.Models.SocialEntities.Enumerators;
using CityVoxWeb.Data.Models.UserEntities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static CityVoxWeb.Common.EntityValidationConstants.PostValidations;

namespace CityVoxWeb.Data.Models.SocialEntities
{
    public class Post
    {
        public Post()
        {
            this.Votes = new HashSet<VotePost>();
            this.Comments = new HashSet<Comment>();
        }
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(TextMaxLength)]
        public string Text { get; set; } = null!;

        [MaxLength(ImageUrlsMaxLength)]
        public string? ImageUrls { get; set; } = null!; //"url1;url2;url3;...;url8;";

        public DateTime CreatedAt { get; set; }

        public PostType PostType { get; set; }

        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public virtual ApplicationUser User { get; set; } = null!;

        [ForeignKey(nameof(Report))]
        public Guid? ReportId { get; set; }
        public virtual Report? Report { get; set; }

        [ForeignKey(nameof(Emergency))]
        public Guid? EmergencyId { get; set; }
        public virtual Emergency? Emergency { get; set; }
        
        [ForeignKey(nameof(Event))]
        public Guid? EventId { get; set; }
        public virtual Event? Event { get; set; }

        [ForeignKey(nameof(InfrastructureIssue))]
        public Guid? InfrastructureIssueId { get; set; }
        public virtual InfrastructureIssue? InfrastructureIssue { get; set; }

        public virtual ICollection<VotePost> Votes { get; set; } = null!; //UpVotes and DownVotes

        public virtual ICollection<Comment> Comments { get; set; } = null!;
    }
}
