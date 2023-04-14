using Ci_Platform.Repositories.Interfaces;
using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ci_Platform.Repositories.Repositories
{
    public class TimesheetRepository : ITimesheet
    {
        private readonly ApplicationDbContext _context;

        public TimesheetRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Mission> GetMissionsById(long userId)
        {
            return _context.Missions.Where(mission => mission.MissionApplications.Any(ma => ma.ApprovalStatus == "PUBLISHED" && ma.UserId == userId)).Include(mission => mission.MissionApplications).ToList();
        }

        public List<Timesheet> GetTimesheetsById(long userId)
        {
            return _context.Timesheets.Where(timesheet => timesheet.UserId == userId && timesheet.Status == "APPROVED").Include(timesheet => timesheet.Mission).ToList();
        }
        public async Task<Mission> GetMission(long missionId)
        {
            var mission = _context.Missions.Where(mission => mission.MissionId == missionId).FirstOrDefault();
            return mission;
        }
        public Timesheet GetTimesheetDataById(long timesheetId)
        {
            var timesheet = _context.Timesheets.Where(timesheet => timesheet.TimesheetId == timesheetId).Include(timesheet => timesheet.Mission).FirstOrDefault();
            return timesheet;
        }

        public async Task<String> AddTimeBasedData(VolunteeringTimesheetModel model, long userId)
        {
            // Combine Hours and Minutes into a TimeSpan
            TimeSpan? timeSpent = null;
            if (model.TimeBasedTimesheet.Hours.HasValue && model.TimeBasedTimesheet.Minutes.HasValue)
            {
                timeSpent = new TimeSpan(model.TimeBasedTimesheet.Hours.Value, model.TimeBasedTimesheet.Minutes.Value, 0);
            }
            if (model.TimeBasedTimesheet.TimesheetId == 0 || model.TimeBasedTimesheet.TimesheetId == null)
            {
                var timesheet = _context.Timesheets.Where(timesheet => timesheet.UserId == userId && timesheet.MissionId == model.TimeBasedTimesheet.timeMission).FirstOrDefault();
                if (timesheet != null && timesheet.DateVolunteered == model.TimeBasedTimesheet.timeDate)
                {
                    return "exists";
                }
                Timesheet data = new();
                data.UserId = userId;
                data.MissionId = model.TimeBasedTimesheet.timeMission;
                data.Time = timeSpent;
                data.DateVolunteered = model.TimeBasedTimesheet.timeDate;
                data.Notes = model.TimeBasedTimesheet.timeMessage;

                await _context.Timesheets.AddAsync(data);
                await _context.SaveChangesAsync();
            }
            else
            {
               var timesheet = _context.Timesheets.Where(timesheet => timesheet.TimesheetId == model.TimeBasedTimesheet.TimesheetId).FirstOrDefault();
                var timesheets = _context.Timesheets.Any(timesheet => timesheet.TimesheetId != model.TimeBasedTimesheet.TimesheetId && timesheet.UserId == userId && timesheet.MissionId == model.TimeBasedTimesheet.timeMission && timesheet.DateVolunteered == model.TimeBasedTimesheet.timeDate);
                if (timesheets)
                {
                    return "exists";
                }
                timesheet.Time = timeSpent;
                timesheet.DateVolunteered = model.TimeBasedTimesheet.timeDate;
                timesheet.Notes = model.TimeBasedTimesheet.timeMessage;
                await _context.SaveChangesAsync();
                return "updated";
            }
            return "success";
        }
        public async Task<string> AddGoalBasedData(VolunteeringTimesheetModel model, long userId)
        {
            if (model.GoalBasedTimesheet.TimesheetId == 0 || model.GoalBasedTimesheet.TimesheetId == null)
            {
                var timesheet = _context.Timesheets.Where(timesheet => timesheet.UserId == userId && timesheet.MissionId == model.GoalBasedTimesheet.goalMission).FirstOrDefault();

                if (timesheet != null && timesheet.DateVolunteered == model.GoalBasedTimesheet.goalDate)
                {
                    return "exists";
                }
                Timesheet data = new();
                data.UserId = userId;
                data.MissionId = model.GoalBasedTimesheet.goalMission;
                data.Action = model.GoalBasedTimesheet.Actions;
                data.DateVolunteered = model.GoalBasedTimesheet.goalDate;
                data.Notes = model.GoalBasedTimesheet.goalMessage;

                await _context.Timesheets.AddAsync(data);
                await _context.SaveChangesAsync();
            }
            else
            {
                var timesheet = _context.Timesheets.Where(timesheet => timesheet.TimesheetId == model.GoalBasedTimesheet.TimesheetId).FirstOrDefault();
                var timesheets = _context.Timesheets.Any(timesheet => timesheet.TimesheetId != model.GoalBasedTimesheet.TimesheetId && timesheet.UserId == userId && timesheet.MissionId == model.GoalBasedTimesheet.goalMission && timesheet.DateVolunteered == model.GoalBasedTimesheet.goalDate);
                if (timesheets)
                {
                    return "exists";
                }
                timesheet.Action = model.GoalBasedTimesheet.Actions;
                timesheet.DateVolunteered = model.GoalBasedTimesheet.goalDate;
                timesheet.Notes = model.GoalBasedTimesheet.goalMessage;
                await _context.SaveChangesAsync();
                return "updated";
            }
            return "success";
        }

        public async Task DeleteTimesheetById(long timesheetId)
        {
            var timesheet = _context.Timesheets.Where(timesheet => timesheet.TimesheetId == timesheetId).FirstOrDefault();
            _context.Timesheets.Remove(timesheet);
            await _context.SaveChangesAsync();
        }

    }
}
