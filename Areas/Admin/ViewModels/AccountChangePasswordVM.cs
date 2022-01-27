using System.ComponentModel.DataAnnotations;

namespace OneWorld.Areas.Admin.ViewModels
{
    public class AccountChangePasswordVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Display(Name = "Current Password")]
        [Required,DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
        [Display(Name = "New Password")]
        [Required,DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Display(Name = "Retype New Password")]
        [Required,DataType(DataType.Password)]
        [Compare(nameof(NewPassword))]
        public string RetypeNewPassword { get; set; }
    }
}