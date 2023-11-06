using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CityVoxWeb.Data.Models.GeoEntities;
using static CityVoxWeb.Common.EntityValidationConstants.MunicipalityRepresentativeValidations;

namespace CityVoxWeb.Data.Models.UserEntities
{
    public class MunicipalityRepresentative
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey(nameof(Municipality))]
        public Guid MunicipalityId { get; set; }

        public Municipality Municipality { get; set; } = null!;

        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }

        public ApplicationUser User { get; set; } = null!;


        // Role or position of the representative in the municipality
        [Required]
        [MaxLength(PositionNameMaxLength)]
        public string Position { get; set; } = null!;

        // The department the representative belongs to
        [Required]
        [MaxLength(DepartmentNameMaxLength)]
        public string Department { get; set; } = null!;

        // The representative's office contact details
        [MaxLength(OfficePhoneNumMaxLength)]
        public string? OfficePhoneNumber { get; set; }

        [MaxLength(OfficeEmailMaxLength)]
        public string? OfficeEmail { get; set; }

        // An optional description or bio
        [MaxLength(BioMaxLength)]
        public string? Bio { get; set; }

        // The date the representative started their position
        public DateTime StartDate { get; set; }

        // If the representative's position is term-limited, the end date of the term
        public DateTime? EndDate { get; set; }

    }
}
