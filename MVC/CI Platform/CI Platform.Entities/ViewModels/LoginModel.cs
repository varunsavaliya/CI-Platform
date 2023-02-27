using System.ComponentModel.DataAnnotations;

namespace CI_Platform.Entities.ViewModels
{
    public class LoginModel
    {
        [Required]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage="Invalid Email")]
        public string email { get; set; }

        [Required]
        public string password { get; set; }
    }
}
