﻿using Microsoft.AspNetCore.Authentication.Cookies;
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
using System.Web;
using static System.Net.Mime.MediaTypeNames;
using System.Data;
using BPMPlus.Attributes;
using Microsoft.AspNetCore.Hosting;
using BPMPlus.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BPMPlus.Controllers
{
    public class LoginController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly EmailService emailService;
        private readonly AesAndTimestampService aesAndTimestampService;
        private readonly ResetPasswordService resetPwService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public LoginController(ApplicationDbContext context, EmailService emailService, AesAndTimestampService aesAndTimestampService, ResetPasswordService resetPwService, IWebHostEnvironment WebHostEnvironment) : base(context)
        {
            _context = context;
            this.emailService = emailService;
            this.aesAndTimestampService = aesAndTimestampService;
            this.resetPwService = resetPwService;
            _webHostEnvironment = WebHostEnvironment;
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /* 登入 */
        [HttpPost]
        public async Task<IActionResult> Login(LoginInputVM login)
        {

            //先判斷user是否存在
            var user = await _context.User
                 .FirstOrDefaultAsync(m => EF.Functions.Collate(m.UserId, "Latin1_General_BIN") == login.UserId && m.UserIsActive == true);
            
            // user存在
            if (user != null)
            {
                //判斷密碼是否為空
                if (login.Password != null)
                {
                    //BCrypt 判斷密碼是否正確
                    bool isTruePassword = BCryptHelper.Verify(login.Password, user.Password);

                    if (isTruePassword == true)
                    {
                        // 登入成功，建立驗證 cookie
                        Claim[] claims = new[] { new Claim("UserId", login.UserId)};
                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                        string userName = user.UserName;
                        CookieOptions option = new CookieOptions();
                        Response.Cookies.Append("UserName", userName, option);

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
                //密碼為空
                else
                {
                    ViewBag.errMsg = "密碼不得為空!";
                    return View("Index", login); // 登入失敗導回頁面
                }
            }
            // user不存在
            else
            {
                ViewBag.errMsg = "帳號或密碼輸入錯誤";
                return View("Index", login); // 登入失敗導回頁面
            }

            // 抓第一筆符合的User資料
            var userWithGroups = await _context.User
                .Include(u => u.PermissionGroups)
                .ThenInclude(pg => pg.Users)
                .FirstOrDefaultAsync(u => u.UserId == login.UserId && u.UserId == user.UserId);

            // 抓Admin群組的所有userId
            var userActivityList = userWithGroups.PermissionGroups
                .Where(u => u.PermissionGroupId == "G0001")
                .SelectMany(u => u.Users)
                .Select(u => u.UserId)
                .ToList();

            // 判斷當前userId 是否包含在Admin群組內
            bool isAdminUser = userActivityList.Contains(login.UserId);

            // 套用相應layout, 使用session
            if (isAdminUser)
            {
                HttpContext.Session.SetString("IsAdmin", "true");
                return RedirectToAction("Index", "Home");
            }
            else
            {
                HttpContext.Session.SetString("IsAdmin", "false");
                return RedirectToAction("Index", "Home");
            }
        }

       //登入page
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
            if (Email != null)
            {
                var request = await _context.User.FirstOrDefaultAsync(m => m.Email == Email);
                if (request == null)
                {
                    ViewBag.errMsg = "Email輸入錯誤!";
                    return View("~/Views/Login/ForgetPassword.cshtml");
                }
                else
                {
                    var UserName = request.UserName;
                    emailService.SendEmail(Email, UserName);
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ViewBag.errMsg = "Email輸入錯誤!";
                return View("~/Views/Login/ForgetPassword.cshtml");
            }

        }

        //忘記密碼page
        public IActionResult ForgetPassword()
        {
            return View();
        }


        /* 忘記密碼重設 */
        [HttpPost]
        public async Task<IActionResult> ForgetPwResetPw(ForgetPasswordVM vm)
        {
            var _service = aesAndTimestampService;

            try
            {
                //dataStr url解碼
                var dataStrDecode = HttpUtility.UrlDecode(vm.dataStr);
                //換回符號+/
                dataStrDecode = dataStrDecode.Replace('-', '+').Replace('_', '/');
                //分割dataStr ";"
                string[] dataStrSplit = dataStrDecode.Split(new[] { "|" }, StringSplitOptions.None);
                string ivKey = null;
                string encryptStr = null;
                if (dataStrSplit.Length > 1)
                {
                    ivKey = dataStrSplit[0];
                    encryptStr = dataStrSplit[1];
                }
                //解密encryptStr
                var Key = _service.GetKey();
                var decryptStr = _service.Decrypt(encryptStr, Key, ivKey);
                //分割decryptStr "|"
                string[] decryptStrSplit = decryptStr.Split("|", StringSplitOptions.None);
                string stampTime = null;
                string Email = null;
                if (decryptStrSplit.Length > 1)
                {
                    stampTime = decryptStrSplit[0];
                    Email = decryptStrSplit[1];
                }

                //確認user身分
                var user = await _context.User.FirstOrDefaultAsync(m => m.Email == Email && m.UserIsActive == true);
                //取得現在時間戳
                DateTimeOffset dateTimeOffset = new DateTimeOffset(DateTime.Now);
                long nowStampTime = dateTimeOffset.ToUnixTimeSeconds();
                //Email驗證信queryString
                string queryString = "?data=" + vm.dataStr;

                //判斷現在時間是否超過驗證信有效時間
                if (nowStampTime <= long.Parse(stampTime) + 900)
                {
                    // 判斷user是否存在
                    if (user != null)
                    {
                        //判斷user是否在30分鐘內重設過密碼
                        if (nowStampTime <= user.ModifyPasswordTime + 1800)
                        {
                            //判斷密碼是否符合規則
                            var resetPasswordService = new ResetPasswordService();
                            var result = resetPasswordService.ValidatePassword(vm.ResetPassword);
                            if (result.IsValid == false)
                            {
                                ViewBag.errMsg = "密碼不符合規則";
                                return Redirect(Url.Action("ForgetPwResetPw", "Login") + queryString); // 修改失敗導回重設密碼頁
                            }
                            else
                            {
                                //判斷新密碼是否輸入正確
                                if (vm.ResetPassword != vm.ConfirmPassword)
                                {
                                    ViewBag.errMsg = "請確認密碼是否輸入一致";
                                    return Redirect(Url.Action("ForgetPwResetPw", "Login") + queryString); // 修改失敗導回重設密碼頁
                                }
                                else
                                {
                                    //重設密碼加密後存入DB
                                    string newPassword = BCryptHelper.HashPassword(vm.ResetPassword);
                                    user.Password = newPassword;
                                    await _context.SaveChangesAsync();
                                    //取得儲存密碼時間戳
                                    DateTimeOffset saveChangeTime = new DateTimeOffset(DateTime.Now);
                                    user.ModifyPasswordTime = saveChangeTime.ToUnixTimeSeconds();
                                    //儲存修改密碼時間戳
                                    await _context.SaveChangesAsync();

                                    TempData["SuccessMessage"] = "密碼變更成功!";
                                }
                            }
                        }
                        //30分鐘內已重設過密碼
                        else
                        {
                            ViewBag.ErrorMessage = "30分鐘內已重設過密碼, 請稍後再嘗試或聯繫系統管理員!";
                            return View("~/Views/Login/ErrorPage.cshtml");
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "無效的使用者! 請聯繫系統管理員!";
                        return View("~/Views/Login/ErrorPage.cshtml");
                    }
                }
                //重設信件連結逾期
                else
                {
                    ViewBag.ErrorMessage = "密碼重設信件逾期, 請重新寄送驗證信!";
                    return View("~/Views/Login/ErrorPage.cshtml");

                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "密碼重設錯誤,請重試或聯繫系統管理員!+"+ex;
                return View("~/Views/Login/ErrorPage.cshtml");
            }
            return View("ForgetPwResetPw", vm);
        }

        //忘記密碼重設page
        public IActionResult ForgetPwResetPw()
        {
            //取得驗證信url "data="後的內容
            string dataStr = HttpContext.Request.Query["data"];
            ViewBag.DataStr = dataStr;
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
        [AuthAttribute]
        [HttpPost]
        public async Task<IActionResult> ResetPassWord(ChangePasswordVM vm)
        {
            var user = await GetAuthorizedUser();

            //舊密碼不為空
            if (vm.OldPassword != null)
            {
                //判斷舊密碼是否輸入正確
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
                            //第二次輸入新密碼不為空
                            if (vm.ConfirmPassword != null)
                            {
                                //判斷新密碼是否輸入正確
                                if (vm.NewPassword != vm.ConfirmPassword)
                                {
                                    ViewBag.errMsg = "請確認新密碼是否輸入一致";
                                    return View("ResetPassWord", vm); // 修改失敗導回頁面
                                }
                            }
                        }
                    }
                }
                else
                {
                    ViewBag.errMsg = "舊密碼輸入錯誤";
                    return View("ResetPassWord", vm); // 修改失敗導回頁面
                }
                string newPassword = BCryptHelper.HashPassword(vm.NewPassword);

                user.Password = newPassword;
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "密碼變更成功!";
                //取得儲存密碼時間戳
                DateTimeOffset saveChangeTime = new DateTimeOffset(DateTime.Now);
                user.ModifyPasswordTime = saveChangeTime.ToUnixTimeSeconds();
                //儲存修改密碼時間戳
                await _context.SaveChangesAsync();
            }
            else
            {
                ViewBag.errMsg = "舊密碼不得為空";
            }
            return View("ResetPassWord", vm);
        }

        [Authorize]
        //修改密碼page
        public IActionResult ResetPassWord()
        {
            return View();
        }

        //重設密碼失敗跳轉
        public IActionResult ErrorPage()
        {
            return View();
        }

        //設定大頭貼

        [HttpGet]
        public IActionResult UploadProfilePicture()
        {
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadProfilePicture(IFormFile profilePicture)
        {
            User user = await GetAuthorizedUser();

           
            if (profilePicture == null || profilePicture.Length == 0)
            {
                return Json(new { success = false, message = "請選擇檔案上傳" });
            }

            //var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "profiles");

            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/profiles");

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            var fileName = Path.GetFileName(profilePicture.FileName);
            var newFileName = $"{user.UserId}{Path.GetExtension(fileName)}";
            var filePath = Path.Combine(uploadPath, newFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await profilePicture.CopyToAsync(fileStream);
            }

           

            return Json(new { success = true, message = "上傳成功", imagePath = $"/profiles/{newFileName}" });
        }

    }
}
