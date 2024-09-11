using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BPMPlus.Models;
using System.Diagnostics;
using BPMPlus.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using BPMPlus.Service;
using BPMPlus.ViewModels.Login;
using BCryptHelper = BCrypt.Net.BCrypt;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace BPMPlus.Controllers
{
    public class LoginController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly EmailService emailService;
        private readonly AesEncryptionService aesService;
        private readonly ResetPasswordService resetPwService;

        public LoginController(ApplicationDbContext context, EmailService emailService, AesEncryptionService aesService, ResetPasswordService resetPwService) : base(context)
        {
            _context = context;
            this.emailService = emailService;
            this.aesService = aesService;
            this.resetPwService = resetPwService;
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /* 登入 */
        [HttpPost]
        public async Task<IActionResult> Login(LoginInput login)
        {
            //先判斷user是否存在
            var user = await _context.User
                 .FirstOrDefaultAsync(m => m.UserId == login.UserId && m.UserIsActive == true);
            
            // user存在
            if (user != null)
            {
                //BCrypt 判斷密碼是否正確
                bool isTruePassword = BCryptHelper.Verify(login.Password, user.Password);

                if (isTruePassword == true)
                {
                    // 登入成功，建立驗證 cookie
                    Claim[] claims = new[] { new Claim("UserId", login.UserId) };
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    // 呼叫 SignInAsync 以登入使用者
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, new AuthenticationProperties()
                    {
                        IsPersistent = false,   //瀏覽器關閉立馬登出
                    });
                }
                //密碼輸入錯誤
                else
                {
                    ViewBag.errMsg = "帳號或密碼輸入錯誤";
                    return View("Index", login); // 登入失敗導回頁面
                }
            }
            // user不存在
            else
            {
                ViewBag.errMsg = "帳號或密碼輸入錯誤";
                return View("Index", login); // 登入失敗導回頁面
            }
            // 成功登入導至隱私頁面
            return RedirectToAction("Index", "Home");
        }

       //登入
        public IActionResult Index()
		{
			return View();
		}

        /* 登出 */
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");       // 導至登入頁
        }

        /* 忘記密碼, 寄驗證信 */
        [HttpPost]
        public async Task<IActionResult> ForgetPasswordSendEmail(string Email)
        {
            var request = await _context.User.FirstOrDefaultAsync(m => m.Email == Email == true);
            if (request == null)
            {                
                //ViewBag.errMsg = "Email輸入錯誤!";
                return RedirectToAction("Index", "Home");
            }

            var UserName = request.UserName;
            emailService.SendEmail(Email, UserName);
            return RedirectToAction("Index", "Home");

        }
       
        //忘記密碼
        public IActionResult ForgetPassword()
        {
            return View();
        }


        //忘記密碼重設
        public IActionResult ForgetPwResetPw()
        {
            return View();
        }


        /* 測試修改密碼
        [HttpGet]
        public async Task<IActionResult> ResetPW(string password)
        {
            var resetPasswordService = new ResetPasswordService();
            string PW = "A@1234567";
            ValidationResult result = resetPwService.ValidatePassword(PW);

            if (result.IsValid)
            {
                Console.WriteLine("Password is valid.");
                return Ok("Password is valid.");
            }
            else
            {
                Console.WriteLine("Password is invalid. Errors:");
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"- {error}");
                    return Ok($"- {error}");
                }
            }
            return Ok("!");
        }
        */

        /* 修改密碼 */
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ResetPassWord(ChangePasswordVM vm)
        {
            var user = await GetAuthorizedUser();
            bool isTruePassword = BCryptHelper.Verify(vm.OldPassword, user.Password);

            if (isTruePassword == true)
            {
                //判斷新舊密碼是否重複
                if (vm.NewPassword == vm.OldPassword)
                {
                    ViewBag.errMsg = "新舊密碼不得重複";
                    return View("ResetPassWord", vm); // 修改失敗導回頁面
                }
                else
                {
                    //判斷密碼是否符合規則
                    var resetPasswordService = new ResetPasswordService();
                    var result = resetPasswordService.ValidatePassword(vm.NewPassword);
                    if (result.IsValid == false)
                    {
                        ViewBag.errMsg = "新密碼不符合密碼規則";
                        return View("ResetPassWord", vm); // 修改失敗導回頁面
                    }
                    else
                    {
                        //判斷新密碼是否輸入正確
                        if (vm.NewPassword != vm.ConfirmPassword)
                        {
                            ViewBag.errMsg = "請確認新密碼是否輸入一致";
                            return View("ResetPassWord", vm); // 修改失敗導回頁面
                        }
                    }
                }
                string newPassword = BCryptHelper.HashPassword(vm.NewPassword);

                user.Password = newPassword;
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "密碼變更成功!";

                //await Logout();
                return View("ResetPassWord", vm);
                //return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.errMsg = "舊密碼輸入錯誤";
                return View("ResetPassWord", vm); // 修改失敗導回頁面
            }
        }

        [Authorize]
        //修改密碼
        public IActionResult ResetPassWord()
        {
            return View();
        }
    }
}
