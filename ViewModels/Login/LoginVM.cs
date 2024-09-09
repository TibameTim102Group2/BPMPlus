using Microsoft.AspNetCore.Mvc;

namespace BPMPlus.ViewModels.Login
{
    public class LoginVM
    {
        public string UserId { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
