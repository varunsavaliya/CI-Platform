﻿using System.ComponentModel.DataAnnotations;

namespace CI_Platform.Entities.ViewModels
{
    public class PasswordResetModel
    {
        [Required]
        public string password { get; set; }

        [Required]
        [Compare("password", ErrorMessage = "Confirm password doesn't match, Type again !")]
        public string ConfirmPassword { get; set; }
        public bool PasswordChanged { get; set; }

        public string Email { get; set; }
        public string Token { get; set; }
    }
}
