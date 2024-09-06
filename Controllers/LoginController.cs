using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BPMPlus.Models;
using System.Diagnostics;
using BPMPlus.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.AspNetCore.Identity;
using BPMPlus.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Runtime.Intrinsics.X86;

namespace BPMPlus.Controllers
{
    public class LoginController : BaseController
    {

        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            var user = await _context.User
                 .FirstOrDefaultAsync(m => m.UserId == vm.UserId && m.Password == vm.Password && m.UserIsActive == true);
            if (user == null)
            {
                ViewBag.errMsg = "帳號或密碼輸入錯誤";
                return View("Index", vm); // 登入失敗導回頁面
            }

            // 登入成功，建立驗證 cookie
            Claim[] claims = new[] { new Claim("UserId", vm.UserId) };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            // 呼叫 SignInAsync 以登入使用者
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                claimsPrincipal,
                new AuthenticationProperties()
                {
                    //IsPersistent = false：瀏覽器關閉立馬登出
                    IsPersistent = false,
                });

            // 導至隱私頁面
            return RedirectToAction("Index", "Home");
        }
       //登入
        public IActionResult Index()
		{
			return View();
		}

        [Authorize]
        //登出
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");       // 導至登入頁
        }


        [HttpPost]
        public async Task<IActionResult> EmailValid(LoginVM vm)
        {
            var request = await _context.User.SingleOrDefaultAsync(m => m.Email == vm.Email == true);

            if (request == null)
            {
                ViewBag.errMsg = "Email輸入錯誤!";
                return View("EmailValid", vm);
            }

            return View();
        }
        //忘記密碼
        public IActionResult EmailValid()
        {
            return View();
        }


        //輸入驗證
        public IActionResult EmailForgetPassWord()
        {
            return View();
        }



        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ResetPassWord(ChangePasswordVM vm)
        {
            var user = await GetAuthorizedUser();
            var request = await _context.User.SingleOrDefaultAsync(m => m.Password == vm.OldPassword == true);

            //輸入不得為空
            if (request == null)
            {
                ViewBag.errMsg = "舊密碼輸入錯誤";
                return View("ResetPassWord", vm); // 登入失敗導回頁面
            }
            //判斷新密碼是否輸入正確
            if (vm.NewPassword != vm.ConfirmPassword)
            {
                ViewBag.errMsg = "請確認新密碼是否輸入一致";
                return View("ResetPassWord", vm); // 登入失敗導回頁面
            }
            //判斷新舊密碼是否重複
            if (vm.NewPassword == vm.OldPassword)
            {
                ViewBag.errMsg = "新舊密碼不得重複";
                return View("ResetPassWord", vm); // 登入失敗導回頁面
            }

            user.Password = vm.NewPassword;
            await _context.SaveChangesAsync();

            ViewBag.Success = "修改成功!";

            //await Logout();
            //return RedirectToAction("Index", "Home");
            return View();
        }
        [Authorize]
        //修改密碼
        public IActionResult ResetPassWord()
        {
            return View();
        }
    }
}
