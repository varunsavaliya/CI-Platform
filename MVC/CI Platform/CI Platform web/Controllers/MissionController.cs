using Ci_Platform.Repositories.Interfaces;
using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using CI_Platform_web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace CI_Platform_web.Controllers
{
    public class MissionController : Controller
    {
        private readonly IFilters _filters;
        private readonly IMission _mission;
        private readonly ISendInvite<MissionVolunteeringModel> _sendInvite;

        public MissionController(ApplicationDbContext context, IFilters filters, IMission mission)
        {
            _filters = filters;
            _mission = mission;
        }
        [HttpGet]
        public async Task<IActionResult> LandingPage()
        {
            if (HttpContext.Session.GetString("UserId") == null)
            {
                string? returnUrl = Url.Action("LandingPage", "Mission");
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
            long UserId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));

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
        public async Task<IActionResult> AddToFavorites(long missionId)
        {
            long userId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
            await _mission.HandleFav(missionId, userId);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(Comment formData)
        {
            long userId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));

            await _mission.HandleComment(formData, userId);
            return Ok(new { icon = "success", message = "Comment " + Constants.addMessage });
        }
        [HttpGet]
        public async Task<IActionResult> MissionVolunteering(int id)
        {
            if (HttpContext.Session.GetString("UserId") == null)
            {
                string? returnUrl = Url.Action("MissionVolunteering", "Mission");
                return RedirectToAction("Index", "Home", new { returnUrl });
            }
            long userId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));

            MissionVolunteeringModel model = new()
            {
                mission = _mission.GetMissionVolunteeringData(id, userId),
                RelatedMissions = _mission.GetRelatedMission(id, userId),
                totalVolunteers = await _mission.GetTotalVolunteers(id),
                MissionDocs = await _mission.GetMissionDocs(id),
                UserRatings = await _mission.GetUserMissionRating(id, userId),
                UserList = await _mission.GetUsersList(userId),
            };
            return View(model);
        }

        public IActionResult DisplayDoc(string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/MissionDocuments", fileName);
            var fileStream = new FileStream(filePath, FileMode.Open);
            var contentType = "application/pdf";
            return File(fileStream, contentType);
        }

        [HttpGet]
        public async Task<IActionResult> GetRecentVolunteers(long missionId, int pageNo, int pageSize)
        {
            MissionVolunteeringModel model = new()
            {
                recentVolunteers = await _mission.GetRecentVolByPage(missionId, pageNo, pageSize),
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
        public async Task<IActionResult> UpdateRating(long missionId, int rating)
        {
            long userId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
            await _mission.HandleRatings(missionId, userId, rating);
            return Ok();
        }

        [HttpPost]
        public IActionResult Apply(long missionId)
        {
            long userId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
            _mission.HandleMissionApply(missionId, userId);
            return Ok(new { icon = "success", message = "You have " + Constants.applyMessage + " in this mission"});
        }
    }
}
