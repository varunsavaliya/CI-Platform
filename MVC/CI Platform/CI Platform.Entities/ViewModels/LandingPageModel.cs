using CI_Platform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class LandingPageModel
    {
        public List<Country> Country { get; set; } = new List<Country>();

        public List<City> City { get; set; } = new List<City>();

        public List<MissionTheme> Theme { get; set; } = new List<MissionTheme>();

        public List<Skill> Skill { get; set; } = new List<Skill>();
        public List<MissionSkill> MissionSkills { get; set; } = new List<MissionSkill>();

        //public List<Mission> MissionList { get; set; } = new List<Mission>();
        public List<MissionCard> MissionList { get; set; } = new List<MissionCard>();
        public int totalRecords { get; set; }
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

        public double? rating { get; set; } = 0;

        public bool IsDeadlinePassed { get; set; }

        public bool IsEnddatePassed { get; set; }

        public int? Goalvalue { get; set; }

        public int? AchievedGoal { get; set; }

        public string? MissionMedia { get; set; }
        public string? DefaultMedia { get; set; }

        public string? goalObjectiveText { get; set; }

    }
}

