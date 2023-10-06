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
        [MaxLength(16, ErrorMessage = "Employee Id should be less than 16 characters")]
        public string? employeeId { get; set; }
        public string? Manager { get; set; }
        [MaxLength(255, ErrorMessage = "Title should be less than 255 characters")]
        public string? title { get; set; }
        public string? whyIVolunteer { get; set; }
        public long? City { get; set; }
        public string? Availablity { get; set; }
        [Url(ErrorMessage = "Invalid website URL")]
        [RegularExpression(@"^https:\/\/www\.linkedin\.com\/in\/[a-zA-Z0-9\-]+\/?$",
ErrorMessage = "Invalid LinkedIn URL")]
        public string? LinkedIn { get; set; }
        [MaxLength(16, ErrorMessage = "Department should be less than 16 characters")]
        public string? department { get; set; }
        [ValidateNever]
        public string? ProfileImageName { get; set; }
        [ValidateNever]
        public IFormFile ProfileImage { get; set; } = null!;
        public string? selectedSkills { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(16, ErrorMessage = "Name should be between 1 and 16")]
        [MinLength(1, ErrorMessage = "Name should be between 1 and 16")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Surname is required")]
        [MaxLength(16, ErrorMessage = "Name should be between 1 and 16")]
        [MinLength(1, ErrorMessage = "Name should be between 1 and 16")]
        public string? Surname { get; set; }
        [Required(ErrorMessage = "Profile text is required")]
        public string MyProfile { get; set; } = null;
        [Required(ErrorMessage = "Select your country")]
        public long Country { get; set; }
        [ValidateNever]
        public User userDetails { get; set; } = new User();
        [ValidateNever]
        public List<Country>? countryList { get; set; }
        public List<City>? CityList { get; set; }
        [ValidateNever]
        public List<Skill>? skillList { get; set; }
        [ValidateNever]
        public List<UserSkill>? userSkills { get; set; }
    }
}
