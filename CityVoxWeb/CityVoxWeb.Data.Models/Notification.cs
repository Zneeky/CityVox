using CityVoxWeb.Data.Models.UserEntities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static CityVoxWeb.Common.EntityValidationConstants.NotificationValidations;

namespace CityVoxWeb.Data.Models
{
    public class Notification
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime TimeSent { get; set; }

        [Required]
        [MaxLength(ContentMaxLength)]
        public string Content { get; set; } = null!;
        public bool IsRead { get; set; }

        // User who will receive the notification
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;
    }
}
