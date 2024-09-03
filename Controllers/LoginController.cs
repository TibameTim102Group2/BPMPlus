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

namespace BPMPlus.Controllers
{
    public class LoginController : Controller
    {

        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> Login(string UserId, string Password)
        {
            var user = await _context.User
                 .FirstOrDefaultAsync(m => m.UserId == UserId && m.Password == Password && m.UserIsActive == true);
            if (user == null)
            {
                ViewBag.errMsg = "帳號或密碼輸入錯誤";
                return View("~/Views/Login/Index.cshtml"); // 登入失敗導回頁面
            }

            // 登入成功，建立驗證 cookie
            Claim[] claims = new[] { new Claim("UserId", UserId) };
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
        public ActionResult Index()
		{
			return View();
		}

        //登出
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");       // 導至登入頁
        }




        //忘記密碼
        public ActionResult EmailValid()
        {
            return View();
        }

        //驗證
        public ActionResult EmailForgetPassWord()
        {
            return View();
        }



        //修改密碼
        public ActionResult ResetPassWord()
        {
            return View();
        }
    }
}
