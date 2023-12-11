using CityVoxWeb.Data.Models.IssueEntities;
using CityVoxWeb.DTOs.Notification;

namespace CityVoxWeb.Services.Interfaces
{
    public interface INotificationService
    {
        Task ChangeNotificationToReadAsync(string notificationId);
        Task CreateNotificationForEmergencyAsync(int statusValue, string issueType, Emergency emergency);
        Task CreateNotificationForInfrastructureIssueAsync(int statusValue, string issueType, InfrastructureIssue infrastructureIssue);
        Task CreateNotificationForReportAsync(int statusValue, string issueType, Report report);
        Task<List<ExportNotificationDto>> GetUnreadNotificationsByUserIdAsync(string userId);
    }
}