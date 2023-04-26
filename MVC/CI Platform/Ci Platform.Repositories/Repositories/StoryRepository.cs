using Ci_Platform.Repositories.Interfaces;
using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Ci_Platform.Repositories.Repositories
{
    public class StoryRepository : SendInvite<StoryDetailModel>, IStory
    {
        private new readonly ApplicationDbContext _context;

        public StoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Story> GetStoryById(long id)
        {
            Story? story = _context.Stories.Where(s => s.StoryId == id).Include(s => s.Mission).Include(s => s.User).Include(s => s.StoryMedia).FirstOrDefault();
            if (story.Status == "PUBLISHED" )
            {
                story.Views += 1;
                await _context.SaveChangesAsync();
            }
            return story;
        }
        public List<User> GetUsers(long userId)
        {
            return _context.Users.Where(u => u.UserId != userId && u.DeletedAt == null).ToList();
        }
        public async Task<List<Story>> GetStories(List<long> storyIds)
        {
            List<Story> stories = new();
            stories = await _context.Stories.Where(s => storyIds.Contains(s.StoryId) && s.User.DeletedAt == null)
                .Include(s => s.User)
                .Include(s => s.StoryMedia)
                .Include(s => s.Mission).ThenInclude(m => m.Theme)
                .ToListAsync();

            return stories;
        }

        public async Task<List<Mission>> GetMissionsByUser(long userId)
        {
            List<Mission> missions = new();
            missions = await _context.Missions.Where(m => m.MissionApplications.Any(ma => ma.UserId == userId && ma.ApprovalStatus == "PUBLISHED"))
                             .ToListAsync();
            return missions;
        }

        public Story AvailableStory(long userId, long missionId)
        {
            Story? story = _context.Stories.Include(s => s.StoryMedia).Where(s => s.MissionId == missionId && s.UserId == userId).FirstOrDefault();
            return story;
        }
        public bool isStoryAvailable(long userId, long missionId)
        {
            return _context.Stories.Any(s => s.MissionId == missionId && s.UserId == userId);
        }

        public (List<StoryCard> storyList, int totalRecords) GetStoryCards(InputData queryParams)
        {
            var query = _context.Stories.Where(story => story.Status == "PUBLISHED" && story.DeletedAt == null).AsQueryable();

            if (queryParams.CityIds.Any())
                query = query.Where(story => queryParams.CityIds.Contains(story.Mission.CityId));

            if (!queryParams.CityIds.Any() && queryParams.CountryId != 0)
                query = query.Where(story => story.Mission.CountryId == queryParams.CountryId);

            if (queryParams.ThemeIds.Any())
                query = query.Where(story => queryParams.ThemeIds.Contains(story.Mission.ThemeId));

            if (queryParams.SkillIds.Any())
                query = query.Where(story => story.Mission.MissionSkills.Any(skill => queryParams.SkillIds.Contains(skill.SkillId)));

            if (!string.IsNullOrWhiteSpace(queryParams.searchText))
                query = query.Where(story => story.Title.ToLower().Contains(queryParams.searchText.ToLower()));

            var storyCardQuery = query.Select(story => new StoryCard()
            {
                StoryData = story,
                UserName = story.User.FirstName+" " + story.User.LastName,
                StoryMedia = story.StoryMedia.Where(story => story.Type == "image" && story.DeletedAt == null).Select(story => story.Path).FirstOrDefault(),
                ThemeName = story.Mission.Theme.Title,
                UserProfile = story.User.Avatar,
            });

            var records = storyCardQuery
                .Skip((queryParams.pageNo - 1) * queryParams.pageSize)
                .Take(queryParams.pageSize)
                .ToList();

            return (records, storyCardQuery.Count());
        }

        public async Task DeleteImages(Story story)
        {
            var existingMedia = _context.StoryMedia.Where(m => m.StoryId == story.StoryId && m.Type == "image");
            var oldMediaPaths = new List<string>();
            var oldMedia = _context.StoryMedia.Where(m => existingMedia.Select(s => s.StoryId).Contains(m.StoryId) && m.Type == "image").ToList();
            if (oldMedia.Count > 0)
            {
                foreach (var media in oldMedia)
                {
                    var oldMediaPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", media.Path);

                    oldMediaPaths.Add(oldMediaPath);
                }
            }
            // delete the previous images from the server's directory
            foreach (var file in Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload")))
            {
                if (oldMediaPaths.Contains(file))
                {
                    System.IO.File.Delete(file);
                }
            }
            _context.RemoveRange(existingMedia);
            await _context.SaveChangesAsync();
        }

        public async Task AddImages(List<IFormFile> model, Story story)
        {
            var mediaCount = 1;
            foreach (var file in model)
            {
                var fileExtension = Path.GetExtension(file.FileName);
                var fileName = "story_" + story.StoryId + "_image_" + mediaCount + fileExtension;
                mediaCount++;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", fileName);
                var Photos = new StoryMedium
                {
                    StoryId = story.StoryId,
                    Type = "image",
                    Path = fileName
                };
                await _context.StoryMedia.AddAsync(Photos);
                await _context.SaveChangesAsync();
                //using (var stream = new FileStream(filePath, FileMode.Create))
                //{
                //    await file.CopyToAsync(stream);
                //}
                //simplified using statement
                using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);
            }
        }
        public async Task HandleStoryUrls(String[] urls, Story story)
        {
            var existingMedia = _context.StoryMedia.Where(m => m.StoryId == story.StoryId && m.Type == "VIDEO");
            _context.StoryMedia.RemoveRange(existingMedia);

            foreach (var url in urls)
            {
                if (url is not null)
                {
                    var VideoUrl = new StoryMedium
                    {
                        StoryId = story.StoryId,
                        Type = "VIDEO",
                        Path = url
                    };

                   await _context.StoryMedia.AddAsync(VideoUrl);
                   await _context.SaveChangesAsync();
                }
            }
        }
        public async Task AddStoryAsDraft(ShareStoryModel model, Story story)
        {
            await _context.Stories.AddAsync(story);
            await _context.SaveChangesAsync();

            if (model.url is not null)
            {
                await HandleStoryUrls(model.url, story);
            }
            await AddImages(model.images, story);
        }

        public async Task UpdateStory(ShareStoryModel model, Story story)
        {
            story.Title = model.storyTitle;
            story.UpdatedAt = DateTime.Now;
            story.Description = model.Story;
            story.Status = "DRAFT";
            await _context.SaveChangesAsync();

            if (model.url is not null)
            {
                await HandleStoryUrls(model.url, story);
            }

            if (model.images is not null)
            {
                await DeleteImages(story);
                await AddImages(model.images, story);
                
            }
        }
        public async Task AddStoryAsPending(ShareStoryModel model, Story story)
        {
            story.Title = model.storyTitle;
            story.UpdatedAt = DateTime.Now;
            story.Description = model.Story;
            story.Status = "PENDING";
            await _context.SaveChangesAsync();

            if (model.url is not null)
            {
                await HandleStoryUrls(model.url, story);
            }

            if (model.images is not null)
            {
                await DeleteImages(story);
                await AddImages(model.images, story);
            }
        }              
    }
}
