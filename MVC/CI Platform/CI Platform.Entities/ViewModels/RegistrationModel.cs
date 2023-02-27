using System.ComponentModel.DataAnnotations;

namespace CI_Platform.Entities.ViewModels
{
    public class RegistrationModel
    {
        [Required]
        public string first_name { get; set; }

        [Required(ErrorMessage = "You must provide a phone number")]
        //[DisplayName("Home Phone")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string phone_number { get; set; }


        [Required]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Invalid Email")]
        public string email { get; set; }

        [Required]
        public string password { get; set; }

        [Required]
        [Compare("password", ErrorMessage = "Confirm password doesn't match, Type again !")]
        public string confirmPassword { get; set; }
    }
}