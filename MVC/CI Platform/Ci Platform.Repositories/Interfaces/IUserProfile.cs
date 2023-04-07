using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;

namespace Ci_Platform.Repositories.Interfaces
{
    public interface IUserProfile
    {
        public Task<UserProfileModel> GetUser(long id);
        public Task<List<UserSkill>> GetUserSkills(long id);
        public Task UpdateUserDetails(long id, UserProfileModel model);
    }
}
