using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;

namespace Ci_Platform.Repositories.Interfaces
{
    public interface INotification
    {
        public (List<UserNotification> newNotificationList, List<UserNotification> yesterdayNotificationList, List<UserNotification> olderNotificationList, int count) GetNotificationsByUserId(long userId);

        public List<NotificationSetting> GetNotificationSettings();
        public Task<List<long>> GetUserNotificationSettingIds(long userId);

        public Task UpdateNotificationSettings(List<long> settings, long userId);
        public Task NotificationMarkAsRead(List<long> notificationIds);

        public Task ClearAllNotifications(long userId);

    }
}
