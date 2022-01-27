using System.ComponentModel.DataAnnotations;

namespace OneWorld.ViewModels
{
    public class AccountResendEmailConfirmation
    {
        [Required]
        [Display(Name = "Email Address")]
        [EmailAddress]
        public string EmailAddress { get; set; }
    }
}