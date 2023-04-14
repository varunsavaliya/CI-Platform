using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ci_Platform.Repositories.Interfaces
{
    public interface IAdmin
    {

        public List<User> GetUsers();
        public User GetUserById(long userId);
        public Task<string> DeleteUserById(long userId);
        public Task AddUser(AdminUserModel model);
        public bool IsUserExists(string email);
        public bool IsUserExists(string email, long userId);
        public Task UpdateUser(AdminUserModel model);

    }
}
