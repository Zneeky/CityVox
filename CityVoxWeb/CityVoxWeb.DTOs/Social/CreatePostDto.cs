using System.ComponentModel.DataAnnotations;
using static CityVoxWeb.Common.EntityValidationConstants.PostValidations;

namespace CityVoxWeb.DTOs.Social
{
    public class CreatePostDto
    {
        [Required]
        [MaxLength(TextMaxLength)]
        public string Text { get; set; } = null!;

        [MaxLength(ImageUrlsMaxLength)]
        public string? ImageUrls { get; set; } = null!; //"url1;url2;url3;...;url8;";

        public int PostType { get; set; }

        [Required]
        public string IssueId { get; set; } = null!;

        [Required]
        public string UserId { get; set; } = null!;
    }
}
