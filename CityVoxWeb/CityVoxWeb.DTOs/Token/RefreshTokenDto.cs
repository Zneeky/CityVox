using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityVoxWeb.DTOs.Token
{
    public class RefreshTokenDto
    {
        [Required]
        public string Token { get; set; } = null!;
        public DateTime Expires { get; set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public Guid UserId { get; set; }
    }
}
