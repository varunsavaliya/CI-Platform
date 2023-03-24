using Ci_Platform.Repositories.Interfaces;
using CI_Platform.Entities.DataModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ci_Platform.Repositories.Repositories
{
    public class StoryRepository : Repository<Story> ,IStory
    {
        private ApplicationDbContext _context;

        public StoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public ApplicationDbContext Context { get; }

        public async Task<List<Story>> GetStories(List<long> storyIds)
        {
            List<Story> stories = new List<Story>();
            stories = await _context.Stories.Where(s => storyIds.Contains(s.StoryId))
                .Include(s=>s.User)
                .Include(s => s.Mission).ThenInclude(m => m.Theme)
                .ToListAsync();

            return stories;
        }
        
        public async Task<List<Mission>> GetMissionsByUser(long userId)
        {
            List<Mission> missions = new List<Mission>();
            missions = await _context.Missions
                .Where(m => m.MissionApplications.Any(ma => ma.UserId == userId && ma.ApprovalStatus == "PUBLISHED"))
                             .ToListAsync();

            return missions;
        }
    }
}
