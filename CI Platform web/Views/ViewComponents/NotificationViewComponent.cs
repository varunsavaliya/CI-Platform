using Ci_Platform.Repositories.Interfaces;
using CI_Platform.Entities.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CI_Platform_web.Views.ViewComponents
{
    public class NotificationViewComponent : ViewComponent
    {
        private readonly INotification _notification;

        public NotificationViewComponent(INotification notification)
        {
            _notification = notification;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            long userId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
            var (newNotificationList,yesterdayNotificationList,olderNotificationList, count) = _notification.GetNotificationsByUserId(userId);
            NotificationModel model = new()
            {
                NewNotifications = newNotificationList,
                YesterDayNotifications = yesterdayNotificationList,
                OlderNotifications = olderNotificationList,
                NotificationCount = count,
                NotificationSettings = _notification.GetNotificationSettings(),
                UserNotificationSettingIds = await _notification.GetUserNotificationSettingIds(userId),
            };
            return View(model);   
        }
    }
}
