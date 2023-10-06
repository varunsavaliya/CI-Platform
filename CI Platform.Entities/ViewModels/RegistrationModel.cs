using System.ComponentModel.DataAnnotations;

namespace CI_Platform.Entities.ViewModels
{
    public class RegistrationModel
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty ;

        [Required(ErrorMessage = "You must provide a phone number")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Phone number must be 10 digits long")]
        public long PhoneNumber { get; set; }


        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;


        [Required]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "The password must be exactly 8 characters long.")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again !")]
        public string confirmPassword { get; set; }= string.Empty;
    }
}