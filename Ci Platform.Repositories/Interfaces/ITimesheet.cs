using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ci_Platform.Repositories.Interfaces
{
    public interface ITimesheet
    {
        public List<Mission> GetMissionsById(long userId);
        public List<Timesheet> GetTimesheetsById(long userId);
        public Timesheet GetTimesheetDataById(long timesheetId);
        public Task DeleteTimesheetById(long timesheetId);

        public Task<string> AddTimeBasedData(VolunteeringTimesheetModel model, long userId);
        public Task<string> AddGoalBasedData(VolunteeringTimesheetModel model, long userId);
        public Task<Mission> GetMission(long missionId);

    }
}
