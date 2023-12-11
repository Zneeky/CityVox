using CityVoxWeb.DTOs.Notification;
using CityVoxWeb.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityVoxWeb.API.Controllers
{
    [Route("api/notifications")]
    [ApiController]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUnreadNotificationsByUserId(string userId)
        {
            try
            {
                var notifications = await _notificationService.GetUnreadNotificationsByUserIdAsync(userId);
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to get notifications! {ex.Message}");
            }
        }

        [HttpPut("{notificationId}")]
        public async Task<IActionResult> UpdateNotificationToRead(string notificationId)
        {
            try
            {
                await _notificationService.ChangeNotificationToReadAsync(notificationId);  
                return Ok("Updated successfully");
            }
            catch(Exception ex)
            {
                return BadRequest($"Failed to update notification! {ex.Message}");
            }
        }
    }
}
