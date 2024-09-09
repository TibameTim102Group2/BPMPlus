﻿using System.ComponentModel.DataAnnotations;

namespace BPMPlus.ViewModels.Login
{
    public class ForgetPasswordVM
    {
        public string Email { get; set; }
        public string UserName { get; set; }

        [Display(Name = "重設密碼")]
        [DataType(DataType.Password)]
        public string ResetPassword { get; set; }

        [Display(Name = "確認密碼")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
