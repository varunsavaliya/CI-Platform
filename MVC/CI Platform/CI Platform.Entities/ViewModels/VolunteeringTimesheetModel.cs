using CI_Platform.Entities.DataModels;

namespace CI_Platform.Entities.ViewModels
{
    public class VolunteeringTimesheetModel
    {
        public TimeBasedTimesheet TimeBasedTimesheet { get; set; }
        public GoalBasedTimesheet GoalBasedTimesheet { get; set;}
        public List<Mission> missions { get; set; }
        public List<Timesheet> timesheets { get; set; }
    }
}
