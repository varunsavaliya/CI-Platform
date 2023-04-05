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
    public class UserProfileModel
    {
        public String? employeeId { get; set; }
        public String? Manager { get; set; }
        public String? title { get; set; }
        public String? whyIVolunteer { get; set; }
        public long? City { get; set; }
        public String? Availablity { get; set; }
        public String? LinkedIn { get; set; }
        public String? department { get; set; }
        [ValidateNever]
        public String? ProfileImageName { get; set; }
        [ValidateNever]
        public IFormFile ProfileImage { get; set;}
        public String? selectedSkills { get; set; }
        [Required(ErrorMessage ="Name is required")]
        public String Name { get; set; }
        [Required(ErrorMessage ="Surname is required")]
        public String Surname { get; set; }
        [Required(ErrorMessage = "Profile text is required")]
        public String MyProfile { get; set; }
        [Required(ErrorMessage = "Select your country")]
        public long Country { get; set; }
        [ValidateNever]
        public User userDetails { get; set; } = new User();
        [ValidateNever]
        public List<Country> countryList { get; set; } 
        [ValidateNever]
        public List<Skill> skillList { get; set; }
        [ValidateNever]
        public List<UserSkill> userSkills { get; set; }
    }
}
