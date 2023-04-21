using CI_Platform.Entities.DataModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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
        public string? MissionUrls { get; set; }
        public List<Mission> Missions { get; set; } = new List<Mission>();
        public long MissionId { get; set; }

        public long ThemeId { get; set; }

        public long CityId { get; set; }

        public long CountryId { get; set; }
        [Required(ErrorMessage = "Enter mission title")]
        public string Title { get; set; } = null!;
        [Required(ErrorMessage = "Enter mission short description")]
        public string? ShortDescription { get; set; }
        [Required(ErrorMessage = "Select start date")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "Select start date")]
        public DateTime? StartDate { get; set; }
        [Required(ErrorMessage = "Select end date")]
        public DateTime? EndDate { get; set; }
        [Required(ErrorMessage = "Select mission type")]
        public string MissionType { get; set; } = null!;
        [Required(ErrorMessage = "Select status")]
        public int? Status { get; set; }
        [Required(ErrorMessage = "Enter organization name")]
        public string? OrganizationName { get; set; }
        [Required(ErrorMessage = "Enter organization detail")]
        public string? OrganizationDetail { get; set; }
        [Required(ErrorMessage = "select availibility")]
        public string? Availability { get; set; }

        public int? TotalSeats { get; set; }
        public string? GoalObjectiveText { get; set; }

        public int GoalValue { get; set; }

        public List<IFormFile> Files { get; set; } = null!;
        public List<string>? FileNames { get; set; } = null!;
        public IFormFile DefaultImage { get; set; } = null!;
        public string? DefaultImageName { get; set; } = null!;
        public List<Country> CountryList { get; set; } = null!;
        public List<City> CityList { get; set; } = null!;
        public List<MissionTheme> ThemeList { get; set; } = null!;
        public List<Skill> SkillList { get; set; } = null!;
        public List<int> MissionSkills { get; set; } = null!;
        
        public List<IFormFile> MissionDocs { get; set; } = null!;
        public List<string> MissionDocsNames { get; set; } = null!;
    }
}
