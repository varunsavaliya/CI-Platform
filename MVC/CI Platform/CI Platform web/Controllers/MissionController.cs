using CI_Platform.Entities.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CI_Platform_web.Controllers
{
    public class MissionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MissionController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult LandingPage()
        {
            if(HttpContext.Session.GetString("UserName") != null)
            {
                ViewBag.UserName = HttpContext.Session.GetString("UserName");
                ViewBag.IsLoggedIn = HttpContext.Session.GetString("IsLoggedIn");
            }
            else
            {
                ViewBag.UserName = "Evan Donohue";
            }
             var country = _context.Countries.ToList();
            ViewBag.countryList = country;

            var theme = _context.MissionThemes.ToList();
            ViewBag.themeList = theme;

            var skill = _context.Skills.ToList();
            ViewBag.skillList = skill;
            return View();
        }
        public IActionResult GetCitiesByCountry(int countryId)
        {
            var cities = _context.Cities.Where(c => c.CountryId == countryId).ToList();
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
