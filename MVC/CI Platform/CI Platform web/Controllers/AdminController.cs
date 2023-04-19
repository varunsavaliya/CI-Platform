using Ci_Platform.Repositories.Interfaces;
using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using CI_Platform_web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CI_Platform_web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdmin _admin;
        private readonly IFilters _filters;

        public AdminController(IAdmin admin, IFilters filters)
        {
            _admin = admin;
            _filters = filters;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AdminUser()
        {
            AdminUserModel model = new();
            model.users = _admin.GetUsers();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AdminUser(AdminUserModel model)
        {
            if (model.UserId == 0)
            {
                if (_admin.IsUserExists(model.Email))
                {
                    TempData["Message"] = "User " + Constants.existsMessage;
                    TempData["Icon"] = "warning";
                }
                else
                {
                    await _admin.AddUser(model);
                    TempData["Message"] = "User " + Constants.addMessage;
                    TempData["Icon"] = "success";
                }
            }
            else
            {
                if (_admin.IsUserExists(model.Email, model.UserId))
                {
                    TempData["Message"] = "User " + Constants.existsMessage;
                    TempData["Icon"] = "warning";
                }
                else
                {
                    await _admin.UpdateUser(model);
                    TempData["Message"] = "User " + Constants.updateMessage;
                    TempData["Icon"] = "success";
                }
            }
            return RedirectToAction("AdminUser");
        }
        public async Task<IActionResult> AddorEditUser(long id)
        {
            AdminUserModel model = new();
            if (id != 0)
            {
                AdminUserModel result = _admin.GetUserById(id);
                model = result;
            }
            model.countryList = await _filters.GetCountriesAsync(); ;
            return PartialView("_AddorEditUser", model);
        }
        public async Task<IActionResult> DeleteUser(long id)
        {
            string message = await _admin.DeleteUserById(id);
            return Json(message + Constants.deleteMessage);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult AdminCMS()
        {
            AdminCMSModel model = new()
            {
                cmsTables = _admin.GetCMSList(),
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AdminCMS(AdminCMSModel model)
        {
            if (model.CmsPageId == 0)
            {
                if (_admin.IsCMSExists(model.Slug))
                {
                    TempData["Message"] = "CMS Page " + Constants.existsMessage;
                    TempData["Icon"] = "warning";
                }
                else
                {
                    await _admin.AddCMS(model);
                    TempData["Message"] = "CMS Page " + Constants.addMessage;
                    TempData["Icon"] = "success";
                }
            }
            else
            {
                if (_admin.IsCMSExists(model.Slug, model.CmsPageId))
                {
                    TempData["Message"] = "CMS Page " + Constants.existsMessage;
                    TempData["Icon"] = "warning";
                }
                else
                {
                    await _admin.UpdateCMS(model);
                    TempData["Message"] = "CMS Page " + Constants.updateMessage;
                    TempData["Icon"] = "success";
                }
            }
            return RedirectToAction("AdminCMS");
        }
        public IActionResult AddorEditCMS(long id)
        {
            AdminCMSModel model = new();
            if(id != 0)
            {
                model = _admin.GetCMSById(id);
            }
            return PartialView("_AddorEditCMS", model);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminMission()
        {
            AdminMissionModel model = new()
            {
                Missions = _admin.GetMissionList(),
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult AdminMission(AdminMissionModel model)
        {
            
            return RedirectToAction("AdminMission");
        }

        public async Task<IActionResult> AddorEditMission(long id)
        {
            AdminMissionModel model = new()
            {
                CountryList = await _filters.GetCountriesAsync(),
                ThemeList = await _filters.GetThemesAsync(),
                SkillList = await _filters.GetSkillsAsyc(),
            };
            if (id != 0)
            {

            }
            return PartialView("_AddorEditMission", model);
        }
    }
}
