using CI_Platform.Entities.DataModels;

namespace CI_Platform.Entities.ViewModels
{
    public class VolunteeringTimesheetModel
    {
        public TimeBasedTimesheet TimeBasedTimesheet { get; set; } = new();
        public GoalBasedTimesheet GoalBasedTimesheet { get; set; } = new();
        public List<Mission> missions { get; set; } = new();
        public List<Timesheet> timesheets { get; set; } = new();
    }
}
