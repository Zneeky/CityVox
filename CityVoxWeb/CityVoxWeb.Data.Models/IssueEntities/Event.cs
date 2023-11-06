using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static CityVoxWeb.Common.EntityValidationConstants.IssuesValidations;
using CityVoxWeb.Data.Models.IssueEntities.Enumerators.Event;
using CityVoxWeb.Data.Models.UserEntities;
using CityVoxWeb.Data.Models.GeoEntities;
using CityVoxWeb.Data.Models.SocialEntities;

namespace CityVoxWeb.Data.Models.IssueEntities
{
    public class Event
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; } = null!;

        [MaxLength(ImageUrlMaxLength)]
        public string? ImageUrl { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public EventType EventType { get; set; } // Type of the event

        public virtual Post? Post { get; set; }

        [ForeignKey(nameof(Municipality))]
        public Guid MunicipalityId { get; set; }

        public virtual Municipality Municipality { get; set; } = null!;

        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }

        public virtual ApplicationUser User { get; set; } = null!;
    }
}
