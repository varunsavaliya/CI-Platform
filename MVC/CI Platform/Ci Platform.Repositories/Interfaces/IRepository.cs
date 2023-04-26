using CI_Platform.Entities.DataModels;

namespace Ci_Platform.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        public void Add(T model);
        public void Save();
        public bool IsRegistered(string email);
        public bool IsRegistered(PasswordReset user);
        public bool ComparePassword(User user);
    }
}
