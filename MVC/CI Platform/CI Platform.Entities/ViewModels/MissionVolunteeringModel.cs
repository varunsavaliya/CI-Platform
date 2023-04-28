using CI_Platform.Entities.DataModels;
using System.ComponentModel.DataAnnotations;

namespace CI_Platform.Entities.ViewModels
{
    public class MissionVolunteeringModel
    {
        public MissionCard mission { get; set; } = new();
        public List<MissionCard> RelatedMissions { get; set; } = new();
        public List<User> UserList { get; set; } = new();
        public string Link { get; set; } = null!;
        public int UserRatings { get; set; } = 0;
        [Required(ErrorMessage = "Enter your comment")]
        public string comment1 { get; set; } = null!;
        public List<MissionApplication> recentVolunteers { get; set; } = new();
        public int totalVolunteers { get; set; }
        public List<string> MissionDocs { get; set; } = new();
    }
}
