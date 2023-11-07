using System.ComponentModel.DataAnnotations;
using static CityVoxWeb.Common.EntityValidationConstants.PostValidations;

namespace CityVoxWeb.DTOs.Social
{
    public class ExportPostDto
    {
        public ExportPostDto()
        {
            this.Comments = new HashSet<ExportCommentDto>();
        }
        public string Id { get; set; } = null!;
        public string? IssueId { get; set; }
        public string Username { get; set; } = null!;

        [MaxLength(ImageUrlsMaxLength)]
        public string? ProfilePictureUrl { get; set; }

        [Required]
        [MaxLength(TextMaxLength)]
        public string Text { get; set; } = null!;

        [MaxLength(ImageUrlsMaxLength)]
        public string? ImageUrls { get; set; } = null!;

        public string CreatedAt { get; set; } = null!;

        public string PostType { get; set; } = null!;

        public int PostTypeValue { get; set; }

        public int UpVotesCount { get; set; }

        public bool IsUpVoted { get; set; }
        public ICollection<ExportCommentDto> Comments { get; set; } = null!;

    }
}
