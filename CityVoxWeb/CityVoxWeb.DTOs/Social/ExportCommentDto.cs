using System.ComponentModel.DataAnnotations;
using static CityVoxWeb.Common.EntityValidationConstants.CommentValidations;

namespace CityVoxWeb.DTOs.Social
{
    public class ExportCommentDto
    {
        public string Id { get; set; } = null!;
        [Required]
        [MaxLength(TextMaxLength)]
        public string Text { get; set; } = null!;

        public string CreatedAt { get; set; } = null!;

        public string Username { get; set; } = null!;

        public string? ProfilePictureUrl { get; set; }

        public bool Edited { get; set; }
    }
}
