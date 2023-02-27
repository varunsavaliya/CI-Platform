using System.ComponentModel.DataAnnotations;

namespace CI_Platform.Entities.ViewModels
{
    public class ForgotPasswordModel
    {
        [Required]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Invalid Email")]
        public string email { get; set; }
        public bool emailSent { get; set; }
    }
}
