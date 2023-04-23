using CI_Platform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class AdminMissionSkillModel
    {
        public List<Skill> missionSkills = new List<Skill>();
        public int SkillId { get; set; }
        [Required(ErrorMessage = "Enter skill name")]
        public string? SkillName { get; set; }
        [Required(ErrorMessage = "Select skill status")]
        public byte Status { get; set; }
    }
}
