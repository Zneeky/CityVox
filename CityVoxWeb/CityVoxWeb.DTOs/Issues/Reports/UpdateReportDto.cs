using System.ComponentModel.DataAnnotations;
using static CityVoxWeb.Common.EntityValidationConstants.ApplicationUserValidations;
using static CityVoxWeb.Common.EntityValidationConstants.IssuesValidations;

namespace CityVoxWeb.DTOs.Issues.Reports
{
    public class UpdateReportDto
    {
        [Required]
        [MinLength(TitleMinLength)]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; } = null!;

        [Required]
        [MinLength(DescriptionMinLength)]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; } = null!;

        [MaxLength(ImageUrlMaxLength)]
        public string? ImageUrl { get; set; }

        public int Type { get; set; }

        public int Status { get; set; }
    }
}
