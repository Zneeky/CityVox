using System.ComponentModel.DataAnnotations;
using static CityVoxWeb.Common.EntityValidationConstants.ApplicationUserValidations;

namespace CityVoxWeb.DataTransferObjects.Users
{
    public class RegisterDto
    {
        [Required]
        [MinLength(UsernameMinLength)]
        [MaxLength(UsernameMaxLength)]
        public string Username { get; set; } = null!;

        [Required]
        [MinLength(FirstNameMinLength)]
        [MaxLength(FirstNameMaxLength)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MinLength(LastNameMinLength)]
        [MaxLength(LastNameMaxLength)]
        public string LastName { get; set; } = null!;

        [Required]
        [MinLength(EmailMinLength)]
        [MaxLength(EmailMaxLength)]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [MinLength(PasswordMinLength)]
        [MaxLength(PasswordMaxLength)]
        [RegularExpression(PasswordRegex)]
        public string Password { get; set; } = null!;

        [MaxLength(ProfilePictureUrlMaxLength)]
        public string? ProfilePictureUrl { get; set; }
        


    }
}
