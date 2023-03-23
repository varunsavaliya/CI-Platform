using CI_Platform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class ShareStoryModel
    {
        [Required(ErrorMessage = "select a mission")]
        public int selectMission { get; set; }

        [Required(ErrorMessage ="Enter story title")]
        public string storyTitle { get; set; }

        [Required(ErrorMessage = "Please enter a date")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Please enter your story")]
        public string Story { get; set; }
        public List<Mission> missionListByUser { get; set; }
        //public List<Mission> MissionListByUser { get; set; }
    }
}
