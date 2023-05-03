using System.ComponentModel.DataAnnotations;

namespace CI_Platform.Entities.ViewModels
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string email { get; set; } = String.Empty;
        public bool emailSent { get; set; }

        public string? token { get; set; } = String.Empty;
    }
}
