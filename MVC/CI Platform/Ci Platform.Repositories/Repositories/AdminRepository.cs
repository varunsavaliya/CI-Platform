using Ci_Platform.Repositories.Interfaces;
using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ci_Platform.Repositories.Repositories
{
    public class AdminRepository : IAdmin
    {
        private readonly ApplicationDbContext _context;

        public AdminRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<User> GetUsers()
        {
            return _context.Users.Where(user => user.Role == "User" && user.DeletedAt == null).ToList();
        }

        public AdminUserModel GetUserById(long userId)
        {
            User? user = _context.Users.Include(user => user.City).FirstOrDefault(user => user.UserId == userId);
            AdminUserModel model = new()
            {
                UserId = user.UserId,
                Name = user.FirstName,
                Surname = user.LastName,
                Email = user.Email,
                Password = user.Password,
                CountryId = user.CountryId == 0 ? null : user.CountryId,
                CityId = user.CityId == 0 ? null : user.CityId,
                employeeId = user.EmployeeId,
                Status = user.Status,
                department = user.Department,
                MyProfile = user.ProfileText
            };
            if (user.CountryId != 0 || user.CountryId != null)
            {
                model.cityList = _context.Cities.Where(city => city.CountryId == user.CountryId).ToList();
            }
            return model;
        }
        public async Task<string> DeleteUserById(long userId)
        {
            User? userTobeRemoved = _context.Users.FirstOrDefault(user => user.UserId == userId);
            userTobeRemoved.DeletedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return "User ";
        }
        public bool IsUserExists(string email)
        {
            return _context.Users.Any(user => user.Email == email);
        }
        public bool IsUserExists(string email, long userId)
        {
            return _context.Users.Any(u => u.UserId != userId && u.Email == email); ;
        }


        public async Task AddUser(AdminUserModel model)
        {
            User user = new()
            {
                FirstName = model.Name,
                LastName = model.Surname,
                Email = model.Email,
                Password = model.Password,
                CountryId = model.CountryId == 0 ? null : model.CountryId,
                CityId = model.CityId == 0 ? null : model.CityId,
                EmployeeId = model.employeeId,
                Status = model.Status,
                Department = model.department,
                ProfileText = model.MyProfile
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUser(AdminUserModel model)
        {
            User? user = _context.Users.FirstOrDefault(user => user.UserId == model.UserId);
            if (user != null)
            {
                user.FirstName = model.Name;
                user.LastName = model.Surname;
                user.Email = model.Email;
                user.Password = model.Password;
                user.CountryId = model.CountryId == 0 || model.CountryId == null ? null : model.CountryId;
                user.CityId = model.CityId == 0 || model.CityId == null ? null : model.CityId;
                user.EmployeeId = model.employeeId;
                user.Status = model.Status;
                user.Department = model.department;
                user.ProfileText = model.MyProfile;
                user.UpdatedAt = DateTime.Now;
            };
            await _context.SaveChangesAsync();
        }

        public List<CmsTable> GetCMSList()
        {
            return _context.CmsTables.ToList();
        }

        public AdminCMSModel GetCMSById(long cmsId)
        {
            CmsTable? cmsTable = _context.CmsTables.Find(cmsId);
            AdminCMSModel model = new()
            {
                CmsPageId = cmsTable.CmsPageId,
                Title = cmsTable.Title,
                Description = cmsTable.Description,
                Slug = cmsTable.Slug,
                Status = cmsTable.Status,
            };
            return model;
        }

        public async Task AddCMS(AdminCMSModel model)
        {
            CmsTable cms = new()
            {
                Title = model.Title,
                Description = model.Description,
                Slug = model.Slug,
                Status = model.Status,
            };

            await _context.CmsTables.AddAsync(cms);
            await _context.SaveChangesAsync();
        }

        public bool IsCMSExists(string slug)
        {
            return _context.CmsTables.Any(cms => cms.Slug == slug); ;
        }

        public bool IsCMSExists(string slug, long cmsPageId)
        {
            return _context.CmsTables.Any(cms => cms.CmsPageId != cmsPageId && cms.Slug == slug); ;
        }


        public async Task UpdateCMS(AdminCMSModel model)
        {
            CmsTable? cms = _context.CmsTables.FirstOrDefault(cms => cms.CmsPageId == model.CmsPageId);
            if (cms != null)
            {
                cms.Title = model.Title;
                cms.Description = model.Description;
                cms.Slug = model.Slug;
                cms.Status = model.Status;
                cms.UpdatedAt = DateTime.Now;
            };
            await _context.SaveChangesAsync();
        }


        public List<Mission> GetMissionList()
        {
            List<Mission> missions = _context.Missions.ToList();
            return missions;
        }

        public async Task<AdminMissionModel> GetMissionById(long missionId)
        {
            Mission? mission = await _context.Missions.Where(mission => mission.MissionId == missionId).Include(mission => mission.GoalMissions).FirstOrDefaultAsync();
            AdminMissionModel model = new()
            {
                MissionId = mission.MissionId,
                ThemeId = mission.ThemeId,
                CityId = mission.CityId,
                CityList = await _context.Cities.Where(city => city.CountryId == mission.CountryId).ToListAsync(),
                CountryId = mission.CountryId,
                Title = mission.Title,
                Description = mission.Description,
                ShortDescription = mission.ShortDescription,
                StartDate = mission.StartDate,
                EndDate = mission.EndDate,
                MissionType = mission.MissionType,
                MissionSkills = await _context.MissionSkills.Where(missionSkill => missionSkill.MissionId == missionId).Select(misssionSkill => misssionSkill.SkillId).ToListAsync(),
                Status = mission.Status,
                OrganizationName = mission.OrganizationName,
                OrganizationDetail = mission.OrganizationDetail,
                Availability = mission.Availability,
                TotalSeats = mission.TotalSeats,
                GoalObjectiveText = await _context.GoalMissions.Where(goalMission => goalMission.MissionId == missionId).Select(goalMission => goalMission.GoalObjectiveText).FirstOrDefaultAsync(),
                GoalValue = await _context.GoalMissions.Where(goalMission => goalMission.MissionId == missionId).Select(goalMission => goalMission.GoalValue).FirstOrDefaultAsync(),
                FileNames = await _context.MissionMedia.Where(missionMedia => missionMedia.MissionId == missionId && missionMedia.DefaultMedia == 0).Select(missionMedia => missionMedia.MediaPath).ToListAsync(),
                MissionDocsNames = await _context.MissionDocuments.Where(missiondocs => missiondocs.MissionId == missionId).Select(missiondocs => missiondocs.DocumentPath).ToListAsync(),
                DefaultImageName = await _context.MissionMedia.Where(missionMedia => missionMedia.MissionId == missionId && missionMedia.DefaultMedia == 1).Select(missionMedia => missionMedia.MediaPath).FirstOrDefaultAsync(),
            };
            return model;
        }

        public async Task DeleteImages(long missionId)
        {
            var existingMedia = _context.MissionMedia.Where(missionMedia => missionMedia.MissionId == missionId && missionMedia.MediaType == "image");
            List<string> oldMediaPaths = new();
            var oldMedia = _context.MissionMedia.Where(missionMedia => existingMedia.Select(media => media.MissionId).Contains(missionMedia.MissionId) && missionMedia.MediaType == "image").ToList();
            if (oldMedia.Count > 0)
            {
                foreach (var media in oldMedia)
                {
                    string oldMediaPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "MissionImages", media.MediaPath);

                    oldMediaPaths.Add(oldMediaPath);
                }
            }
            // delete the previous images from the server's directory
            foreach (var file in Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "MissionImages")))
            {
                if (oldMediaPaths.Contains(file))
                {
                    System.IO.File.Delete(file);
                }
            }
            _context.RemoveRange(existingMedia);
            await _context.SaveChangesAsync();
        }

        public async Task AddImages(List<IFormFile> model, IFormFile defaultImage, long missionId)
        {
            var mediaCount = 1;
            foreach (var file in model)
            {
                try
                {

                    var fileExtension = Path.GetExtension(file.FileName);
                    var fileName = "mission_" + missionId + "_image_" + mediaCount + fileExtension;
                    mediaCount++;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "MissionImages", fileName);
                    MissionMedium image = new()
                    {
                        MissionId = missionId,
                        MediaName = fileName,
                        MediaType = "image",
                        MediaPath = fileName,
                        DefaultMedia = 0,
                    };
                    await _context.MissionMedia.AddAsync(image);
                    await _context.SaveChangesAsync();
                    using var stream = new FileStream(filePath, FileMode.Create);
                    await file.CopyToAsync(stream);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            if (defaultImage != null)
            {
                var fileExtension = Path.GetExtension(defaultImage.FileName);
                var fileName = "mission_" + missionId + "_defaultImage_" + fileExtension;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "MissionImages", fileName);
                MissionMedium defImage = new()
                {
                    MissionId = missionId,
                    MediaName = fileName,
                    MediaType = "image",
                    MediaPath = fileName,
                    DefaultMedia = 1,
                };
                await _context.MissionMedia.AddAsync(defImage);
                await _context.SaveChangesAsync();
                using var stream = new FileStream(filePath, FileMode.Create);
                await defaultImage.CopyToAsync(stream);
            }
        }

        public async Task DeleteMissionDocs(long missionId)
        {
            var existingDocs = _context.MissionDocuments.Where(missionDocs => missionDocs.MissionId == missionId);
            List<string> oldDocsPaths = new();
            var oldDocs = _context.MissionDocuments.Where(missionDocs => existingDocs.Select(media => media.MissionId).Contains(missionDocs.MissionId)).ToList();
            if (oldDocs.Count > 0)
            {
                foreach (var doc in oldDocs)
                {
                    string oldDocPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "MissionDocuments", doc.DocumentPath);

                    oldDocsPaths.Add(oldDocPath);
                }
            }
            // delete the previous images from the server's directory
            foreach (var file in Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "MissionDocuments")))
            {
                if (oldDocsPaths.Contains(file))
                {
                    System.IO.File.Delete(file);
                }
            }
            _context.RemoveRange(existingDocs);
            await _context.SaveChangesAsync();
        }

        public async Task AddMissionDocs(List<IFormFile> model, long missionId)
        {
            foreach (var file in model)
            {
                var fileExtension = Path.GetExtension(file.FileName);
                var fileName = file.FileName;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "MissionDocuments", fileName);
                MissionDocument missionDocument = new()
                {
                    MissionId = missionId,
                    DocumentName = fileName,
                    DocumentPath = fileName,
                };
                await _context.MissionDocuments.AddAsync(missionDocument);
                await _context.SaveChangesAsync();
                using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);
            }
        }
        public async Task AddMission(AdminMissionModel model)
        {
            Mission newMission = new()
            {
                Title = model.Title,
                ShortDescription = model.ShortDescription,
                Description = model.Description,
                CityId = model.CityId,
                CountryId = model.CountryId,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                MissionType = model.MissionType,
                OrganizationDetail = model.OrganizationDetail,
                OrganizationName = model.OrganizationName,
                Status = model.Status,
                Availability = model.Availability,
                TotalSeats = model.TotalSeats,
                ThemeId = model.ThemeId,
            };
            await _context.Missions.AddAsync(newMission);
            await _context.SaveChangesAsync();

            long missionId = await _context.Missions.Where(mission => mission.Title == model.Title).Select(mission => mission.MissionId).FirstOrDefaultAsync();

            if (model.MissionType == "Goal")
            {
                GoalMission goalMission = new()
                {
                    MissionId = missionId,
                    GoalObjectiveText = model.GoalObjectiveText,
                    GoalValue = model.GoalValue,
                };
                await _context.GoalMissions.AddAsync(goalMission);
                await _context.SaveChangesAsync();
            }

            if (model.MissionSkills != null)
            {
                foreach (var skillId in model.MissionSkills)
                {
                    MissionSkill missionSkill = new()
                    {
                        MissionId = missionId,
                        SkillId = skillId,
                    };
                    await _context.MissionSkills.AddAsync(missionSkill);
                    await _context.SaveChangesAsync();
                }
            }

            if (model.Files != null || model.DefaultImage != null)
            {
                await AddImages(model.Files, model.DefaultImage, missionId);
            }
            if (model.MissionDocs != null)
            {
                await AddMissionDocs(model.MissionDocs, missionId);
            }

        }

        public async Task UpdateMission(AdminMissionModel model, long missionId)
        {
            Mission? mission = await _context.Missions.FindAsync(missionId);
            mission.Title = model.Title;
            mission.ShortDescription = model.ShortDescription;
            mission.Description = model.Description;
            mission.CityId = model.CityId;
            mission.CountryId = model.CountryId;
            mission.StartDate = model.StartDate;
            mission.EndDate = model.EndDate;
            mission.MissionType = model.MissionType;
            mission.OrganizationDetail = model.OrganizationDetail;
            mission.OrganizationName = model.OrganizationName;
            mission.Status = model.Status;
            mission.Availability = model.Availability;
            mission.TotalSeats = model.TotalSeats;
            mission.ThemeId = model.ThemeId;
            mission.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            if (model.MissionType == "Goal")
            {
                GoalMission? goalMission = await _context.GoalMissions.FirstOrDefaultAsync(goalMission => goalMission.MissionId == missionId);
                goalMission.GoalObjectiveText = model.GoalObjectiveText;
                goalMission.GoalValue = model.GoalValue;
                await _context.SaveChangesAsync();
            }

            if (model.MissionSkills != null)
            {
                var existingSkills = _context.MissionSkills.Where(missionSkill => missionSkill.MissionId == missionId).ToList();
                _context.MissionSkills.RemoveRange(existingSkills);
                await _context.SaveChangesAsync();
                foreach (var skillId in model.MissionSkills)
                {
                    MissionSkill missionSkill = new()
                    {
                        MissionId = missionId,
                        SkillId = skillId,
                    };
                    await _context.MissionSkills.AddAsync(missionSkill);
                    await _context.SaveChangesAsync();
                }
            }

            await DeleteImages(missionId);
            if (model.Files != null || model.DefaultImage != null)
            {
                await AddImages(model.Files, model.DefaultImage, missionId);
            }
            await DeleteMissionDocs(missionId);
            if (model.MissionDocs != null)
            {
                await AddMissionDocs(model.MissionDocs, missionId);
            }

        }
    }
}
