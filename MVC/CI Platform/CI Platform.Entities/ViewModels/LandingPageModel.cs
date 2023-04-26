using CI_Platform.Entities.DataModels;

namespace CI_Platform.Entities.ViewModels
{
    public class LandingPageModel
    {
        public List<Country> Country { get; set; } = new List<Country>();

        public List<City> City { get; set; } = new List<City>();

        public List<MissionTheme> Theme { get; set; } = new List<MissionTheme>();

        public List<Skill> Skill { get; set; } = new List<Skill>();
        public List<MissionSkill> MissionSkills { get; set; } = new List<MissionSkill>();

        public List<MissionCard> MissionList { get; set; } = new List<MissionCard>();
        public int totalRecords { get; set; } = 0;
        public List<User> Users { get; set; } = new List<User>();
    }

    public class MissionCard
    {
        public Mission Missiondata { get; set; } = null!;

        public int? seatsleft { get; set; }

        public bool IsFavourite { get; set; }

        public bool IsOngoing { get; set; }

        public bool HasApplied { get; set; }

        public string? CityName { get; set; }

        public string? ThemeName { get; set; }

        public double? rating { get; set; } = null!;

        public bool IsDeadlinePassed { get; set; }

        public bool IsEnddatePassed { get; set; }

        public int? Goalvalue { get; set; }

        public int? AchievedGoal { get; set; }
        public string? MissionMedia { get; set; }
        public string? goalObjectiveText { get; set; }
        public int TotalUserRated { get; set; }
        public List<Comment> MissionComments { get; set; } = new List<Comment>();
        public List<string> MissionSkills { get; set;} = new List<string>();
        public List<MissionMedium> MissionAllMedia { get; set; } = new List<MissionMedium>();
    }
}

