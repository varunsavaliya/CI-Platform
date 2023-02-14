using System.ComponentModel.DataAnnotations;

namespace CI_Platform_web.Models
{
    public class ResetPassword
    {
        [Required]
        public string password { get; set; }

        [Required]
        [Compare("password", ErrorMessage = "Confirm password doesn't match, Type again !")]
        public string ConfirmPassword { get; set; }
    }
}
