using System;
using System.Collections.Generic;

namespace CI_Platform.Entities.DataModels;

public partial class Notification
{
    public long NotificationId { get; set; }

    public long UserId { get; set; }

    public long NotificationSettingsId { get; set; }

    public long? FromUserId { get; set; }

    public long? ToUserId { get; set; }

    public long? MissionId { get; set; }

    public long? StoryId { get; set; }

    public long? TimesheetId { get; set; }

    public long? CommentId { get; set; }

    public long? MissionApplicationId { get; set; }

    public bool? Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Comment? Comment { get; set; }

    public virtual User? FromUser { get; set; }

    public virtual Mission? Mission { get; set; }

    public virtual MissionApplication? MissionApplication { get; set; }

    public virtual NotificationSetting NotificationSettings { get; set; } = null!;

    public virtual Story? Story { get; set; }

    public virtual Timesheet? Timesheet { get; set; }

    public virtual User? ToUser { get; set; }

    public virtual User User { get; set; } = null!;
}
