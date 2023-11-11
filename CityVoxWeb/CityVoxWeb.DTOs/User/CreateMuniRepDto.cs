using System.ComponentModel.DataAnnotations;
using static CityVoxWeb.Common.EntityValidationConstants.MunicipalityRepresentativeValidations;



namespace CityVoxWeb.DataTransferObjects.Users
{
    public class CreateMuniRepDto
    {
        [Required]
        public string MunicipalityId { get; set; } = null!;

        [Required]
        public string UserName { get; set; } = null!;

        public string? UserId { get; set; }

        [Required]
        [MaxLength(PositionNameMaxLength)]
        public string Position { get; set; } = null!;

        [Required]
        [MaxLength(DepartmentNameMaxLength)]
        public string Department { get; set; } = null!;

        // The representative's office contact details
        [MaxLength(OfficePhoneNumMaxLength)]
        public string? OfficePhoneNumber { get; set; }

        [MaxLength(OfficeEmailMaxLength)]
        public string? OfficeEmail { get; set; }

    }
}
