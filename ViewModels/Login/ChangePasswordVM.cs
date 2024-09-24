using System.ComponentModel.DataAnnotations;

namespace BPMPlus.ViewModels.Login
{
    public class ChangePasswordVM
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "舊密碼")]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "新密碼")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "確認新密碼")]
        public string ConfirmPassword { get; set; }
    }
}
