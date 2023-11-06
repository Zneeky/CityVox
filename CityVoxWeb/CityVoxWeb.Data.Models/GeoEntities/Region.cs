using System.ComponentModel.DataAnnotations;
using static CityVoxWeb.Common.EntityValidationConstants.RegionValidations;

namespace CityVoxWeb.Data.Models.GeoEntities
{
    public class Region
    {
        public Region()
        {
            this.Municipalities = new HashSet<Municipality>();
        }
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(RegionNameMaxLength)]
        public string RegionName { get; set; } = null!;

        [Required]
        [MaxLength(OpenStreetMapCodeMaxLength)]
        public string OpenStreetMapCode { get; set; } = null!;

        public ICollection<Municipality> Municipalities { get; set; } = null!;
    }
}
