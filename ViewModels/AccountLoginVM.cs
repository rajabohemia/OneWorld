using System.ComponentModel.DataAnnotations;

namespace OneWorld.ViewModels
{
    public class AccountLoginVM
    {
        [Required]
        [Display(Name = "Email Address")]
        [EmailAddress]
        public string EmailAddress { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}