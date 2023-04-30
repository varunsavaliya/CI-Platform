using System;
using System.Collections.Generic;

namespace CI_Platform.Entities.DataModels;

public partial class UserNotificationSetting
{
    public long UserNotificationSettingsId { get; set; }

    public long UserId { get; set; }

    public long NotificationSettingsId { get; set; }

    public bool? IsEnabled { get; set; }

    public virtual NotificationSetting NotificationSettings { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
