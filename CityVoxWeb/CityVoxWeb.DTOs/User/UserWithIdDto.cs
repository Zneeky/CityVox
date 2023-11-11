using System.ComponentModel.DataAnnotations;
using static CityVoxWeb.Common.EntityValidationConstants.ApplicationUserValidations;



namespace CityVoxWeb.DataTransferObjects.Users
{
    public class UserWithIdDto
    {
        public Guid Id { get; set; }

        public string? AccessToken { get; set; }

        [Required]
        [MinLength(FirstNameMinLength)]
        [MaxLength(FirstNameMaxLength)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MinLength(LastNameMinLength)]
        [MaxLength(LastNameMaxLength)]
        public string LastName { get; set; } = null!;

        [Required]
        [MaxLength(EmailMaxLength)]
        [MinLength(EmailMinLength)]
        public string Email { get; set; } = null!;

        [Required]
        public string Role { get; set; } = null!;

        public string? ProfilePicture { get; set; }

        [Required]
        [MinLength(UsernameMinLength)]
        [MaxLength(UsernameMaxLength)]
        public string Username { get; set; } = null!;


    }
}
