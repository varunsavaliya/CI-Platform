using Ci_Platform.Repositories.Interfaces;
using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;

namespace Ci_Platform.Repositories.Repositories
{
    public class MissionRepository : SendInvite<MissionVolunteeringModel>, IMission
    {
        private readonly ApplicationDbContext _context;

        public MissionRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public (List<MissionCard> missionList, int totalRecords) GetMissionCards(InputData queryParams, long userId)
        {
            var query = _context.Missions.Where(mission => mission.DeletedAt == null).AsQueryable();

            if(queryParams.CityIds.Any())
                query = query.Where(mission => queryParams.CityIds.Contains(mission.CityId));

            if(!queryParams.CityIds.Any() && queryParams.CountryId != 0)
                query = query.Where(mission => mission.CountryId == queryParams.CountryId);

            if(queryParams.ThemeIds.Any())
                query = query.Where(mission => queryParams.ThemeIds.Contains(mission.ThemeId));

            if(queryParams.SkillIds.Any())
                query = query.Where(mission => mission.MissionSkills.Any(skill => queryParams.SkillIds.Contains(skill.SkillId)));

            if(!string.IsNullOrWhiteSpace(queryParams.searchText))
                query = query.Where(mission => mission.Title.ToLower().Contains(queryParams.searchText.ToLower()));

            var missionCardQuery = query.Select(mission => new MissionCard()
            {
                CityName = mission.City.Name,
                Missiondata = mission,
                HasApplied = mission.MissionApplications.Any(missionApplication => missionApplication.UserId == userId && missionApplication.DeletedAt == null),
                IsFavourite = mission.FavoriteMissions.Any(fav => fav.UserId == userId && fav.DeletedAt == null),
                //DefaultMedia = mission.MissionMedia.Where(missionMedia => missionMedia.DeletedAt == null && missionMedia.DefaultMedia == 1).Select(missionMedia => missionMedia.MediaPath).FirstOrDefault(),
                MissionMedia = mission.MissionMedia.Where(missionMedia => missionMedia.DeletedAt == null).OrderByDescending(missionMedia => missionMedia.DefaultMedia).Select(missionMedia => missionMedia.MediaPath).FirstOrDefault(),
                rating =(float?) mission.MissionRatings.Average(rating => rating.Rating) == null? 0 : (float?)mission.MissionRatings.Average(rating => rating.Rating),
                seatsleft = mission.TotalSeats - mission.MissionApplications.Count(missionApplication => missionApplication.ApprovalStatus == "PUBLISHED" && missionApplication.DeletedAt == null),
                ThemeName = mission.Theme.Title,
                IsDeadlinePassed = mission.StartDate.Value.AddDays(-1) < DateTime.Now,
                IsEnddatePassed = mission.EndDate < DateTime.Now,
                IsOngoing = (mission.StartDate < DateTime.Now) && (mission.EndDate > DateTime.Now),
                goalObjectiveText = mission.GoalMissions.Select(goalMission => goalMission.GoalObjectiveText).FirstOrDefault(),
                AchievedGoal = mission.Timesheets.Sum(achievedGoal => achievedGoal.Action),
                Goalvalue = mission.GoalMissions.Select(goalMission => goalMission.GoalValue).FirstOrDefault(),
            });

            switch (queryParams.SortBy)
            {
                case "SeatsLeft":
                    missionCardQuery = queryParams.SortOrder == "Desc" ? missionCardQuery.Where(mission => !mission.IsEnddatePassed).OrderByDescending(query => query.seatsleft) : missionCardQuery.Where(mission => !mission.IsEnddatePassed).OrderBy(query => query.seatsleft);
                    break;
                case "StartDate":
                    missionCardQuery =missionCardQuery.Where(mission => !mission.IsEnddatePassed).OrderBy(query => query.Missiondata.StartDate);
                    break;
                case "Favorites":
                    missionCardQuery  = missionCardQuery.Where(mission => mission.IsFavourite);
                    break;
                default:
                    missionCardQuery = queryParams.SortOrder == "Desc" ? missionCardQuery.OrderByDescending(query => query.Missiondata.CreatedAt) : missionCardQuery.OrderBy(query => query.Missiondata.CreatedAt);
                    break;
            }

            var records = missionCardQuery
                .Skip((queryParams.pageNo - 1) * queryParams.pageSize)
                .Take(queryParams.pageSize)
                .ToList();

            return (records, missionCardQuery.Count());
        }
    }
}
