using CityVoxWeb.Data.Models.IssueEntities;
using CityVoxWeb.Data.Models.UserEntities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static CityVoxWeb.Common.EntityValidationConstants.MunicipalityValidations;


namespace CityVoxWeb.Data.Models.GeoEntities
{
    public class Municipality
    {
        public Municipality()
        {
            this.Reports  =new HashSet<Report>();
            this.Emergencies  = new HashSet<Emergency>();
            this.InfrastructureIssues  = new HashSet<InfrastructureIssue>();
            this.Events  = new HashSet<Event>();
            this.MunicipalityRepresentatives  = new HashSet<MunicipalityRepresentative>();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(MunicipalityNameMaxLength)]
        public string MunicipalityName { get; set; } = null!;

        [Required]
        [MaxLength(OpenStreetMapCodeMaxLength)]
        public string OpenStreetMapCode { get; set; } = null!;

        [ForeignKey(nameof(Region))]
        public Guid RegionId { get; set; }
        public virtual Region Region { get; set; } = null!;

        public ICollection<Report> Reports { get; set; } = null!;
        public ICollection<Emergency> Emergencies { get; set; } = null!;
        public ICollection<InfrastructureIssue> InfrastructureIssues { get; set; } = null!;
        public ICollection<Event> Events { get; set; } = null!;
        public ICollection<MunicipalityRepresentative> MunicipalityRepresentatives { get; set; } = null!;
    }
}
