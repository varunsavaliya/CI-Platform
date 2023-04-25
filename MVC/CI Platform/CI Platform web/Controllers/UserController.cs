using Ci_Platform.Repositories.Interfaces;
using CI_Platform.Entities.ViewModels;
using Microsoft.AspNetCore.Mvc;

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
            if (HttpContext.Session.GetString("UserId") == null)
            {
                string? returnUrl = Url.Action("UserProfile", "User");
                return RedirectToAction("Index", "Home", new { returnUrl });
            }
            long userId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
            
            UserProfileModel viewModel = await _userProfile.GetUser(userId);
            viewModel.countryList = await _filters.GetCountriesAsync();
            viewModel.skillList = await _filters.GetSkillsAsyc();
            viewModel.userSkills = await _userProfile.GetUserSkills(userId);
            return View(viewModel);
        }

        public async Task<IActionResult> ValidateEmployeeId(string employeeId)
        {
            bool isInvalid = await _userProfile.ValidateEmpId(employeeId);
            if (isInvalid)
            {
                ModelState.AddModelError("employeeId", "Invalid employee ID");
                return Json(new { isValid = false, message = "Invalid employee ID" });
            }
            return Json(new { isValid = true });
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
