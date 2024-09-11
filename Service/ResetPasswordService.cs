﻿using System.ComponentModel.DataAnnotations;

namespace BPMPlus.Service
{
    public class ResetPasswordService
    {
        public ValidationResult ValidatePassword(string password)
        {
            var errors = new System.Collections.Generic.List<string>();

            //密碼最小長度
            const int requiredLength = 8;
            //至少一個大寫字母
            const int requiredUppercase = 1;
            //至少一個數字
            const int requiredDigit = 1;
            //至少一個特殊符號
            const int requiredSpecialChar = 1;

            // 检查密码长度
            if (password.Length < requiredLength)
            {
                errors.Add($"Password must be at least {requiredLength} characters long.");
            }

            // 检查是否包含大写字母
            if (password.Count(char.IsUpper) < requiredUppercase)
            {
                errors.Add("Password must contain at least one uppercase letter.");
            }

            // 检查是否包含数字
            if (password.Count(char.IsDigit) < requiredDigit)
            {
                errors.Add("Password must contain at least one digit.");
            }

            // 检查是否包含特殊字符
            if (!password.Any(ch => "!@#$_+.".Contains(ch)))
            {
                errors.Add("Password must contain at least one special character.");
            }

            // 返回验证结果
            return new ValidationResult
            {
                IsValid = !errors.Any(),
                Errors = errors
            };
        }
    }

    // 验证结果类
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public System.Collections.Generic.IList<string> Errors { get; set; }
    }
}

