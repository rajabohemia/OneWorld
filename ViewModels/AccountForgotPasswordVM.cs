using System.ComponentModel.DataAnnotations;

namespace OneWorld.ViewModels
{
    public class AccountForgotPasswordVM
    {
        [EmailAddress]
        [Required]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }
    }
}