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
    public class AdminUserModel
    {
        public List<User> users { get; set; } = new List<User>();
        public long UserId { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Surname is required")]
        public string? Surname { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Enter password")]
        public string Password { get; set; } = null!;
        public List<Country> countryList { get; set; } = new List<Country>();
        public List<City> cityList { get; set; } = new List<City>();
        public string? MyProfile { get; set; }
        public long? CityId { get; set; }
        public long? CountryId { get; set; }
        [Required]
        [Range(0, 1, ErrorMessage = "The Status field must be either 0 or 1.")]
        public int? Status { get; set; }
        public string? employeeId { get; set; }
        public string? department { get; set; }
    }
}
