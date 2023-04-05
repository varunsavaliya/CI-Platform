using Ci_Platform.Repositories.Interfaces;
using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CI_Platform_web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserProfile _userProfile;
        private readonly IFilters _filters;

        public UserController(IUserProfile userProfile, IFilters filters)
        {
            _userProfile = userProfile;
            _filters = filters;
        }

        public async Task<IActionResult> UserProfile()
        {
            var UserId = "";
            if (HttpContext.Session.GetString("UserName") != null)
            {
                ViewBag.UserName = HttpContext.Session.GetString("UserName");
                ViewBag.IsLoggedIn = HttpContext.Session.GetString("IsLoggedIn");
                UserId = HttpContext.Session.GetString("UserId");
                ViewBag.UserId = UserId;
            }
            else
            {
                ViewBag.UserName = "Login";
            }

            UserProfileModel viewModel = await _userProfile.GetUser(Convert.ToInt64(UserId));
            viewModel.countryList = await _filters.GetCountriesAsync();
            viewModel.skillList = await _filters.GetSkillsAsyc();
            viewModel.userSkills = await _userProfile.GetUserSkills(Convert.ToInt64(UserId));
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UserProfile(UserProfileModel model)
        {
            var UserId = "";
            if (HttpContext.Session.GetString("UserId") != null)
            {
                //ViewBag.UserName = HttpContext.Session.GetString("UserName");
                ViewBag.IsLoggedIn = HttpContext.Session.GetString("IsLoggedIn");
                UserId = HttpContext.Session.GetString("UserId");
                ViewBag.UserId = UserId;
            }
            else
            {
                ViewBag.UserName = "Login";
            }
            if (ModelState.IsValid)
            {
                await _userProfile.UpdateUserDetails(Convert.ToInt64(UserId), model);

                return RedirectToAction("UserProfile", "User");
            }
            return BadRequest();
        }
    }
}
