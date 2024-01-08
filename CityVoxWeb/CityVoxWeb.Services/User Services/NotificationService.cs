using AutoMapper;
using CityVoxWeb.Data;
using CityVoxWeb.Data.Models;
using CityVoxWeb.Data.Models.IssueEntities;
using CityVoxWeb.Data.Models.IssueEntities.Enumerators.Emergency;
using CityVoxWeb.Data.Models.IssueEntities.Enumerators.InfIssue;
using CityVoxWeb.Data.Models.IssueEntities.Enumerators.Report;
using CityVoxWeb.DTOs.Notification;
using CityVoxWeb.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityVoxWeb.Services.User_Services
{
    public class NotificationService : INotificationService
    {
        private readonly CityVoxDbContext _dbContext;
        private readonly IMapper _mapper;

        public NotificationService(CityVoxDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task CreateNotificationForReportAsync(int statusValue, string issueType, Report report)
        {
            try
            {
                string status = GetStatusText(statusValue, issueType);
                Notification notificationToCreate = new Notification
                {
                    UserId = report.UserId,
                    TimeSent = DateTime.UtcNow,
                    IsRead = false,
                    Content = $"Your report submission about \"{report.Title}\" was {status}, report, {report.Id}",
                };

                await _dbContext.Notifications.AddAsync(notificationToCreate);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task CreateNotificationForEmergencyAsync(int statusValue, string issueType, Emergency emergency)
        {
            try
            {
                string status = GetStatusText(statusValue, issueType);
                Notification notificationToCreate = new Notification
                {
                    UserId = emergency.UserId,
                    TimeSent = DateTime.UtcNow,
                    IsRead = false,
                    Content = $"Your emergency submission about \"{emergency.Title}\" was {status}, emergency, {emergency.Id}",
                };

                await _dbContext.Notifications.AddAsync(notificationToCreate);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task CreateNotificationForInfrastructureIssueAsync(int statusValue, string issueType, InfrastructureIssue infrastructureIssue)
        {
            try
            {
                string status = GetStatusText(statusValue, issueType);
                Notification notificationToCreate = new Notification
                {
                    UserId = infrastructureIssue.UserId,
                    TimeSent = DateTime.UtcNow,
                    IsRead = false,
                    Content = $"Your Infrastructure issue submission about \"{infrastructureIssue.Title}\" was {status},infIssue, {infrastructureIssue.Id}",
                };

                await _dbContext.Notifications.AddAsync(notificationToCreate);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static string GetStatusText(int statusValue, string issueType)
        {
            return issueType switch
            {
                "report" => ((ReportStatus)statusValue).ToString(),
                "emergency" => ((EmergencyStatus)statusValue).ToString(),
                "infIssue" => ((InfrastructureIssueStatus)statusValue).ToString(),
                _ => throw new Exception("Not accepted issue type")
            };
        }

        public async Task<List<ExportNotificationDto>> GetUnreadNotificationsByUserIdAsync(string userId)
        {
            try
            {
                var notifications = await _dbContext.Notifications
                    .Where(n => n.UserId.ToString() == userId && n.IsRead == false)
                    .ToListAsync();

                var notificationsDtos = _mapper.Map<List<ExportNotificationDto>>(notifications);
                return notificationsDtos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task ChangeNotificationToReadAsync(string notificationId)
        {
            try
            {
                var notification = _dbContext.Notifications
                .Where(n => n.Id.ToString() == notificationId)
                .FirstOrDefault();

                if (notification != null)
                {
                    notification.IsRead = true;
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("Notification with the given id vas not found!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }
    }
}
