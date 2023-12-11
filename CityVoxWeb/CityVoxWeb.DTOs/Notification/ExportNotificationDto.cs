using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityVoxWeb.DTOs.Notification
{
    public class ExportNotificationDto
    {
        public string Id { get; set; } = null!;
        public string TimeSent { get; set; } = null!;
        public string Content { get; set; } = null!;
        public bool IsRead { get; set; }
        public string UserId { get; set; } = null!;
    }
}
