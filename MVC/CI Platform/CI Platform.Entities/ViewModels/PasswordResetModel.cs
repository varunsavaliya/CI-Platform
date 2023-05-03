using System.ComponentModel.DataAnnotations;

namespace CI_Platform.Entities.ViewModels
{
    public class PasswordResetModel
    {
        [Required]
        public string password { get; set; } = String.Empty;

        [Required]
        [Compare("password", ErrorMessage = "Confirm password doesn't match, Type again !")]
        public string ConfirmPassword { get; set; } = String.Empty;
        public bool PasswordChanged { get; set; }
        public string Email { get; set; } = String.Empty;
        public string Token { get; set; } = String.Empty;
    }
}
