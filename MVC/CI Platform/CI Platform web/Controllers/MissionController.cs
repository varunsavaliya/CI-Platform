using Ci_Platform.Repositories.Interfaces;
using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Reflection;
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

            var LandingView = new LandingPageModel();


            LandingView.Country = await _filters.GetCountriesAsync();
            LandingView.Theme = await _filters.GetThemesAsync();
            LandingView.Skill = await _filters.GetSkillsAsyc();
            return View(LandingView);
        }

        [HttpPost]
        public async Task<IActionResult> LandingPage(InputData inputData)
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
            List<Mission> missions = new List<Mission>();
            var vm = new LandingPageModel();


            // Extract the values from the inputData object
            string selectedCountry = inputData.selectedCountry;
            string selectedCities = inputData.selectedCities;
            string selectedThemes = inputData.selectedThemes;
            string selectedSkills = inputData.selectedSkills;
            string searchText = inputData.searchText;
            string selectedSortOption = inputData.selectedSortOption;
            string userId = inputData.userId;
            int pageSize = inputData.pageSize;
            int pageNo = inputData.pageNo;

            // use below code when you expect only one datatable

            //var response = _context.Missions.FromSql($"exec spGetMission @searchText={searchText}, @countryId={selectedCountry}, @cityId={selectedCities}, @themeId={selectedThemes}, @skillId={selectedSkills}, @sortCase = {selectedSortOption}, @userId = {userId}, @pageNo={pageNo}, @TotalRecords=@TotalRecords output", totalRecordsParam);

            //var items = await response.ToListAsync();

            //var MissionIds = items.Select(m => m.MissionId).ToList();

            IConfigurationRoot _configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Call the stored procedure
                SqlCommand command = new SqlCommand("spGetMission", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@countryId", SqlDbType.VarChar).Value = selectedCountry != null ? selectedCountry : null;
                command.Parameters.Add("@cityId", SqlDbType.VarChar).Value = selectedCities != null ? string.Join(",", selectedCities) : null;
                command.Parameters.Add("@themeId", SqlDbType.VarChar).Value = selectedThemes != null ? string.Join(",", selectedThemes) : null;
                command.Parameters.Add("@skillId", SqlDbType.VarChar).Value = selectedSkills != null ? string.Join(",", selectedSkills) : null;
                command.Parameters.Add("@searchText", SqlDbType.VarChar).Value = searchText;
                command.Parameters.Add("@sortCase", SqlDbType.VarChar).Value = selectedSortOption;
                command.Parameters.Add("@userId", SqlDbType.VarChar).Value = userId;
                command.Parameters.Add("@pageSize", SqlDbType.Int).Value = pageSize;
                command.Parameters.Add("@pageNo", SqlDbType.Int).Value = pageNo;
                SqlDataReader reader = command.ExecuteReader();

                // Read the results
                List<long> missionIds = new List<long>();
                while (reader.Read())
                {
                    long totalRecords = reader.GetInt32("TotalRecords");
                ViewBag.totalRecords = totalRecords;
                }
                reader.NextResult();

                while (reader.Read())
                {

                    // read only missionIds for comparing with mission table
                    long missionId = reader.GetInt64("mission_id");
                    missionIds.Add(missionId);
                }

                foreach (long missionId in missionIds)
                {
                    Mission mission = _context.Missions.Include(m => m.City).Include(m => m.Country).Include(m => m.Theme).Include(m => m.MissionSkills).ThenInclude(ms => ms.Skill).Include(m => m.GoalMissions).Include(m => m.FavoriteMissions).Include(m => m.MissionRatings).FirstOrDefault(m => m.MissionId == missionId);

                    if (mission != null)
                    {
                        missions.Add(mission);
                    }
                }
                vm.MissionList = missions;

                connection.Close();
            }
            vm.MissionList = missions;
            return PartialView("_GridListPartial", vm);
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

                //return BadRequest("Mission is already in favorites.");
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
            Mission missionDetail = _context.Missions.Include(m => m.MissionApplications).ThenInclude(ma => ma.User).Include(m => m.MissionRatings).Include(m => m.City).Include(m => m.Theme).Include(m => m.FavoriteMissions).Include(m => m.GoalMissions).Include(m=>m.Comments).ThenInclude(c=>c.User).Include(m=>m.MissionSkills).ThenInclude(ms=>ms.Skill).FirstOrDefault(m => m.MissionId == id);
            if (missionDetail == null)
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
        public IActionResult Apply(long missionId,long userId)
        {
            MissionApplication application = new MissionApplication();
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
