using CI_Platform.Entities.DataModels;
using System.ComponentModel.DataAnnotations;

namespace CI_Platform.Entities.ViewModels
{
    public class MissionVolunteeringModel
    {
        public MissionCard mission { get; set; } = new MissionCard();
        public List<MissionCard> RelatedMissions { get; set; } = new List<MissionCard>();
        public List<User> UserList { get; set; } = new List<User>();
        public string Link { get; set; }
        public int UserRatings { get; set; } = 0;
        [Required(ErrorMessage ="Enter your comment")]
        public string comment1 { get; set; }
        public List<MissionApplication> recentVolunteers { get; set; } = new List<MissionApplication>();
        public int totalVolunteers { get; set; }
        public List<string> MissionDocs { get; set; } = new List<string>();
    }
}
