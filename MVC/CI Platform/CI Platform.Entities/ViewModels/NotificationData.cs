using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class NotificationData
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
    }
}
