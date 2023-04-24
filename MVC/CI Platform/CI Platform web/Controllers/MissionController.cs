using Ci_Platform.Repositories.Interfaces;
using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CI_Platform_web.Controllers
{
    public class MissionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFilters _filters;
        private readonly IMission _mission;
        private readonly ISendInvite<MissionVolunteeringModel> _sendInvite;

        public MissionController(ApplicationDbContext context, IFilters filters, IMission mission)
        {
            _context = context;
            _filters = filters;
            _mission = mission;
        }

        public async Task<IActionResult> LandingPage()
        {
            if (HttpContext.Session.GetString("UserId") == null)
            {
                string returnUrl = Url.Action("LandingPage", "Mission");
                return RedirectToAction("Index", "Home", new { returnUrl });
            }

            var LandingView = new LandingPageModel();
            LandingView.Country = await _filters.GetCountriesAsync();
            LandingView.Theme = await _filters.GetThemesAsync();
            LandingView.Skill = await _filters.GetSkillsAsyc();
            return View(LandingView);
        }

        [HttpPost]
        public async Task<IActionResult> LandingPage(InputData inputData)
        {
            long UserId = 0;
            if (HttpContext.Session.GetString("UserName") != null)
            {
                UserId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
                ViewBag.UserId = UserId;
            }

            var (missionList, totalRecords) = _mission.GetMissionCards(inputData, UserId);
            LandingPageModel model = new()
            {
                MissionList = missionList,
                Users = await _filters.GetUsers(),
                totalRecords = totalRecords,
            };

            return PartialView("_GridListPartial", model);
        }

        public async Task<IActionResult> GetCitiesByCountry(int countryId)
        {
            var cities = await _filters.GetCitiesByCountryAsync(countryId);
            return Json(cities);
        }

        [HttpPost]
        public IActionResult AddToFavorites(int missionId)
        {
            string Id = HttpContext.Session.GetString("UserId");
            long userId = long.Parse(Id);

            // Check if the mission is already in favorites for the user
            if (_context.FavoriteMissions.Any(fm => fm.MissionId == missionId && fm.UserId == userId))
            {
                // Mission is already in favorites, return an error message or redirect back to the mission page
                var FavoriteMissionId = _context.FavoriteMissions.Where(fm => fm.MissionId == missionId && fm.UserId == userId).FirstOrDefault();
                _context.FavoriteMissions.Remove(FavoriteMissionId);
                _context.SaveChanges();
                return Ok();
            }

            // Add the mission to favorites for the user
            var favoriteMission = new FavoriteMission { MissionId = missionId, UserId = userId };
            _context.FavoriteMissions.Add(favoriteMission);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost]
        public IActionResult AddComment(Comment formData)
        {
            _context.Comments.Add(formData);
            _context.SaveChanges();
            return View();
        }

        public IActionResult MissionVolunteering(int id)
        {
            long UserId = 0;
            if (HttpContext.Session.GetString("UserId") == null)
            {
                string returnUrl = Url.Action("MissionVolunteering", "Mission");
                return RedirectToAction("Index", "Home", new { returnUrl });
            }
            else
            {
                UserId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));

            }

            // Retrieve the mission detail for the given ID
            Mission missionDetail = _context.Missions.Include(m => m.MissionApplications)
                                                     .ThenInclude(ma => ma.User)
                                                     .Include(m => m.MissionRatings)
                                                     .Include(m => m.City)
                                                     .Include(m => m.Theme)
                                                     .Include(m => m.FavoriteMissions)
                                                     .Include(m => m.GoalMissions)
                                                     .Include(m => m.Comments).ThenInclude(c => c.User)
                                                     .Include(m => m.MissionSkills).ThenInclude(ms => ms.Skill)
                                                     .FirstOrDefault(m => m.MissionId == id);

            if (missionDetail == null)
            {
                return NotFound();
            }

            // Retrieve the related missions
            var relatedMissions = _context.Missions.Include(m => m.City)
                                                    .Include(m => m.Theme)
                                                    .Include(m => m.Country)
                                                    .Include(m => m.MissionApplications)
                                                    .Include(m => m.MissionRatings)
                                                     .Include(m => m.FavoriteMissions)
                                                     .Include(m => m.GoalMissions)
                                                     .Include(m => m.Comments).ThenInclude(c => c.User)
                                                     .Include(m => m.MissionSkills).ThenInclude(ms => ms.Skill)
                                                    .Where(m => m.MissionId != id && m.CityId == missionDetail.CityId)
                                                    .Take(3)
                                                    .ToList();

            // If there are not enough related missions based on city, retrieve based on theme
            if (relatedMissions.Count() < 3)
            {
                var additionalMissions = _context.Missions.Include(m => m.City)
                                                          .Include(m => m.Theme)
                                                          .Where(m => m.MissionId != id && m.ThemeId == missionDetail.ThemeId && !relatedMissions.Contains(m))
                                                          .Include(m => m.Country)
                                                    .Include(m => m.MissionApplications)
                                                    .Include(m => m.MissionRatings)
                                                     .Include(m => m.FavoriteMissions)
                                                     .Include(m => m.GoalMissions)
                                                     .Include(m => m.Comments).ThenInclude(c => c.User)
                                                     .Include(m => m.MissionSkills).ThenInclude(ms => ms.Skill)
                                                          .Take(3 - relatedMissions.Count())
                                                          .ToList();
                relatedMissions.AddRange(additionalMissions);
            }

            // If there are still not enough related missions, retrieve based on country
            if (relatedMissions.Count() < 3)
            {
                var additionalMissions = _context.Missions.Include(m => m.City)
                                                          .Include(m => m.Theme)
                                                          .Where(m => m.MissionId != id && m.CountryId == missionDetail.CountryId && !relatedMissions.Contains(m))
                                                          .Include(m => m.Country)
                                                    .Include(m => m.MissionApplications)
                                                    .Include(m => m.MissionRatings)
                                                     .Include(m => m.FavoriteMissions)
                                                     .Include(m => m.GoalMissions)
                                                     .Include(m => m.Comments).ThenInclude(c => c.User)
                                                     .Include(m => m.MissionSkills).ThenInclude(ms => ms.Skill)
                                                          .Take(3 - relatedMissions.Count())
                                                          .ToList();
                relatedMissions.AddRange(additionalMissions);
            }

            // Create the ViewModel and pass it to the view
            var missionVolunteeringModel = new MissionVolunteeringModel
            {
                mission = missionDetail,
                RelatedMissions = relatedMissions,
                totalVolunteers = _context.MissionApplications.Where(ma => ma.MissionId == id && ma.ApprovalStatus == "PUBLISHED").Count(),
                MissionDocs = _context.MissionDocuments.Where(doc => doc.MissionId == id).Select(doc => doc.DocumentPath).ToList(),
            };
                missionVolunteeringModel.UserList = _context.Users.Where(u => u.UserId != UserId || u.MissionApplications.Any(ma => ma.MissionId == id && ma.ApprovalStatus != "PUBLISHED")).ToList();
            return View(missionVolunteeringModel);
        }

        public IActionResult DisplayDoc(string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/MissionDocuments", fileName);
            var fileStream = new FileStream(filePath, FileMode.Open);
            var contentType = "application/pdf";
            return File(fileStream, contentType);
        }

        [HttpGet]
        public IActionResult GetRecentVolunteers(long missionId, int pageNo, int pageSize)
        {
            MissionVolunteeringModel model = new()
            {
                recentVolunteers = _context.MissionApplications
                .Where(ma => ma.MissionId == missionId && ma.ApprovalStatus == "PUBLISHED")
                .Include(ma => ma.User)
                .OrderByDescending(ma => ma.CreatedAt)
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .ToList(),
            };
            return PartialView("_RecentVolunteers", model);
        }


        [HttpPost]
        public async Task<IActionResult> MissionInvite(long ToUserId, long Id, long FromUserId, MissionVolunteeringModel viewmodel)
        {
            long UserId = 0;
            if (HttpContext.Session.GetString("UserName") != null)
            {
                UserId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
                ViewBag.UserId = UserId;
            }
            var MissionLink = Url.Action("MissionVolunteering", "Mission", new { id = Id }, Request.Scheme);
            await _mission.SendEmailInvite(ToUserId, Id, UserId, MissionLink, viewmodel);
            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult UpdateRating(int missionId, int userId, int rating)
        {
            MissionRating? missionRating = _context.MissionRatings.SingleOrDefault(mr => mr.MissionId == missionId && mr.UserId == userId);

            // if mission rating is not there by this user then add it
            if (missionRating == null)
            {
                missionRating = new MissionRating
                {
                    MissionId = missionId,
                    UserId = userId
                };
                _context.Add(missionRating);
            }

            // Update the rating in the database if rating is already there
            missionRating.Rating = rating;
            _context.SaveChanges();
            return Ok(); // Return a success status code
        }

        [HttpPost]
        public IActionResult Apply(long missionId, long userId)
        {
            MissionApplication application = new();
            application.MissionId = missionId;
            application.UserId = userId;
            _context.MissionApplications.Add(application);
            _context.SaveChanges();
            return Ok();
        }
        public IActionResult StoriesListing()
        {
            return View();
        }
    }
}
