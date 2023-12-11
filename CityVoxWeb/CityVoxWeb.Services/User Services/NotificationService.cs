using AutoMapper;
using CityVoxWeb.Data;
using CityVoxWeb.Data.Models;
using CityVoxWeb.Data.Models.IssueEntities;
using CityVoxWeb.Data.Models.IssueEntities.Enumerators.Emergency;
using CityVoxWeb.Data.Models.IssueEntities.Enumerators.InfIssue;
using CityVoxWeb.Data.Models.IssueEntities.Enumerators.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityVoxWeb.Services.User_Services
{
    public class NotificationService
    {
        private readonly CityVoxDbContext _dbContext;
        private readonly IMapper _mapper;

        public NotificationService(CityVoxDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task CreateNotificationForReport(int statusValue, string issueType, Report report)
        {
            try
            {
                string status = GetStatusText(statusValue,issueType);
                Notification notificationToCreate = new Notification
                {
                    UserId = report.UserId,
                    TimeSent = DateTime.UtcNow,
                    IsRead = false,
                    Content = $"Your report about \"{report.Title}\" was {status}, report, {report.Id}",
                };

                await _dbContext.Notifications.AddAsync(notificationToCreate);
                await _dbContext.SaveChangesAsync();
            }
            catch(Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task CreateNotificationForEmergency(int statusValue, string issueType, Emergency emergency)
        {
            try
            {
                string status = GetStatusText(statusValue, issueType);
                Notification notificationToCreate = new Notification
                {
                    UserId = emergency.UserId,
                    TimeSent = DateTime.UtcNow,
                    IsRead = false,
                    Content = $"Your emergency about \"{emergency.Title}\" was {status}, emergency, {emergency.Id}",
                };

                await _dbContext.Notifications.AddAsync(notificationToCreate);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task CreateNotificationForInfrastructureIssue(int statusValue, string issueType, InfrastructureIssue infrastructureIssue)
        {
            try
            {
                string status = GetStatusText(statusValue, issueType);
                Notification notificationToCreate = new Notification
                {
                    UserId = infrastructureIssue.UserId,
                    TimeSent = DateTime.UtcNow,
                    IsRead = false,
                    Content = $"Your infrastructureIssue about \"{infrastructureIssue.Title}\" was {status},infIssue, {infrastructureIssue.Id}",
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
    }
}
