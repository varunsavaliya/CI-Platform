using System.ComponentModel.DataAnnotations;

namespace CI_Platform_web.Models
{
    public class ForgotPassword
    {
        [Required]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Invalid Email")]
        public string email { get; set; }
    }
}
