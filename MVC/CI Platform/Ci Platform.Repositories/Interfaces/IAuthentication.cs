using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ci_Platform.Repositories.Interfaces
{
    public interface IAuthentication : IRepository<User>
    {
        public PasswordReset BindData(ForgotPasswordModel model);
        public string GenerateToken();
        public void SendMail(string token, string PasswordResetLink, ForgotPasswordModel model);
        public void ResetPass(PasswordResetModel model);
        public void ResetPassword(long UserId, string pass);

        public bool comparePass(long UserId, String pass);
    }
}
