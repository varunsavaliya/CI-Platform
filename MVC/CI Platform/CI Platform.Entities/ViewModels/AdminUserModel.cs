using CI_Platform.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class AdminUserModel
    {
        public List<User> users { get; set; } = new List<User>();
        public long UserId { get; set; }
        public UserProfileModel Profile { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Enter password")]
        public string Password { get; set; } = null!;
        public List<Country> countryList { get; set; } = new List<Country>();
        // avatar is pending
        public string? Avatar { get; set; }
        public string? MyProfile { get; set; }
        public long? CityId { get; set; }
        public long? CountryId { get; set; }
        [Required]
        [Range(0, 1, ErrorMessage = "The Status field must be either 0 or 1.")]
        public int? Status { get; set; }
       
    }
}
