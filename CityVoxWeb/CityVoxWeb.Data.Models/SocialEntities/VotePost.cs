using CityVoxWeb.Data.Models.UserEntities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CityVoxWeb.Data.Models.SocialEntities
{
    public class VotePost
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey(nameof(Post))]
        public Guid PostId { get; set; }
        public virtual Post Post { get; set; } = null!;

        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public virtual ApplicationUser User { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public bool IsUpvote { get; set; }  // true for upvote, false for downvote
    }
}
