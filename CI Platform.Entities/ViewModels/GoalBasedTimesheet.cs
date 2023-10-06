using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class GoalBasedTimesheet
    {
        [Required(ErrorMessage = "Select Mission")]
        public long? goalMission { get; set; }
        [Required(ErrorMessage = "Choose Date")]
        public DateTime? goalDate { get; set; }
        [Required(ErrorMessage = "Enter Action")]
        public int? Actions { get; set; }
        [Required(ErrorMessage = "Enter Your message")]
        public String? goalMessage { get; set; }
        public long TimesheetId { get; set; }
    }
}
