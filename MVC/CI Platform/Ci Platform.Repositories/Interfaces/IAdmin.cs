using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using Microsoft.EntityFrameworkCore;
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
        public Task<string> DeleteCMSById(long cmsId);
        public Task<string> DeleteMissionById(long missionId);
        public Task<string> DeleteMissionThemeById(long themeId);
        public Task<string> DeleteMissionSkillById(int skillId);
        public Task<string> DeleteStoryById(long storyId);
        public Task<string> DeleteBannerById(long bannerId);

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
        public Task<List<MissionTheme>> GetMissionThemeList();
        public Task AddMissionTheme(AdminMissionThemeModel model);
        public Task<bool> IsThemeExists(string title);
        public Task<bool> IsThemeExists(string title, long themeId);
        public Task UpdateMissionTheme(AdminMissionThemeModel model);
        public Task<AdminMissionThemeModel> GetMissionThemeById(long themeId);
        public Task<List<Skill>> GetMissionSkillList();
        public Task AddMissionSkill(AdminMissionSkillModel model);
        public Task<bool> IsSkillExists(string skillName);
        public Task<bool> IsSkillExists(string skillName, int skillId);
        public Task UpdateMissionSkill(AdminMissionSkillModel model);
        public Task<AdminMissionSkillModel> GetMissionSkillById(int skillId);
        public Task<List<MissionApplication>> GetMissionApplicationsList();
        public Task ApproveMissionApplication(long missionApplicationId);
        public Task DeclineMissionApplication(long missionApplicationId);
        public Task<List<Story>> GetStoriesList();
        public  Task ApproveStory(long storyId);
        public  Task DeclineStory(long storyId);
        public Task<BannerModel> GetBannerById(long bannerId);
        public Task<List<Banner>> GetBannerList();
        public Task UpdateBanner(BannerModel model, long bannerId);
        public Task AddBanner(BannerModel model);
        public Task<List<Comment>> GetCommentsList();
        public Task<List<Timesheet>> GetTimesheetList();
        public Task ApproveComment(long commentId);
        public Task DeclineComment(long commentId); 
        public Task ApproveTimesheet(long timesheetId);
        public Task DeclineTimesheet(long timesheetId);
    }
}
