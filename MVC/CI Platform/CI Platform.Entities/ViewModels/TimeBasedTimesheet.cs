using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class TimeBasedTimesheet
    {
        [Required(ErrorMessage = "Select Mission")]
        public long? timeMission { get; set; }
        [Required(ErrorMessage ="Choose Date")]
        public DateTime? timeDate { get; set; }
        [Required(ErrorMessage ="Enter Volunteered Hours")]
        [Range(0, 23, ErrorMessage = "Hours must be between 0 and 23.")]
        public int? Hours { get; set; }
        [Required(ErrorMessage = "Enter Volunteered Minutes")]
        [Range(0, 59, ErrorMessage = "Minutes must be between 0 and 59.")]
        public int? Minutes { get; set; }
        [Required(ErrorMessage = "Enter Your message")]
        public String? timeMessage { get; set; }
        public long TimesheetId { get; set; }
    }
}
