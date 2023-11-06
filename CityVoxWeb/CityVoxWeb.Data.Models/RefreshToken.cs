using CityVoxWeb.Data.Models.UserEntities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityVoxWeb.Data.Models
{
    public class RefreshToken
    {
        [Key]
        public string Token { get; set; } = null!;
        public DateTime Expires { get; set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;

        [ForeignKey(nameof(Token))]
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;
    }
}
