using Ci_Platform.Repositories.Interfaces;
using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Ci_Platform.Repositories.Repositories
{
    public class MissionRepository : Repository<NotificationData>, IMission
    {
        private new readonly ApplicationDbContext _context;

        public MissionRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public (List<MissionCard> missionList, int totalRecords) GetMissionCards(InputData queryParams, long userId)
        {
            var query = _context.Missions.Where(mission => mission.DeletedAt == null && mission.Status == 1 && mission.Theme.Status == 1 && mission.Theme.DeletedAt == null).AsQueryable();

            //query = query.Where(mission => mission.MissionSkills.Where(skill => skill.Skill.Status == 1));

            if (queryParams.CityIds.Any())
                query = query.Where(mission => queryParams.CityIds.Contains(mission.CityId));

            if (!queryParams.CityIds.Any() && queryParams.CountryId != 0)
                query = query.Where(mission => mission.CountryId == queryParams.CountryId);

            if (queryParams.ThemeIds.Any())
                query = query.Where(mission => queryParams.ThemeIds.Contains(mission.ThemeId));

            if (queryParams.SkillIds.Any())
                query = query.Where(mission => mission.MissionSkills.Any(skill => queryParams.SkillIds.Contains(skill.SkillId)));

            if (!string.IsNullOrWhiteSpace(queryParams.searchText))
                query = query.Where(mission => mission.Title.ToLower().Contains(queryParams.searchText.ToLower()));

            var missionCardQuery = query.Select(mission => new MissionCard()
            {
                CityName = mission.City.Name,
                Missiondata = mission,
                HasApplied = mission.MissionApplications.Any(missionApplication => missionApplication.UserId == userId && missionApplication.DeletedAt == null),
                IsFavourite = mission.FavoriteMissions.Any(fav => fav.UserId == userId && fav.DeletedAt == null),
                MissionMedia = mission.MissionMedia.Where(missionMedia => missionMedia.DeletedAt == null).OrderByDescending(missionMedia => missionMedia.DefaultMedia).Select(missionMedia => missionMedia.MediaPath).FirstOrDefault(),
                rating = (float?)mission.MissionRatings.Average(rating => rating.Rating) == null ? 0 : (float?)mission.MissionRatings.Average(rating => rating.Rating),
                seatsleft = mission.TotalSeats - mission.MissionApplications.Count(missionApplication => missionApplication.ApprovalStatus == "PUBLISHED" && missionApplication.DeletedAt == null),
                ThemeName = mission.Theme.Title,
                IsDeadlinePassed = mission.StartDate.Value.AddDays(-1) < DateTime.Now,
                IsEnddatePassed = mission.EndDate < DateTime.Now,
                IsOngoing = (mission.StartDate < DateTime.Now) && (mission.EndDate > DateTime.Now),
                goalObjectiveText = mission.GoalMissions.Select(goalMission => goalMission.GoalObjectiveText).FirstOrDefault(),
                AchievedGoal = mission.Timesheets.Where(achievedGoal => achievedGoal.Status == "APPROVED").Sum(achievedGoal => achievedGoal.Action),
                Goalvalue = mission.GoalMissions.Select(goalMission => goalMission.GoalValue).FirstOrDefault(),
            });


            switch (queryParams.SortBy)
            {
                case "SeatsLeft":
                    missionCardQuery = queryParams.SortOrder == "Desc" ? missionCardQuery.Where(mission => !mission.IsEnddatePassed).OrderByDescending(query => query.seatsleft) : missionCardQuery.Where(mission => !mission.IsEnddatePassed).OrderBy(query => query.seatsleft);
                    break;
                case "StartDate":
                    missionCardQuery = missionCardQuery.Where(mission => !mission.IsEnddatePassed).OrderBy(query => query.Missiondata.StartDate);
                    break;
                case "Favorites":
                    missionCardQuery = missionCardQuery.Where(mission => mission.IsFavourite);
                    break;
                case "CreatedAt":
                    missionCardQuery = queryParams.SortOrder == "Desc" ? missionCardQuery.OrderByDescending(query => query.Missiondata.CreatedAt) : missionCardQuery.OrderBy(query => query.Missiondata.CreatedAt);
                    break;
            }

            switch (queryParams.Explore)
            {
                case "Top Favourite":
                    missionCardQuery = missionCardQuery.OrderByDescending(fav => fav.Missiondata.FavoriteMissions.Count()).Take(6);
                    break;
                case "Most Ranked":
                    missionCardQuery = missionCardQuery.OrderByDescending(ranked => ranked.rating).Take(6);
                    break;
                case "Random":
                    missionCardQuery = missionCardQuery.Take(6);
                    break;
                case "Top Themes":
                    var topThemes = _context.Missions
                        .Where(mission => mission.DeletedAt == null && mission.Status == 1 && mission.Theme.Status == 1 && mission.Theme.DeletedAt == null)
                        .GroupBy(mission => mission.ThemeId)
                        .Select(group => new { ThemeId = group.Key, Count = group.Count() })
                        .OrderByDescending(themeCount => themeCount.Count)
                        .Take(3)
                        .Select(themeCount => themeCount.ThemeId);

                    missionCardQuery = missionCardQuery
                    .Where(mission => topThemes.Contains(mission.Missiondata.ThemeId));
                    break;
            }

            var records = missionCardQuery
                .Skip((queryParams.pageNo - 1) * queryParams.pageSize)
                .Take(queryParams.pageSize)
                .ToList();

            return (records, missionCardQuery.Count());
        }

        public async Task HandleFav(long missionId, long userId)
        {
            // Check if the mission is already in favorites for the user
            if (_context.FavoriteMissions.Any(fm => fm.MissionId == missionId && fm.UserId == userId))
            {
                // Mission is already in favorites, return an error message or redirect back to the mission page
                var FavoriteMissionId = await _context.FavoriteMissions.Where(fm => fm.MissionId == missionId && fm.UserId == userId).FirstOrDefaultAsync();
                _context.FavoriteMissions.Remove(FavoriteMissionId);
                await _context.SaveChangesAsync();
            }
            else
            {
                // Add the mission to favorites for the user
                var favoriteMission = new FavoriteMission { MissionId = missionId, UserId = userId };
                _context.FavoriteMissions.Add(favoriteMission);
                _context.SaveChanges();
            }
        }
        public MissionCard GetMissionVolunteeringData(long missionId, long userId)
        {
            var query = _context.Missions.Where(mission => mission.MissionId == missionId).AsQueryable();
            var missionDetailsQuery = query.Select(mission => new MissionCard()
            {
                Missiondata = mission,
                CityName = mission.City.Name,
                ThemeName = mission.Theme.Title,
                HasApplied = mission.MissionApplications.Any(missionApplication => missionApplication.UserId == userId && missionApplication.DeletedAt == null),
                IsFavourite = mission.FavoriteMissions.Any(fav => fav.UserId == userId && fav.DeletedAt == null),
                IsDeadlinePassed = mission.StartDate.Value.AddDays(-1) < DateTime.Now,
                IsEnddatePassed = mission.EndDate < DateTime.Now,
                IsOngoing = (mission.StartDate < DateTime.Now) && (mission.EndDate > DateTime.Now),
                goalObjectiveText = mission.GoalMissions.Select(goalMission => goalMission.GoalObjectiveText).FirstOrDefault(),
                seatsleft = mission.TotalSeats - mission.MissionApplications.Count(missionApplication => missionApplication.ApprovalStatus == "PUBLISHED" && missionApplication.DeletedAt == null),
                AchievedGoal = mission.Timesheets.Where(achievedGoal => achievedGoal.Status == "APPROVED").Sum(achievedGoal => achievedGoal.Action),
                Goalvalue = mission.GoalMissions.Select(goalMission => goalMission.GoalValue).FirstOrDefault(),
                MissionComments = mission.Comments.Where(comment => comment.ApprovalStatus == "PUBLISHED").Select(comment => new MissionComment()
                {
                    CommentData = comment,
                    UserAvatar = comment.User.Avatar == null ? "default user avatar.jpg" : comment.User.Avatar,
                    UserName = comment.User.FirstName + " " + comment.User.LastName,
                }).ToList(),
                MissionSkills = mission.MissionSkills.Select(skill => skill.Skill.SkillName).ToList(),
                rating = mission.MissionRatings.Average(rating => rating.Rating) == null ? 0 : mission.MissionRatings.Average(rating => rating.Rating),
                MissionAllMedia = mission.MissionMedia.Where(missionMedia => missionMedia.DeletedAt == null).OrderByDescending(missionMedia => missionMedia.DefaultMedia).ToList(),
                TotalUserRated = mission.MissionRatings.Count(),
            });
            return missionDetailsQuery.FirstOrDefault();
        }

        public List<MissionCard> GetRelatedMission(long missionId, long userId)
        {
            Mission? currentMission = _context.Missions.Find(missionId);
            var query = _context.Missions.Where(mission => mission.MissionId != missionId).AsQueryable();

            var reletedMissionQuery = query.Where(mission => mission.CityId == currentMission.CityId);

            if (reletedMissionQuery.Count() < 3)
                reletedMissionQuery = query.Where(mission => mission.MissionId != missionId && (mission.CityId == currentMission.CityId || mission.ThemeId == currentMission.ThemeId));

            if (reletedMissionQuery.Count() < 3)
                reletedMissionQuery = query.Where(mission => mission.MissionId != missionId && (mission.CityId == currentMission.CityId || mission.ThemeId == currentMission.ThemeId || mission.CountryId == currentMission.CountryId));

            var reletedMissionCardsQuery = reletedMissionQuery.Select(mission => new MissionCard()
            {
                CityName = mission.City.Name,
                Missiondata = mission,
                HasApplied = mission.MissionApplications.Any(missionApplication => missionApplication.UserId == userId && missionApplication.DeletedAt == null),
                IsFavourite = mission.FavoriteMissions.Any(fav => fav.UserId == userId && fav.DeletedAt == null),
                MissionMedia = mission.MissionMedia.Where(missionMedia => missionMedia.DeletedAt == null).OrderByDescending(missionMedia => missionMedia.DefaultMedia).Select(missionMedia => missionMedia.MediaPath).FirstOrDefault(),
                rating = (float?)mission.MissionRatings.Average(rating => rating.Rating) == null ? 0 : (float?)mission.MissionRatings.Average(rating => rating.Rating),
                seatsleft = mission.TotalSeats - mission.MissionApplications.Count(missionApplication => missionApplication.ApprovalStatus == "PUBLISHED" && missionApplication.DeletedAt == null),
                ThemeName = mission.Theme.Title,
                IsDeadlinePassed = mission.StartDate.Value.AddDays(-1) < DateTime.Now,
                IsEnddatePassed = mission.EndDate < DateTime.Now,
                IsOngoing = (mission.StartDate < DateTime.Now) && (mission.EndDate > DateTime.Now),
                goalObjectiveText = mission.GoalMissions.Select(goalMission => goalMission.GoalObjectiveText).FirstOrDefault(),
                AchievedGoal = mission.Timesheets.Where(achievedGoal => achievedGoal.Status == "APPROVED").Sum(achievedGoal => achievedGoal.Action),
                Goalvalue = mission.GoalMissions.Select(goalMission => goalMission.GoalValue).FirstOrDefault(),
            });

            return reletedMissionCardsQuery.Take(3).ToList();
        }

        public async Task<int> GetTotalVolunteers(long missionId)
        {
            return await _context.MissionApplications.CountAsync(ma => ma.MissionId == missionId && ma.ApprovalStatus == "PUBLISHED");
        }

        public async Task<List<string>> GetMissionDocs(long missionId)
        {
            return await _context.MissionDocuments.Where(doc => doc.MissionId == missionId).Select(doc => doc.DocumentPath).ToListAsync();
        }
        public async Task<int> GetUserMissionRating(long missionId, long userId)
        {
            return await _context.MissionRatings.Where(missionRating => missionRating.UserId == userId && missionRating.MissionId == missionId).Select(missionRating => missionRating.Rating).FirstOrDefaultAsync() == null ? 0 : await _context.MissionRatings.Where(missionRating => missionRating.UserId == userId && missionRating.MissionId == missionId).Select(missionRating => missionRating.Rating).FirstOrDefaultAsync();
        }

        public async Task<List<User>> GetUsersList(long userId)
        {
            return await _context.Users.Where(user => user.UserId != userId && user.DeletedAt == null && user.Status == 1).ToListAsync();
        }
        public async Task<List<MissionApplication>> GetRecentVolByPage(long missionId, int pageNo, int pageSize)
        {
            var recentVolunteers = await _context.MissionApplications
                .Where(ma => ma.MissionId == missionId && ma.ApprovalStatus == "PUBLISHED")
                .Include(ma => ma.User)
                .OrderByDescending(ma => ma.CreatedAt)
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return recentVolunteers;
        }
        public async Task HandleRatings(long missionId, long userId, int rating)
        {
            MissionRating? missionRating = await _context.MissionRatings.FirstOrDefaultAsync(mr => mr.MissionId == missionId && mr.UserId == userId);

            // if mission rating is not there by this user then add it
            if (missionRating == null)
            {
                missionRating = new MissionRating
                {
                    MissionId = missionId,
                    UserId = userId
                };
                await _context.AddAsync(missionRating);
            }

            // Update the rating in the database if rating is already there
            missionRating.Rating = rating;
            await _context.SaveChangesAsync();
        }

        public async Task HandleMissionApply(long missionId, long userId)
        {
            MissionApplication application = new();
            application.MissionId = missionId;
            application.UserId = userId;
            await _context.MissionApplications.AddAsync(application);
            await _context.SaveChangesAsync();
        }
        public async Task HandleComment(Comment comment, long userId)
        {
            Comment commentToBeAdded = new()
            {
                UserId = userId,
                Comment1 = comment.Comment1,
                MissionId = comment.MissionId,
            };
            await _context.Comments.AddAsync(commentToBeAdded);
            await _context.SaveChangesAsync();
        }

        public async Task SaveInviteData(long toUserId, long missionId, long fromUserId)
        {
            NotificationData notification = new()
            {
                UserId = toUserId,
                ToUserId = toUserId,
                FromUserId = fromUserId,
                MissionId = missionId,
                NotificationSettingsId = 1,
                Status = false,
            };
            await AddNotitifcationData(notification);
            var missionInvite = new MissionInvite()
            {
                FromUserId = fromUserId,
                ToUserId = toUserId,
                MissionId = missionId,
            };

            await _context.MissionInvites.AddAsync(missionInvite);
            await _context.SaveChangesAsync();
        }
    }
}
