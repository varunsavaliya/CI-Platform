using CI_Platform.Entities.DataModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class AdminMissionModel
    {
        public List<Mission> Missions { get; set; } = new List<Mission>();
        public long MissionId { get; set; }

        public long ThemeId { get; set; }

        public long CityId { get; set; }

        public long CountryId { get; set; }

        public string Title { get; set; } = null!;

        public string? ShortDescription { get; set; }

        public string? Description { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string MissionType { get; set; } = null!;

        public int? Status { get; set; }

        public string? OrganizationName { get; set; }

        public string? OrganizationDetail { get; set; }

        public string? Availability { get; set; }

        public int? TotalSeats { get; set; }
        public string? GoalObjectiveText { get; set; }

        public int GoalValue { get; set; }

        public List<IFormFile> Files { get; set; } = null!;
        public IFormFile DefaultImage { get; set; } = null!;
        public List<string> MissionUrls { get; set; } = null!;
        public List<Country> CountryList { get; set; } = null!;
        public List<City> CityList { get; set; } = null!;
        public List<MissionTheme> ThemeList { get; set; } = null!;
        public List<Skill> SkillList { get; set; } = null!;
        public List<long> MissionSkills { get; set; } = null!;
        
        
    }
}
