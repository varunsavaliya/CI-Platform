using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;

namespace Ci_Platform.Repositories.Interfaces
{
    public interface IAuthentication : IRepository<User>
    {
        public string GenerateToken();
        public void SendMail(string token, string PasswordResetLink, ForgotPasswordModel model);
        public void ResetPass(PasswordResetModel model);
        public void ResetPassword(long UserId, string pass);
        public Task<bool> IsPasswordResetDataExist(PasswordResetModel model);
        public bool comparePass(long UserId, String pass);
        public Task AddToContactUs(UserHeaderViewModel contactUs);
        public void SetSession(string email);
        public void DestroySession();
        public int? GetUserRole(long userId);

    }
}
