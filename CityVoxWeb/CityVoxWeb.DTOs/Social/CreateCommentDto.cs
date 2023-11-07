using System.ComponentModel.DataAnnotations;
using static CityVoxWeb.Common.EntityValidationConstants.CommentValidations;

namespace CityVoxWeb.DTOs.Social
{
    public class CreateCommentDto
    {
        [Required]
        [MaxLength(TextMaxLength)]
        public string Text { get; set; } = null!;

        [Required]
        public string UserId { get; set; } = null!;

        [Required]
        public string PostId { get; set; } = null!;
    }
}
