using System;
using System.Collections.Generic;

namespace CI_Platform.Entities.DataModels;

public partial class NotificationSetting
{
    public long NotificationSettingsId { get; set; }

    public string? SettingName { get; set; }

    public virtual ICollection<Notification> Notifications { get; } = new List<Notification>();

    public virtual ICollection<UserNotificationSetting> UserNotificationSettings { get; } = new List<UserNotificationSetting>();
}
