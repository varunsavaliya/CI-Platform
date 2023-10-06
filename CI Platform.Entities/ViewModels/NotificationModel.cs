using CI_Platform.Entities.DataModels;

namespace CI_Platform.Entities.ViewModels
{
    public class NotificationModel
    {
        public List<UserNotification> NewNotifications { get; set; } = new();
        public List<UserNotification> YesterDayNotifications { get; set; } = new();
        public List<UserNotification> OlderNotifications { get; set; } = new();
        public int NotificationCount { get; set; } = 0;
        public List<NotificationSetting> NotificationSettings { get; set; } = new();
        public List<long> UserNotificationSettingIds { get; set; } = new();
    }
    public class UserNotification
    {
        public Notification? Notification { get; set; } = new();
        public string? MissionTitle { get; set; } = string.Empty;
        public string? FromUser { get; set; } = string.Empty;
        public string? FromUserAvatar { get; set; } = string.Empty;  
        public string? StoryTitle { get; set; } = string.Empty;
        public string? MissionApplicationStatus { get;set; } = string.Empty;
        public string? CommentStatus { get;set; } = string.Empty;
        public string? TimesheetStatus { get;set; } = string.Empty;
        public string? StoryStatus { get;set; } = string.Empty;
    }
}
