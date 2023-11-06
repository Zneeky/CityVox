using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static CityVoxWeb.Common.EntityValidationConstants.IssuesValidations;
using CityVoxWeb.Data.Models.IssueEntities.Enumerators.Report;
using CityVoxWeb.Data.Models.UserEntities;
using CityVoxWeb.Data.Models.GeoEntities;
using CityVoxWeb.Data.Models.SocialEntities;

namespace CityVoxWeb.Data.Models.IssueEntities
{
    public class Report
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; } = null!;

        public DateTime ReportTime { get; set; }

        public DateTime? ResolvedTime { get; set; }

        public DateTime DueBy { get; set; }

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

        public virtual Post? Post { get; set; }

        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public virtual ApplicationUser User { get; set; }


        [ForeignKey(nameof(Municipality))]
        public Guid MunicipalityId { get; set; }
        public virtual Municipality Municipality { get; set; } = null!;

        public ReportType Type { get; set; }

        public ReportStatus Status { get; set; }
    }
}
