using CI_Platform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class AdminMissionThemeModel
    {
        public List<MissionTheme> MissionThemes { get; set; } = new List<MissionTheme>();
        public long MissionThemeId { get; set; }
        [Required(ErrorMessage = "Enter theme title")]
        public string? Title { get; set; }
        [Required(ErrorMessage = "Select theme status")]
        public byte Status { get; set; }
    }
}
