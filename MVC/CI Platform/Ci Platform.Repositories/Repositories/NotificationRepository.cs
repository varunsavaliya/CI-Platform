using Ci_Platform.Repositories.Interfaces;
using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Ci_Platform.Repositories.Repositories
{
    public class NotificationRepository : INotification
    {
        private readonly ApplicationDbContext _context;

        public NotificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public (List<UserNotification> newNotificationList,List<UserNotification> yesterdayNotificationList, List<UserNotification> olderNotificationList, int count) GetNotificationsByUserId(long userId)
        {
            List<long> userSettings = _context.UserNotificationSettings.Where(notif => notif.UserId == userId && notif.IsEnabled == true).Select(notif => notif.NotificationSettingsId).ToList();
            var query = _context.Notifications.Where(notif => notif.DeletedAt == null && notif.UserId == userId && userSettings.Contains(notif.NotificationSettingsId)).OrderByDescending(notif => notif.CreatedAt).AsQueryable();

            var count = query.Count(count => count.Status == false);

            var now = DateTime.Now;

            var newNotificationQuery = query.Where(notification => notification.CreatedAt.Date == now.Date).Select(notif => new UserNotification()
            {
                Notification = notif,
                FromUser = notif.FromUser.FirstName + " " + notif.FromUser.LastName,
                MissionTitle = notif.Mission.Title,
                FromUserAvatar = notif.FromUser.Avatar == null ? "default user avatar.jpg" : notif.FromUser.Avatar,
                StoryTitle = notif.Story.Title,
                MissionApplicationStatus = notif.MissionApplication.ApprovalStatus,
                CommentStatus = notif.Comment.ApprovalStatus,
                TimesheetStatus = notif.Timesheet.Status,
                StoryStatus = notif.Story.Status,
            });

            var yesterdayNotificationQuery = query.Where(notification => notification.CreatedAt.Date == now.AddDays(-1).Date).Select(notif => new UserNotification()
            {
                Notification = notif,
                FromUser = notif.FromUser.FirstName + " " + notif.FromUser.LastName,
                MissionTitle = notif.Mission.Title,
                FromUserAvatar = notif.FromUser.Avatar == null ? "default user avatar.jpg" : notif.FromUser.Avatar,
                StoryTitle = notif.Story.Title,
                MissionApplicationStatus = notif.MissionApplication.ApprovalStatus,
                CommentStatus = notif.Comment.ApprovalStatus,
                TimesheetStatus = notif.Timesheet.Status,
                StoryStatus = notif.Story.Status,
            });

            var olderNotificationQuery = query.Where(notification => notification.CreatedAt.Date < now.AddDays(-1).Date).Select(notif => new UserNotification()
            {
                Notification = notif,
                FromUser = notif.FromUser.FirstName + " " + notif.FromUser.LastName,
                MissionTitle = notif.Mission.Title,
                FromUserAvatar = notif.FromUser.Avatar == null ? "default user avatar.jpg" : notif.FromUser.Avatar,
                StoryTitle = notif.Story.Title,
                MissionApplicationStatus = notif.MissionApplication.ApprovalStatus,
                CommentStatus = notif.Comment.ApprovalStatus,
                TimesheetStatus = notif.Timesheet.Status,
                StoryStatus = notif.Story.Status,
            });



            return (newNotificationQuery.ToList(),yesterdayNotificationQuery.ToList(), olderNotificationQuery.ToList(), count);
        }

        public List<NotificationSetting> GetNotificationSettings()
        {
            List<NotificationSetting> notificationSettings = _context.NotificationSettings.ToList();
            return notificationSettings;
        }

        public async Task<List<long>> GetUserNotificationSettingIds(long userId)
        {
            return await _context.UserNotificationSettings.Where(setting => setting.UserId == userId && setting.IsEnabled == true).Select(setting => setting.NotificationSettingsId).ToListAsync();
        }

        public async Task UpdateNotificationSettings(List<long> settings, long userId)
        {
            var userNotificationSettings = await _context.UserNotificationSettings.Where(x => x.UserId == userId).ToListAsync();

            if (userNotificationSettings == null || userNotificationSettings.Count == 0)
            {
                // No user notification settings exist for this user, so add all the settings as new
                foreach (var setting in settings)
                {
                    UserNotificationSetting notificationSetting = new()
                    {
                        UserId = userId,
                        NotificationSettingsId = setting,
                        IsEnabled = true,
                    };

                    await _context.UserNotificationSettings.AddAsync(notificationSetting);
                }
            }
            else
            {
                // User notification settings exist for this user, so update the existing ones and add new ones
                foreach (var userNotificationSetting in userNotificationSettings)
                {
                    if (settings.Contains(userNotificationSetting.NotificationSettingsId))
                    {
                        userNotificationSetting.IsEnabled = true;
                    }
                    else
                    {
                        userNotificationSetting.IsEnabled = false;
                    }

                    settings.Remove(userNotificationSetting.NotificationSettingsId); 
                }

                foreach (var setting in settings)
                {
                    UserNotificationSetting notificationSetting = new()
                    {
                        UserId = userId,
                        NotificationSettingsId = setting,
                        IsEnabled = true,
                    };

                    await _context.UserNotificationSettings.AddAsync(notificationSetting);
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task NotificationMarkAsRead(List<long> notificationIds)
        {
            List<Notification> notifications = await _context.Notifications.Where(notification => notificationIds.Contains(notification.NotificationId)).ToListAsync();

            foreach (var notification in notifications)
            {
                notification.Status = true;
                notification.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
        }

        public async Task ClearAllNotifications(long userId)
        {
            List<Notification> notifications = await _context.Notifications.Where(notification => notification.UserId == userId).ToListAsync();

            _context.Notifications.RemoveRange(notifications);
            await _context.SaveChangesAsync();
        }
    }
}
