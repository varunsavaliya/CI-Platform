using CI_Platform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class MissionVolunteeringModel
    {
        public Mission mission { get; set; }
        public List<Mission> RelatedMissions { get; set; }
        public List<User> UserList { get; set; }
        public string Link { get; set; }

        [Required(ErrorMessage ="Enter comment")]
        public string comment1 { get; set; }
        public List<MissionApplication> recentVolunteers { get; set; }
        public int totalVolunteers { get; set; }
        public List<string> MissionDocs { get; set; } = new List<string>();
    }
}
