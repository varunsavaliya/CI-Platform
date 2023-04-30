using Ci_Platform.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CI_Platform_web.Controllers
{
    public class NotificationController : Controller
    {
        private readonly INotification _notification;

        public NotificationController(INotification notification)
        {
            _notification = notification;
        }

        [HttpPost]
        public async Task<IActionResult> HandleNotificationSettings(long[] notificationSettings)
        {
            long userId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
            await _notification.UpdateNotificationSettings(notificationSettings.ToList(), userId);
            return Ok("Notification settings saved successfully.");
        }

        [HttpPost]
        public async Task<IActionResult> HandleNotificationStatus(long?[] notificationIds)
        {
            //await _notification.NotificationMarkAsRead(notificationIds.ToList());
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> ClearNotifications()
        {
            long userId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
            //await _notification.ClearAllNotifications(userId);
            return Ok();
        }

    }
}
