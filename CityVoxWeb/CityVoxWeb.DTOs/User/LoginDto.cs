using System.ComponentModel.DataAnnotations;
using static CityVoxWeb.Common.EntityValidationConstants.ApplicationUserValidations;

namespace CityVoxWeb.DTOs.Users
{
    public class LoginDto
    {
        [Required]
        [MinLength(EmailMinLength)]
        [MaxLength(EmailMaxLength)]
        public string Email { get; set; } = null!;

        [Required]
        [MinLength(PasswordMinLength)]
        [MaxLength(PasswordMaxLength)]
        [RegularExpression(PasswordRegex)]
        public string Password { get; set; } = null!;
    }
}
