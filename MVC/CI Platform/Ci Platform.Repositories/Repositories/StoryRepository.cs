using Ci_Platform.Repositories.Interfaces;
using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ci_Platform.Repositories.Repositories
{
    public class StoryRepository : SendInvite<StoryDetailModel>, IStory
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
                .Include(s => s.User)
                .Include(s => s.StoryMedia)
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

        public async Task AddStoryAsDraft(ShareStoryModel model, Story story)
        {
            await _context.Stories.AddAsync(story);
            await _context.SaveChangesAsync();

            if (model.url is not null)
            {
                foreach (var url in model.url)
                {
                    if (url is not null)
                    {
                        var VideoUrl = new StoryMedium
                        {
                            StoryId = story.StoryId,
                            Type = "VIDEO",
                            Path = url
                        };

                        _context.StoryMedia.Add(VideoUrl);
                        _context.SaveChanges();
                    }
                }
            }

            foreach (var file in model.images)
            {
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var fileExtension = Path.GetExtension(file.FileName);
                var uniqueFileName = fileName + "_" + Guid.NewGuid().ToString() + fileExtension;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", uniqueFileName);

                var Photos = new StoryMedium
                {
                    StoryId = story.StoryId,
                    Type = "image",
                    Path = uniqueFileName
                };

                await _context.StoryMedia.AddAsync(Photos);
                await _context.SaveChangesAsync();

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }

        }

        public async Task<Story> AvailableStory(long userId, long missionId)
        {
            Story story = _context.Stories.Include(s => s.StoryMedia).Where(s => s.MissionId == missionId && s.UserId == userId).FirstOrDefault();
            return story;
        }
        public bool isStoryAvailable(long userId, long missionId)
        {
            return _context.Stories.Any(s => s.MissionId == missionId && s.UserId == userId);
        }

        public async Task UpdateStory(ShareStoryModel model, Story story)
        {
            story.Title = model.storyTitle;
            //story.UpdatedAt = viewmodel.date;
            story.Description = model.Story;
            story.Status = "DRAFT";
            await _context.SaveChangesAsync();

            if (model.url is not null)
            {
                var existingMedia = _context.StoryMedia.Where(m => m.StoryId == story.StoryId && m.Type == "VIDEO");
                _context.StoryMedia.RemoveRange(existingMedia);

                foreach (var url in model.url)
                {
                    if (url is not null)
                    {
                        var VideoUrl = new StoryMedium
                        {
                            StoryId = story.StoryId,
                            Type = "VIDEO",
                            Path = url
                        };

                        _context.StoryMedia.Add(VideoUrl);
                        _context.SaveChanges();
                    }
                }
            }

            if (model.images is not null)
            {
                var existingMedia = _context.StoryMedia.Where(m => m.StoryId == story.StoryId && m.Type == "image");

                var oldMediaPaths = new List<string>();

                var oldMedia = _context.StoryMedia.Where(m => existingMedia.Select(s => s.StoryId).Contains(m.StoryId) && m.Type == "image").ToList();
                if (oldMedia.Count > 0)
                {
                    foreach (var media in oldMedia)
                    {
                        //var oldMediaPath = Path.Combine(Directory.GetCurrentDirectory(), "Upload", media.Path);
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

                foreach (var file in model.images)
                {
                    var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                    var fileExtension = Path.GetExtension(file.FileName);
                    var uniqueFileName = fileName + "_" + Guid.NewGuid().ToString() + fileExtension;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", uniqueFileName);
                    var Photos = new StoryMedium
                    {
                        StoryId = story.StoryId,
                        Type = "image",
                        Path = uniqueFileName
                    };
                    await _context.StoryMedia.AddAsync(Photos);
                    await _context.SaveChangesAsync();
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                       await file.CopyToAsync(stream);
                    }
                }
            }
        }
        public async Task AddStoryAsPending(ShareStoryModel model, Story story)
        {
            story.Title = model.storyTitle;
            //story.UpdatedAt = viewmodel.date;
            story.Description = model.Story;
            story.Status = "PENDING";
            await _context.SaveChangesAsync();

            if (model.url is not null)
            {
                var existingMedia = _context.StoryMedia.Where(m => m.StoryId == story.StoryId && m.Type == "VIDEO");
                _context.StoryMedia.RemoveRange(existingMedia);

                foreach (var url in model.url)
                {
                    if (url is not null)
                    {
                        var VideoUrl = new StoryMedium
                        {
                            StoryId = story.StoryId,
                            Type = "VIDEO",
                            Path = url
                        };

                        _context.StoryMedia.Add(VideoUrl);
                        _context.SaveChanges();
                    }
                }
            }

            if (model.images is not null)
            {
                var existingMedia = _context.StoryMedia.Where(m => m.StoryId == story.StoryId && m.Type == "image");

                var oldMediaPaths = new List<string>();

                var oldMedia = _context.StoryMedia.Where(m => existingMedia.Select(s => s.StoryId).Contains(m.StoryId) && m.Type == "image").ToList();
                if (oldMedia.Count > 0)
                {
                    foreach (var media in oldMedia)
                    {
                        //var oldMediaPath = Path.Combine(Directory.GetCurrentDirectory(), "Upload", media.Path);
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

                foreach (var file in model.images)
                {
                    var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                    var fileExtension = Path.GetExtension(file.FileName);
                    var uniqueFileName = fileName + "_" + Guid.NewGuid().ToString() + fileExtension;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", uniqueFileName);
                    var Photos = new StoryMedium
                    {
                        StoryId = story.StoryId,
                        Type = "image",
                        Path = uniqueFileName
                    };
                    await _context.StoryMedia.AddAsync(Photos);
                    await _context.SaveChangesAsync();
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }
            }
        }

        public async Task<Story> GetStoryById(long id)
        {
            Story story = _context.Stories.Include(s => s.Mission).Include(s => s.User).Include(s => s.StoryMedia).Where(s => s.StoryId == id).FirstOrDefault();
            if (story.Status != "DRAFT")
            {
                story.Views += 1;
                await _context.SaveChangesAsync();
            }
            return story;
        }

        public async Task<List<User>> GetUsers(long userId)
        {
            return _context.Users.Where(u => u.UserId != userId).ToList();
        }
    }
}
