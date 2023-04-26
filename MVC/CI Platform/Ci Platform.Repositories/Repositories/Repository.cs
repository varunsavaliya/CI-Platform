using Ci_Platform.Repositories.Interfaces;
using CI_Platform.Entities.DataModels;

namespace Ci_Platform.Repositories.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(T model)
        {
            _context.Add(model);
        }

        public void Save()
        {
            _context.SaveChanges();
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

    }
}
