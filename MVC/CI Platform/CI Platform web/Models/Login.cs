using System.ComponentModel.DataAnnotations;

namespace CI_Platform_web.Models
{
    public class Login
    {
        [Required]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage="Invalid Email")]
        public string email { get; set; }

        [Required]
        public string pwd { get; set; }
    }
}
