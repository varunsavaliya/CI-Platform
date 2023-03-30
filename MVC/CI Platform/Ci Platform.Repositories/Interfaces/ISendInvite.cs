using CI_Platform.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ci_Platform.Repositories.Interfaces
{
    public interface ISendInvite<T> where T : class
    {
        public Task SendEmailInvite(long ToUserId, long Id, long FromUserId, String link, T viewmodel);
    }
}
