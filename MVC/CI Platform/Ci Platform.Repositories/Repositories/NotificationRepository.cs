using Ci_Platform.Repositories.Interfaces;
using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;

namespace Ci_Platform.Repositories.Repositories
{
    public class NotificationRepository: INotification
    {
        private readonly ApplicationDbContext _context;

        public NotificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public (List<UserNotification> notificationList, int count) GetNotificationsByUserId(long userId)
        {
           List<long?> userSettings = _context.UserNotificationSettings.Where(notif => notif.UserId== userId).Select(notif=>notif.NotificationSettingsId).ToList();
            var query = _context.Notifications.Where(notif=>notif.DeletedAt== null && notif.UserId== userId && userSettings.Contains(notif.NotificationSettingsId)).AsQueryable();

            var count = query.Count(count => count.Status == true);
            var userNotificationQuery = query.Select(notif => new UserNotification()
            {
                Notification = notif, 
                FromUser = notif.User.FirstName + " " + notif.User.LastName,
                MissionTitle = notif.Mission.Title,
                FromUserAvatar = notif.User.Avatar == null? "default user avatar.jpg" : notif.User.Avatar,
                StoryTitle = notif.Story.Title,
            });

            

            return (userNotificationQuery.ToList(), count);
        }

        public List<NotificationSetting> GetNotificationSettings()
        {
            List<NotificationSetting> notificationSettings = _context.NotificationSettings.ToList();
            return notificationSettings;
        }
    }
}
