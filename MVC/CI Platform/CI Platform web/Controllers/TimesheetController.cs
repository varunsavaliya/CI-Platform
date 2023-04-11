using Ci_Platform.Repositories.Interfaces;
using CI_Platform.Entities.ViewModels;
using CI_Platform.Entities.DataModels;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CI_Platform_web.Controllers
{
    public class TimesheetController : Controller
    {
        private readonly ITimesheet _timesheet;

        public TimesheetController(ITimesheet timesheet)
        {
            _timesheet = timesheet;
        }

        public IActionResult TimeSheet()
        {
            VolunteeringTimesheetModel model = new();
            long UserId = 0;
            if (HttpContext.Session.GetString("UserId") != null)
            {
                UserId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
            }
            model.missions = _timesheet.GetMissionsById(UserId);
            model.timesheets = _timesheet.GetTimesheetsById(UserId);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Timesheet(VolunteeringTimesheetModel model)
        {
            long UserId = 0;
            if (HttpContext.Session.GetString("UserId") != null)
            {
                UserId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
            }
            if (model.TimeBasedTimesheet != null)
            {
                string result = await _timesheet.AddTimeBasedData(model, UserId);
                if (result == "exists")
                {
                    TempData["Message"] = "Timesheet for same date and mission already exists";
                    TempData["Icon"] = "error";
                    return RedirectToAction("Timesheet");
                }else if(result == "success")
                {
                    TempData["Message"] = "Timesheet has been added and it will be visible as soon as the admin approves!!";
                    TempData["Icon"] = "success";
                    return RedirectToAction("Timesheet");
                }else if(result == "updated")
                {
                    TempData["Message"] = "Timesheet has been updated successfully!!";
                    TempData["Icon"] = "success";
                    return RedirectToAction("Timesheet");
                }
            }
            else
            {
                string result = await _timesheet.AddGoalBasedData(model, UserId);
                if (result == "exists")
                {
                    TempData["Message"] = "Timesheet for same date and mission already exists";
                    TempData["Icon"] = "error";
                    return RedirectToAction("Timesheet");
                }
                else if (result == "success")
                {
                    TempData["Message"] = "Timesheet has been added and it will be visible as soon as the admin approves!!";
                    TempData["Icon"] = "success";
                    return RedirectToAction("Timesheet");
                }
                else if (result == "updated")
                {
                    TempData["Message"] = "Timesheet has been updated successfully!!";
                    TempData["Icon"] = "success";
                    return RedirectToAction("Timesheet");
                }
            }
            return RedirectToAction("Timesheet");
        }

        public async Task<ActionResult> DeleteTimesheet(long timesheetId)
        {
           await _timesheet.DeleteTimesheetById(timesheetId);
            return Ok();
        }

        public async Task<IActionResult> GetMissionData(long missionId)
        {
            long UserId = 0;
            if (HttpContext.Session.GetString("UserId") != null)
            {
                UserId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
            }
            var mission = await _timesheet.GetMission(missionId);
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };
            return Json(mission, options);
        }

        public IActionResult GetTimesheetData(long timesheetId)
        {
            var timesheet = _timesheet.GetTimesheetDataById(timesheetId);
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };
            return Json(timesheet, options);
        }
    }
}
