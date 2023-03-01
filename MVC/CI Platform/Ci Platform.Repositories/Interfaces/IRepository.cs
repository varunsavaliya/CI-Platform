using CI_Platform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ci_Platform.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        public void Add(T model);
        public void Save();
        public bool IsRegistered(User user);
        public bool IsRegistered(PasswordReset user);
        public bool ComparePassword(User user);
    }
}
