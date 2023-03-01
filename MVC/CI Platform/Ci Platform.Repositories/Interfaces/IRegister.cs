using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ci_Platform.Repositories.Interfaces
{
    public interface IRegister : IRepository<User>
    {
        //public bool chkUser(ForgotPasswordModel model);
        //public bool chkUser(User user);
    }
}
