using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace BPMPlus.ViewModels.Login
{
    public class ForgetPasswordVM
    {
        public string dataStr { get; set; }


        public string ResetPassword { get; set; }


        public string ConfirmPassword { get; set; }
    }

    public class GetEmail
    {
        public string? Email { get; set; }
    }
}
