using Ci_Platform.Repositories.Interfaces;
using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using CI_Platform_web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CI_Platform_web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdmin _admin;
        private readonly IFilters _filters;

        public AdminController(IAdmin admin, IFilters filters)
        {
            _admin = admin;
            _filters = filters;
        }

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

        public async Task<IActionResult> DeleteCMS(long id)
        {
            string message = await _admin.DeleteCMSById(id);
            return Json(message + Constants.deleteMessage);
        }
        public async Task<IActionResult> DeleteMission(long id)
        {
            string message = await _admin.DeleteMissionById(id);
            return Json(message + Constants.deleteMessage);
        }
        public async Task<IActionResult> DeleteMissionTheme(long id)
        {
            string message = await _admin.DeleteMissionThemeById(id);
            return Json(message + Constants.deleteMessage);
        }
        public async Task<IActionResult> DeleteMissionSkill(int id)
        {
            string message = await _admin.DeleteMissionSkillById(id);
            return Json(message + Constants.deleteMessage);
        }
        public async Task<IActionResult> DeleteStory(long id)
        {
            string message = await _admin.DeleteStoryById(id);
            return Json(message + Constants.deleteMessage);
        }
        public async Task<IActionResult> DeleteBanner(long id)
        {
            string message = await _admin.DeleteBannerById(id);
            return Json(message + Constants.deleteMessage);
        }
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
            if (id != 0)
            {
                model = _admin.GetCMSById(id);
            }
            return PartialView("_AddorEditCMS", model);
        }
        public IActionResult AdminMissionPage()
        {
            AdminMissionModel model = new()
            {
                Missions = _admin.GetMissionList(),
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AdminMissionPage(AdminMissionModel model)
        {
            if (model.MissionId != 0)
            {
                await _admin.UpdateMission(model, model.MissionId);
                return Ok(new { icon = "success", message = "Mission " + Constants.updateMessage });
            }
            else
            {
                await _admin.AddMission(model);
                return Ok(new { icon = "success", message = "Mission " + Constants.addMessage });
            }
        }

        public async Task<IActionResult> AddorEditMission(long id)
        {
            AdminMissionModel model = new();
            if (id != 0)
            {
                model = await _admin.GetMissionById(id);
            }
            model.CountryList = await _filters.GetCountriesAsync();
            model.ThemeList = await _filters.GetThemesAsync();
            model.SkillList = await _filters.GetSkillsAsyc();
            return PartialView("_AddorEditMission", model);
        }

        public async Task<IActionResult> AdminMissionTheme()
        {
            AdminMissionThemeModel model = new()
            {
                MissionThemes = await _admin.GetMissionThemeList(),
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AdminMissionTheme(AdminMissionThemeModel model)
        {
            if (model.MissionThemeId == 0)
            {
                if (_admin.IsUserExists(model.Title))
                {
                    TempData["Message"] = "Theme " + Constants.existsMessage;
                    TempData["Icon"] = "warning";
                }
                else
                {
                    await _admin.AddMissionTheme(model);
                    TempData["Message"] = "Theme " + Constants.addMessage;
                    TempData["Icon"] = "success";
                }
            }
            else
            {
                if (_admin.IsUserExists(model.Title, model.MissionThemeId))
                {
                    TempData["Message"] = "Theme " + Constants.existsMessage;
                    TempData["Icon"] = "warning";
                }
                else
                {
                    await _admin.UpdateMissionTheme(model);
                    TempData["Message"] = "Theme " + Constants.updateMessage;
                    TempData["Icon"] = "success";
                }
            }
            return RedirectToAction("AdminMissionTheme");
        }
        public async Task<IActionResult> AddorEditMissionTheme(long id)
        {
            AdminMissionThemeModel model = new();
            if (id != 0)
            {
                model = await _admin.GetMissionThemeById(id);
            }
            return PartialView("_AddorEditMissionTheme", model);
        }

        public async Task<IActionResult> AdminMissionSkill()
        {
            AdminMissionSkillModel model = new()
            {
                missionSkills = await _admin.GetMissionSkillList(),
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AdminMissionSkill(AdminMissionSkillModel model)
        {
            if (model.SkillId == 0)
            {
                if (_admin.IsUserExists(model.SkillName))
                {
                    TempData["Message"] = "Skill " + Constants.existsMessage;
                    TempData["Icon"] = "warning";
                }
                else
                {
                    await _admin.AddMissionSkill(model);
                    TempData["Message"] = "Skill " + Constants.addMessage;
                    TempData["Icon"] = "success";
                }
            }
            else
            {
                if (_admin.IsUserExists(model.SkillName, model.SkillId))
                {
                    TempData["Message"] = "Skill " + Constants.existsMessage;
                    TempData["Icon"] = "warning";
                }
                else
                {
                    await _admin.UpdateMissionSkill(model);
                    TempData["Message"] = "Skill " + Constants.updateMessage;
                    TempData["Icon"] = "success";
                }
            }
            return RedirectToAction("AdminMissionSkill");
        }

        public async Task<IActionResult> AddorEditMissionSkill(int id)
        {
            AdminMissionSkillModel model = new();
            if (id != 0)
            {
                model = await _admin.GetMissionSkillById(id);
            }
            return PartialView("_AddorEditMissionSkill", model);
        }

        public async Task<IActionResult> AdminMissionApplication()
        {
            AdminMissionApplicationModel model = new()
            {
                missionApplications = await _admin.GetMissionApplicationsList(),
            };
            return View(model);
        }

        public async Task<IActionResult> HandleMissionApplication(int button, long Id)
        {
            if (button == 1)
            {
                await _admin.ApproveMissionApplication(Id);
                return Ok(new { icon = "success", message = "Mission Application " + Constants.approveMessage });
            }
            else if (button == 0)
            {
                await _admin.DeclineMissionApplication(Id);
                return Ok(new { icon = "warning", message = "Mission Application " + Constants.declineMessage });
            }
            return RedirectToAction("AdminMissionApplication");
        }

        public async Task<IActionResult> AdminStory()
        {
            AdminStoryModel model = new()
            {
                stories = await _admin.GetStoriesList(),
            };
            return View(model);
        }

        public async Task<IActionResult> HandleStoryApproval(int button, long Id)
        {
            if (button == 1)
            {
                await _admin.ApproveStory(Id);
                return Ok(new { icon = "success", message = "Story " + Constants.approveMessage });
            }
            else if (button == 0)
            {
                await _admin.DeclineStory(Id);
                return Ok(new { icon = "warning", message = "Story " + Constants.declineMessage });
            }
            return RedirectToAction("AdminStory");
        }
        public async Task<IActionResult> BannerManagement()
        {
            BannerModel model = new()
            {
                Banners = await _admin.GetBannerList(),
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> BannerManagement(BannerModel model)
        {
            if (model.BannerId == 0)
            {
                await _admin.AddBanner(model);
                TempData["Message"] = "Banner " + Constants.addMessage;
                TempData["Icon"] = "success";
            }
            else
            {
                await _admin.UpdateBanner(model, model.BannerId);
                TempData["Message"] = "Banner " + Constants.updateMessage;
                TempData["Icon"] = "success";
            }

            return RedirectToAction("BannerManagement");
        }

        public async Task<IActionResult> AddorEditBanner(long id)
        {
            BannerModel model = new();
            if (id != 0)
            {
                model = await _admin.GetBannerById(id);
            }
            return PartialView("_AddorEditBanner", model);
        }

        public async Task<IActionResult> CommentManagement()
        {
            AdminCommentsModel model = new()
            {
                Comments = await _admin.GetCommentsList(),
            };
            return View(model);
        }

        public async Task<IActionResult> HandleCommentApproval(int button, long Id)
        {
            if (button == 1)
            {
                await _admin.ApproveComment(Id);
                return Ok(new { icon = "success", message = "Comment " + Constants.approveMessage });
            }
            else if (button == 0)
            {
                await _admin.DeclineComment(Id);
                return Ok(new { icon = "warning", message = "Comment " + Constants.declineMessage });
            }
            return RedirectToAction("CommentManagement");
        }

        public async Task<IActionResult> TimesheetManagement()
        {
            AdminTimesheetModel model = new()
            {
                timesheets = await _admin.GetTimesheetList(),
            };
            return View(model);
        }

        public async Task<IActionResult> HandleTimesheetApproval(int button, long Id)
        {
            if (button == 1)
            {
                await _admin.ApproveTimesheet(Id);
                return Ok(new { icon = "success", message = "Timesheet " + Constants.approveMessage });
            }
            else if (button == 0)
            {
                await _admin.DeclineTimesheet(Id);
                return Ok(new { icon = "warning", message = "Timesheet " + Constants.declineMessage });
            }
            return RedirectToAction("CommentManagement");
        }
    }
}
