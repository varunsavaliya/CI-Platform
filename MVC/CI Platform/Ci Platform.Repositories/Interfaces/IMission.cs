using CI_Platform.Entities.ViewModels;

namespace Ci_Platform.Repositories.Interfaces
{
    public interface IMission: ISendInvite<MissionVolunteeringModel>
    {
        public (List<MissionCard> missionList, int totalRecords) GetMissionCards(InputData queryParams, long userId)
;
    }
}
