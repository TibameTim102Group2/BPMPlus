using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BPMPlus.Models;
using System.Diagnostics;

namespace BPMPlus.Controllers
{
	public class LoginController : Controller
	{
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        // GET: LoginController
        [HttpPost]
        public async Task<IActionResult> Login(string UserId, string Password, bool IsRememberMe)
        {

            if ((UserId == "123" && Password == "456") == false)
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
                    //IsPersistent = false：瀏覽器關閉立馬登出；IsPersistent = true 就變成常見的Remember Me功能
                    IsPersistent = false,
                });

            // 導至隱私頁面
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");// 導至登入頁
        }
        public ActionResult Index()
		{
			return View();
		}

        public ActionResult EmailValid()
        {
            return View();
        }

        public ActionResult EmailForgetPassWord()
        {
            return View();
        }
        public ActionResult ResetPassWord()
        {
            return View();
        }
    }
}
