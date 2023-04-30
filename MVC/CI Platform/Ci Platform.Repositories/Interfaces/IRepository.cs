using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;

namespace Ci_Platform.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        public bool IsRegistered(string email);
        public bool IsRegistered(PasswordReset user);
        public bool ComparePassword(User user);

        public Task AddNotitifcationData(NotificationData notificaitonData);
    }
}
