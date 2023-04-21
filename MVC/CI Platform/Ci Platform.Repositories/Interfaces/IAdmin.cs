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
        public AdminUserModel GetUserById(long userId);
        public Task<string> DeleteUserById(long userId);
        public Task AddUser(AdminUserModel model);
        public bool IsUserExists(string email);
        public bool IsUserExists(string email, long userId);
        public Task UpdateUser(AdminUserModel model);
        public List<CmsTable> GetCMSList();
       public AdminCMSModel GetCMSById(long cmsId);
        public Task AddCMS(AdminCMSModel model);
        public bool IsCMSExists(string slug);
        public bool IsCMSExists(string slug, long cmsPageId);
        public Task UpdateCMS(AdminCMSModel model);
        public List<Mission> GetMissionList();
        public Task<AdminMissionModel> GetMissionById(long missionId);
        public Task AddMission(AdminMissionModel model);
        public Task UpdateMission(AdminMissionModel model, long missionId);

    }
}
