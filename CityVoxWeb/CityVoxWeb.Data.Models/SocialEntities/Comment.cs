using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CityVoxWeb.Data.Models.UserEntities;
using static CityVoxWeb.Common.EntityValidationConstants.CommentValidations;

namespace CityVoxWeb.Data.Models.SocialEntities
{
    public class Comment
    {

        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(TextMaxLength)]
        public string Text { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public virtual ApplicationUser User { get; set; } = null!;

        [ForeignKey(nameof(Post))]
        public Guid PostId { get; set; }
        public virtual Post Post { get; set; } = null!;

        public bool Edited { get; set; }
    }
}
