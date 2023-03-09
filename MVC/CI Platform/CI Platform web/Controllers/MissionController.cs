using Ci_Platform.Repositories.Interfaces;
using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
            if(HttpContext.Session.GetString("UserName") != null)
            {
                ViewBag.UserName = HttpContext.Session.GetString("UserName");
                ViewBag.IsLoggedIn = HttpContext.Session.GetString("IsLoggedIn");
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
            LandingView.MissionList = _context.Missions.Include(m => m.City).Include(m => m.Theme).Include(m => m.MissionRatings).Include(m => m.MissionSkills).ThenInclude(ms=>ms.Skill).Include(m => m.GoalMissions).ToList();


            //foreach (var mission in missions)
            //{
            //    double Rating = mission.MissionRatings.Rating;
            //    ViewBag.AverageRatings[mission.MissionId] = averageRating;
            //}

            return View(LandingView);
        }
        public async Task<IActionResult> GetCitiesByCountry(int countryId)
        {
            var cities = await _filters.GetCitiesByCountryAsync(countryId);
            return Json(cities);
        }

        public IActionResult MissionVolunteering()
        {
            return View();
        }
        public IActionResult StoriesListing()
        {
            return View();
        }
    }
}
