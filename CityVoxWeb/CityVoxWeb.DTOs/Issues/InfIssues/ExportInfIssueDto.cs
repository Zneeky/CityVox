using System.ComponentModel.DataAnnotations;
using static CityVoxWeb.Common.EntityValidationConstants.ApplicationUserValidations;
using static CityVoxWeb.Common.EntityValidationConstants.IssuesValidations;

namespace CityVoxWeb.DTOs.Issues.InfIssues
{
    public class ExportInfIssueDto
    {
        [Required]
        [MinLength(UsernameMinLength)]
        [MaxLength(UsernameMaxLength)]
        public string CreatorUsername { get; set; } = null!;

        [Required]
        public string Id { get; set; } = null!;

        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; } = null!;

        public string ReportTime { get; set; } = null!;

        public string? ResolvedTime { get; set; }

        [MaxLength(ImageUrlMaxLength)]
        public string? ImageUrl { get; set; }

        [Required]
        [Range(LatitudeMinRange, LatitudeMaxRange)]
        public double Latitude { get; set; }

        [Required]
        [Range(LongitudeMinRange, LongitudeMaxRange)]
        public double Longitude { get; set; }

        [MaxLength(AddressMaxLength)]
        public string? Address { get; set; }

        public string Municipality { get; set; } = null!;

        public string Type { get; set; } = null!;

        public string Status { get; set; } = null!;

        public int TypeValue { get; set; }

        public int StatusValue { get; set; }

        public string Represent { get; set; } = null!;
    }
}
