using CI_Platform.Entities.DataModels;
using System;
using System.Collections.Generic;
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
    }
}
