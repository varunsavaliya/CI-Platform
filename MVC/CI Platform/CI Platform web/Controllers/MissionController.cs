using Ci_Platform.Repositories.Interfaces;
using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CI_Platform_web.Controllers
{
    public class MissionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFilters _filters;

        public MissionController(ApplicationDbContext context, IFilters filters)
        {
            _context = context;
            _filters = filters;
        }

        public async Task<IActionResult> LandingPage()
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                ViewBag.UserName = HttpContext.Session.GetString("UserName");
                ViewBag.IsLoggedIn = HttpContext.Session.GetString("IsLoggedIn");
                ViewBag.UserId = HttpContext.Session.GetString("UserId");
            }
            else
            {
                ViewBag.UserName = "Login";
            }

            // methods for country, theme and skills is in IFilters
            //var country = await _filters.GetCountriesAsync();
            //ViewBag.countryList = country;

            //var theme = await _filters.GetThemesAsync();
            //ViewBag.themeList = theme;

            //var skill = await _filters.GetSkillsAsyc();
            //ViewBag.skillList = skill;

            //// card integration 
            //var missions = _context.Missions.Include(m => m.City).Include(m => m.Theme).Include(m => m.MissionRatings).ToList();
            //ViewBag.missionList = missions;

            var LandingView = new LandingPageModel();


            LandingView.Country = await _filters.GetCountriesAsync();
            LandingView.Theme = await _filters.GetThemesAsync();
            LandingView.Skill = await _filters.GetSkillsAsyc();
            LandingView.MissionList = _context.Missions.Include(m => m.City).Include(m => m.Theme).Include(m => m.MissionRatings).Include(m => m.MissionSkills).ThenInclude(ms => ms.Skill).Include(m => m.GoalMissions).Include(m => m.MissionApplications).Include(m => m.FavoriteMissions).ToList();


            //foreach (var mission in missions)
            //{
            //    double Rating = mission.MissionRatings.Rating;
            //    ViewBag.AverageRatings[mission.MissionId] = averageRating;
            //}

            return View(LandingView);
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

                //return BadRequest("Mission is already in favorites.");
            }

            // Add the mission to favorites for the user
            var favoriteMission = new FavoriteMission { MissionId = missionId, UserId = userId };
            _context.FavoriteMissions.Add(favoriteMission);
            _context.SaveChanges();

            return Ok();
        }


        public async Task<IActionResult> GetCitiesByCountry(int countryId)
        {
            var cities = await _filters.GetCitiesByCountryAsync(countryId);
            return Json(cities);
        }

        public IActionResult MissionVolunteering(int id)
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                ViewBag.UserName = HttpContext.Session.GetString("UserName");
                ViewBag.IsLoggedIn = HttpContext.Session.GetString("IsLoggedIn");
                ViewBag.UserId = HttpContext.Session.GetString("UserId");
            }
            else
            {
                ViewBag.UserName = "Login";
            }
            Mission missionDetail = _context.Missions.Include(m=>m.MissionApplications).Include(m=>m.MissionRatings).Include(m => m.City).Include(m => m.Theme).Include(m=>m.FavoriteMissions).Include(m => m.GoalMissions).FirstOrDefault(m => m.MissionId == id);
            if(missionDetail == null)
            {
                return NotFound();
            }
            var missionAllDetail = new MissionVolunteeringModel
            {
                mission = missionDetail
            };
                       return View(missionAllDetail);
        }

        [HttpPost]
        public IActionResult UpdateRating(int missionId, int userId, int rating)
        {
            MissionRating missionRating = _context.MissionRatings.SingleOrDefault(mr => mr.MissionId == missionId && mr.UserId == userId);

            // if mission rating is not there by this user then add it
            if( missionRating == null)
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

        public IActionResult Apply(MissionApplication application)
        {
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
