using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;

namespace Ci_Platform.Repositories.Interfaces
{
    public interface INotification
    {
        public (List<UserNotification> notificationList, int count) GetNotificationsByUserId(long userId);
        public List<NotificationSetting> GetNotificationSettings();


    }
}
