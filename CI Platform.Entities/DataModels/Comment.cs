﻿using System;
using System.Collections.Generic;

namespace CI_Platform.Entities.DataModels;

public partial class Comment
{
    public long CommentId { get; set; }

    public long UserId { get; set; }

    public long MissionId { get; set; }

    public string? ApprovalStatus { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public string? Comment1 { get; set; }

    public virtual Mission Mission { get; set; } = null!;

    public virtual ICollection<Notification> Notifications { get; } = new List<Notification>();

    public virtual User User { get; set; } = null!;
}
