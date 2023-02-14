using System.ComponentModel.DataAnnotations;

namespace CI_Platform_web.Models
{
    public class Registration
    {
        [Required]
        public string firstName { get; set; }

        [Required(ErrorMessage = "You must provide a phone number")]
        [Display(Name = "Home Phone")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string PhoneNumber { get; set; }


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
