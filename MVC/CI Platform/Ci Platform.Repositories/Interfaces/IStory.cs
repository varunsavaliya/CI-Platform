using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;

namespace Ci_Platform.Repositories.Interfaces
{
    public interface IStory : ISendInvite<StoryDetailModel>
    {
        public Task<List<Story>> GetStories(List<long> storyIds);
        public (List<StoryCard> storyList, int totalRecords) GetStoryCards(InputData queryParams);

        public Task<List<Mission>> GetMissionsByUser(long userId);
        public Task AddStoryAsDraft(ShareStoryModel model, Story story);
        public Story AvailableStory(long userId, long missionId);
        public bool isStoryAvailable(long userId, long missionId);
        public Task UpdateStory(ShareStoryModel model, Story story);
        public Task AddStoryAsPending(ShareStoryModel model, Story story);
        public Task<Story> GetStoryById(long id);
        public List<User> GetUsers(long userId);
    }
}
