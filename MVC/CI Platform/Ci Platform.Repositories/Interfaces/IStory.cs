using Ci_Platform.Repositories.Repositories;
using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ci_Platform.Repositories.Interfaces
{
    public interface IStory : ISendInvite<StoryDetailModel>
    {
        public Task<List<Story>> GetStories(List<long> storyIds);
        public Task<List<Mission>> GetMissionsByUser(long userId);
        public Task AddStoryAsDraft(ShareStoryModel model, Story story);
        public Task<Story> AvailableStory(long userId, long missionId);
        public bool isStoryAvailable(long userId, long missionId);
        public Task UpdateStory(ShareStoryModel model, Story story);
        public Task AddStoryAsPending(ShareStoryModel model, Story story);
        public Task<Story> GetStoryById(long id);
        public Task<List<User>> GetUsers(long userId);
    }
}
