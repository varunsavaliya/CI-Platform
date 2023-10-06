namespace Ci_Platform.Repositories.Interfaces
{
    public interface ISendInvite<T> where T : class
    {
        public Task SendEmailInvite(long ToUserId, long Id, long FromUserId, String link, T viewmodel);
    }
}
