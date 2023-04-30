using Ci_Platform.Repositories.Interfaces;
using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Ci_Platform.Repositories.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool IsRegistered(string email)
        {
            bool isRegistered = _context.Users.Any(a => a.Email == email && a.DeletedAt == null && a.Status == 1);
            return isRegistered;
        }
        public bool IsRegistered(PasswordReset user)
        {
            bool isRegistered = _context.Users.Any(a => a.Email == user.Email && a.DeletedAt == null);
            return isRegistered;
        }
        public bool ComparePassword(User user)
        {
            var dbUser = _context.Users.FirstOrDefault(a => a.Email.Equals(user.Email) && a.DeletedAt == null);
            if (dbUser != null)
            {
                return string.Equals(dbUser.Password, user.Password, StringComparison.Ordinal);
            }
            return false;
        }

        public async Task AddNotitifcationData(NotificationData notificaitonData)
        {
            // notification settings
            //1   Recommended missions
            //2   Volunteering Hours
            //3   Volunteering Goals
            //4   My comments
            //5   My Stories
            //6   New missions
            //7   New messages
            //8   Recommended story
            //9   Mission application
            //10  News

            if (notificaitonData.NotificationSettingsId == 6)
            {
                List<long> userIds = await _context.Users.Where(user => user.Status == 1 && user.DeletedAt == null).Select(user => user.UserId).ToListAsync();

                foreach (long id in userIds)
                {
                    Notification newNotification = new()
                    {
                        UserId = id,
                        NotificationSettingsId = notificaitonData.NotificationSettingsId,
                        MissionId = notificaitonData.MissionId,
                        Status = false,
                    };
                    await _context.Notifications.AddAsync(newNotification);
                }
            }
            else
            {
                Notification newNotification = new()
                {
                    UserId = notificaitonData.UserId,
                    NotificationSettingsId = notificaitonData.NotificationSettingsId,
                    FromUserId = notificaitonData.FromUserId,
                    ToUserId = notificaitonData.ToUserId,
                    MissionId = notificaitonData.MissionId,
                    StoryId = notificaitonData.StoryId,
                    TimesheetId = notificaitonData.TimesheetId,
                    CommentId = notificaitonData.CommentId,
                    MissionApplicationId = notificaitonData.MissionApplicationId,
                    Status = false,
                };

                await _context.Notifications.AddAsync(newNotification);
            }

            await _context.SaveChangesAsync();

            //if (notificaitonData.NotificationSettingsId == 1)
            //{

            //if (notificaitonData.NotificationSettingsId == 8)
            //{
            //    Notification newNotification = new()
            //    {
            //        UserId = userId,
            //        NotificationSettingsId = notificaitonData.NotificationSettingsId,
            //        FromUserId = userId,
            //        ToUserId = notificaitonData.ToUserId,
            //        StoryId = notificaitonData.StoryId,
            //        Status = false,
            //    };

            //    await _context.Notifications.AddAsync(newNotification);
            //}

            //if (notificaitonData.NotificationSettingsId == 9)
            //{
            //    Notification newNotification = new()
            //    {
            //        UserId = userId,
            //        NotificationSettingsId = notificaitonData.NotificationSettingsId,
            //        MissionApplicationId = notificaitonData.MissionApplicationId,
            //        MissionId = notificaitonData.MissionId,
            //        Status = false,
            //    };

            //    await _context.Notifications.AddAsync(newNotification);
            //}
        }
    }
}
