using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;

namespace Ci_Platform.Repositories.Interfaces
{
    public interface IMission : IRepository<NotificationData>
    {
        public (List<MissionCard> missionList, int totalRecords) GetMissionCards(InputData queryParams, long userId);
        public Task HandleFav(long missionId, long userId);
        public MissionCard GetMissionVolunteeringData(long missionId, long userId);
        public List<MissionCard> GetRelatedMission(long missionId, long userId);
        public Task<int> GetTotalVolunteers(long missionId);
        public Task<List<string>> GetMissionDocs(long missionId);
        public Task<int> GetUserMissionRating(long missionId, long userId);
        public Task<List<User>> GetUsersList(long userId);

        public Task<List<MissionApplication>> GetRecentVolByPage(long missionId, int pageNo, int pageSize);
        public Task HandleRatings(long missionId, long userId, int rating);
        public Task HandleMissionApply(long missionId, long userId);
        public Task HandleComment(Comment comment, long userId);
        public Task SaveInviteData(long toUserId,long missionId,long fromUserId);

    }
}
