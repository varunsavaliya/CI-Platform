using System.ComponentModel.DataAnnotations;

namespace CI_Platform.Entities.ViewModels
{
    public class LoginModel
    {
        [Required]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Please enter a valid email address.")]
        public string email { get; set; } = String.Empty;

        [Required]
        public string password { get; set; } = String.Empty;
    }
}
