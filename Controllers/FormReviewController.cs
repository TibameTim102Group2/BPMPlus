using BPMPlus.Attributes;
using BPMPlus.Data;
using BPMPlus.Models;
using BPMPlus.Service;
using BPMPlus.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.IO.Compression;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BPMPlus.Controllers
{
    public class FormReviewController : BaseController
    {
        ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly EmailService emailService;

        public FormReviewController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, EmailService emailService) : base(context)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            this.emailService = emailService;
        }

		[Authorize]
		[HttpGet]
        public async Task<JsonResult> CheckEmp(string id)
        {
            // 查詢該工單的流程節點表的功能編號是否跟最新工單紀錄的功能編號相同
            // 且該工單的責成人員是否跟工單紀錄最新的責成人員相同
            // 是的話抓取其UserId
            User user = await GetAuthorizedUser();

            var latestDetails = await _context.FormRecord
                                               .AsNoTracking()
                                               .Where(c => c.FormId == id && user.UserId == c.UserId)
                                               .OrderByDescending(t => t.ProcessingRecordId)
                                               .Select(c => new
                                               {
                                                   c.ResultId,
                                                   c.UserActivityId,
                                                   c.UserId,
                                                   c.FormId,
                                               })
                                               .FirstOrDefaultAsync();

            if (latestDetails == null)
            {
                return Json(new { status = false });
            }

            // 撈出這張工單08的userId
            var pnUser = await _context.ProcessNodes.Where(pn => pn.FormId == id && pn.UserActivityId == "08").Select(pn => pn.UserId).FirstOrDefaultAsync();

            // 撈出其userId, manday
            var Handler = await _context.User.Where(u => u.UserId == pnUser).Select(u => u.UserName).FirstOrDefaultAsync();

            // 撈出manday
            var now = await _context.Form
               .Where(fr => fr.FormId == id)
               .Select(fr => new
               {
                   fr.ManDay,
                   fr.ProcessNodeId 
               }).FirstOrDefaultAsync();

            //抓最新的工單紀錄跳過一筆
            //var skipOneFormRecord = await _context.FormRecord
            //    .Where(fr => fr.FormId == id)
            //    .OrderByDescending(d => d.Date)
            //    .Skip(1)
            //    .Select(fr => new
            //    {
            //        fr.UserId,
            //        fr.UserActivityId,
            //        fr.DepartmentId,
            //        fr.ResultId
            //    }).FirstOrDefaultAsync();

            var processNode = await _context.ProcessNodes.Where(pn => pn.FormId == id).Select(c => new
            {
                c.ProcessNodeId,
                c.UserId,
                c.UserActivityId,
            })
           .ToListAsync();

            // User 有 08 功能且 現在流程節點上的 08 userId是現在登入的userId 且最新的工單紀錄上要是08 且不等於 其他
            if ((user.PermittedTo("02") || user.PermittedTo("03") || user.PermittedTo("04") || user.PermittedTo("05") || user.PermittedTo("06")) && int.Parse(latestDetails.UserActivityId) <= 6)
            {
                return Json(new { status = true, userPermit = "other" });
            }
            if (user.PermittedTo("07") && latestDetails.UserActivityId == "07")
            {
                return Json(new { status = true, userPermit = "07", handler = Handler, time = now.ManDay});
            }
            else if (user.PermittedTo("08") && latestDetails.UserActivityId == "08")
            {
                return Json(new { status = true, userPermit = "08", handler = Handler, time = now.ManDay });
            }
            else if (user.PermittedTo("09") && latestDetails.UserActivityId == "09")
            {
                return Json(new { status = true, userPermit = "09", handler = Handler, time = now.ManDay });
            }
            return Json(new { status = false });
        }

		// 指派方輸入人員時會檢查是否與指派方同部門以及有權限處理者
		[Authorize]
		[HttpPost]
        public async Task<JsonResult> AssignEmp(string empName)
        {
            User user = await GetAuthorizedUser();
            var assignEmp = await _context.User.Where(u => u.UserName == empName).Select(e => new
            {
                userName = e.UserName,
                userDepartmentId = e.DepartmentId,
            }).FirstOrDefaultAsync();

            if (assignEmp == null)
            {
                return Json(new { status = false, message = "找不到此員工" });
            }

            var userWithGroups = await _context.User
            .Include(u => u.PermissionGroups).ThenInclude(pg => pg.Users).FirstOrDefaultAsync(u => u.UserName == empName);

            var userActivityList = userWithGroups.PermissionGroups
                .Where(u => u.PermissionGroupId == "G0001" || u.PermissionGroupId == "G0006" || u.PermissionGroupId == "G0008")
                .SelectMany(u => u.Users)
                .Select(u => u.UserName)
                .ToList();

            if (assignEmp.userDepartmentId == user.DepartmentId && userActivityList.Contains(empName))
            {
                return Json(new { status = true, user = assignEmp.userName });
            }
            else return Json(new { status = false });
        }

        //GET: /FormReview/Index/1

        [HttpGet]
		[Authorize]
		[AuthAttribute]
        public async Task<ActionResult> Index(string id)
        {
            User user = await GetAuthorizedUser();
            if (user == null)
            {
                return RedirectToAction("login", "Login");
            }

            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index", "ToDoList");
            }

            // 確認最新一筆工單紀錄是不是待審RS4 & user是否相同
            var todoReview = await _context.FormRecord
                                            .AsNoTracking()
                                            .Where(fr => fr.UserId == user.UserId && fr.ResultId == "RS4")
                                            .OrderByDescending(d=>d.ProcessingRecordId)
                                            .Select(fr => fr.UserId)
                                            .FirstOrDefaultAsync();

            bool lastestToDoReview = todoReview != null;

            if (lastestToDoReview)
            {
                // 審核方點進待辦清單=>點選單號=>回傳頁面
                var formViewModel = await GetFormReviewViewModel(id, user.UserId);
                return View(formViewModel);        
            }
            return RedirectToAction("Index", "ToDoList");
        }

        // POST :  FormReview/CreateAndUpdate/

        // 審核方輸入Remark 且點選核准or退回送出後觸發Action
        // 創建新的FormRecord來顯示目前審核過的紀錄
        // 同時創建新的FormRecord來顯示下一筆審核紀錄
        // 並更新該工單上的ProcessNodeId, or Manday
        [HttpPost]
		[Authorize]
		public async Task<IActionResult> CreateAndUpdate(FormReviewViewModel fvm, string reviewResult)
        {

            User user = await GetAuthorizedUser();

            try
            {
                // 查詢該筆工單的最新部門審核者部門Id, UserId
                var latestDetails = await _context.FormRecord
                                                    .AsNoTracking()
                                                    .Where(fr => fr.FormId == fvm.FormId && user.UserId == fr.UserId)
                                                    .OrderByDescending(f => f.ProcessingRecordId)
                                                    .Select(c => new 
                                                    {
                                                        c.ProcessingRecordId,
                                                        c.DepartmentId,
                                                        c.UserId,
                                                        c.GradeId,
                                                        c.UserActivityId,
                                                        c.Remark,
                                                    })
                                                    .FirstOrDefaultAsync();

                // 抓出提單及驗收方 UserActivityId
                var activityId = _context.UserActivity
                          .Where(c => c.UserActivityId == "09" || c.UserActivityId == "01")
                          .Select(m => m.UserActivityId)
                          .ToList();

                /* ------------ 如果審核方核准送出 -------------*/

                // 判斷如果審核方按下核准送出且目前功能編號不是驗收
                if (reviewResult == "approve")
                {
                    // 如果user不等於提單方or 驗收方
                    if ( !activityId.Contains(fvm.UserActivityId ))
                    {

                        // 查詢被指派的員工細項
                        var assignEmp = await _context.User.Where(u => u.UserName == fvm.AssginEmployee).Select(u => new
                        {
                            userId = u.UserId,
                            userDepartment = u.DepartmentId,
                            userGradeId = u.GradeId,
                        })
                        .FirstOrDefaultAsync();

                        // 呼叫創建ProcessingRecordId方法
                        List<string> formRecordIdList = await GetCreateFormRecordIdListAsync(2);

                        // 創建新的核准FormRecord
                        var addApprove = new FormRecord
                        {
                            ProcessingRecordId = formRecordIdList[0],
                            Remark = fvm.Remark,
                            FormId = fvm.FormId,
                            DepartmentId = latestDetails.DepartmentId,
                            UserId = latestDetails.UserId,
                            ResultId = "RS2",
                            UserActivityId = fvm.UserActivityId,
                            GradeId = latestDetails.GradeId,
                            Date = DateTime.UtcNow,
                            CreatedTime = DateTime.UtcNow,
                            UpdatedTime = DateTime.UtcNow,
                        };
                        _context.Add(addApprove);

                        // 抓下一筆UserId,  UserActivity, DepartmentId
                        // 抓該UserId的職等
                        var nextDetails = GetNextDetails(fvm, user.UserId, latestDetails.UserActivityId);
                        var nextGradeId = _context.User
                                                            .Where(u => u.UserId == nextDetails.UserId)
                                                            .Select(c => c.GradeId)
                                                            .FirstOrDefault();

                            // 現在這張工單的流程節點要是07
                        if (nextDetails.UserId != fvm.AssginEmployee && fvm.UserActivityId == "07")
                        {
                            // 創建新的審核中 FormRecord
                            var addNewAssignEmpNextReview = new FormRecord
                            {
                                ProcessingRecordId = formRecordIdList[1],
                                Remark = "",
                                FormId = fvm.FormId,
                                DepartmentId = assignEmp.userDepartment,
                                UserId = assignEmp.userId,
                                ResultId = "RS4",
                                UserActivityId = nextDetails.UserActivityId,
                                GradeId = assignEmp.userGradeId,
                                Date = DateTime.UtcNow,
                                CreatedTime = DateTime.UtcNow,
                                UpdatedTime = DateTime.UtcNow,
                            };

                            _context.Add(addNewAssignEmpNextReview);

                            // 查詢該流程節點表要更新的欄位
                            var processNodeToUpdate = await _context.ProcessNodes
                                .Where(pn => pn.FormId == fvm.FormId && pn.UserActivityId == "08")
                                .FirstOrDefaultAsync();

                            if (processNodeToUpdate != null)
                            {
                                // 更新該流程節點表的userId
                                processNodeToUpdate.UserId = assignEmp.userId;

                                _context.Update(processNodeToUpdate);
                                await _context.SaveChangesAsync();
                            }
                        }
                        else
                        {
                            // 創建新的審核中 FormRecord
                            var addNextReview = new FormRecord
                            {
                                ProcessingRecordId = formRecordIdList[1],
                                Remark = "",
                                FormId = fvm.FormId,
                                DepartmentId = nextDetails.DepartmentId,
                                UserId = nextDetails.UserId,
                                ResultId = "RS4",
                                UserActivityId = nextDetails.UserActivityId,
                                GradeId = nextGradeId,
                                Date = DateTime.UtcNow,
                                CreatedTime = DateTime.UtcNow,
                                UpdatedTime = DateTime.UtcNow,
                            };
                            _context.Add(addNextReview);
                        }

                        _context.SaveChanges();


                        // 查詢該工單要更新的欄位
                        var formToUpdate = await _context.Form
                                        .Where(f => f.FormId == fvm.FormId)
                                        .FirstOrDefaultAsync();

                        if (formToUpdate != null)
                        {
                            // 更新該工單的 ProcessNodeId, 更新時間
                            formToUpdate.ProcessNodeId = nextDetails.ProcessNodeId;
                            formToUpdate.UpdatedTime = DateTime.UtcNow;
                            if (user.UserId == assignEmp?.userId)
                            {
                                if (fvm.EstimatedTime == 0)
                                {
                                    formToUpdate.ManDay = null;
                                }
                                else
                                {
                                    formToUpdate.ManDay = fvm.EstimatedTime;
                                }
                                _context.Update(formToUpdate);
                            }
                            _context.Update(formToUpdate);
                            await _context.SaveChangesAsync();
                        }
                        // 找出formrecord更新完的userId找出其資料
                        var currentEmailEmp = await _context.FormRecord
                            .Where(u => u.FormId == fvm.FormId)
                            .OrderByDescending(d => d.ProcessingRecordId)
                            .Select(e => e.UserId)
                            .FirstOrDefaultAsync();
                        var recieveEmp = await _context.User.Where(u => u.UserId == currentEmailEmp).Select(c => c).FirstOrDefaultAsync();
                        emailService.SendFormReviewEmail(recieveEmp, fvm.FormId);
                    }

                    // 驗收階段
                    else
                    {
                        List<string> formRecordIdList = await GetCreateFormRecordIdListAsync(2);

                        // 創建核準FormRecord
                        var addApprove = new FormRecord
                        {
                            ProcessingRecordId = formRecordIdList[0],
                            Remark = fvm.Remark,
                            FormId = fvm.FormId,
                            DepartmentId = latestDetails.DepartmentId,
                            UserId = latestDetails.UserId,
                            ResultId = "RS2",
                            UserActivityId = fvm.UserActivityId,
                            GradeId = latestDetails.GradeId,
                            Date = DateTime.UtcNow,
                            CreatedTime = DateTime.UtcNow,
                            UpdatedTime = DateTime.UtcNow,
                        };

                        // 抓下一筆UserId,  UserActivity, DepartmentId
                        // 抓該UserId的職等
                        var nextDetails = GetNextDetails(fvm, user.UserId, latestDetails.UserActivityId);
                        var nextGradeId = _context.User
                                                            .Where(u => u.UserId == nextDetails.UserId)
                                                            .Select(c => c.GradeId)
                                                            .FirstOrDefault();

                        // 創建結案FormRecord
                        var addEnd = new FormRecord
                        {
                            ProcessingRecordId = formRecordIdList[1],
                            Remark = "",
                            FormId = fvm.FormId,
                            DepartmentId = nextDetails.DepartmentId,
                            UserId = nextDetails.UserId,
                            ResultId = "RS3",
                            UserActivityId = nextDetails.UserActivityId,
                            GradeId = nextGradeId,
                            Date = DateTime.UtcNow,
                            CreatedTime = DateTime.UtcNow,
                            UpdatedTime = DateTime.UtcNow,
                        };

                        _context.Add(addApprove);
                        _context.Add(addEnd);
                        _context.SaveChanges();

                        // 查詢該工單要更新的欄位
                        var formToUpdate = await _context.Form
                                        .Where(f => f.FormId == fvm.FormId)
                                        .FirstOrDefaultAsync();

                        if (formToUpdate != null)
                        {
                            // 更新該工單的 ProcessNodeId
                            formToUpdate.ProcessNodeId = nextDetails.ProcessNodeId;
                            formToUpdate.UpdatedTime = DateTime.UtcNow;
                            formToUpdate.ManDay = fvm.EstimatedTime;

                            // 保存更改
                            _context.Update(formToUpdate);
                            await _context.SaveChangesAsync();
                        }
                    }

                    await UploadFiles(fvm);
                    return RedirectToAction("Index", "ToDoList");
                }

                /* ------------ 如果審核方退回送出 -------------*/

                else
                {
                    //抓出該筆工單流程節點的第一筆功能編號
                    var firstUserActivtyId = _context.ProcessNodes
                        .Where(p => p.FormId == fvm.FormId)
                        .OrderBy(ua => ua.UserActivityId)
                        .Select(u => u.UserActivityId)
                        .FirstOrDefault();

                    // 如果審核方按下退回且目前該筆工單的工單紀錄的功能編號不是第一筆時
                    if (latestDetails.UserActivityId != firstUserActivtyId)
                    {
                        // 查詢該工單要更新的欄位
                        var formToUpdate = await _context.Form
                                        .Where(f => f.FormId == fvm.FormId)
                                        .FirstOrDefaultAsync();

                        List<string> formRecordIdList = await GetCreateFormRecordIdListAsync(2);

                        // 創建新的退回FormRecord
                        var addReject = new FormRecord
                        {
                            ProcessingRecordId = formRecordIdList[0],
                            Remark = fvm.Remark,
                            FormId = fvm.FormId,
                            DepartmentId = latestDetails.DepartmentId,
                            UserId = latestDetails.UserId,
                            ResultId = "RS1",
                            UserActivityId = latestDetails.UserActivityId,
                            GradeId = latestDetails.GradeId,
                            Date = DateTime.UtcNow,
                            CreatedTime = DateTime.UtcNow,
                            UpdatedTime = DateTime.UtcNow,
                        };

                        if (latestDetails.UserId == user.UserId && fvm.UserActivityId == "08")
                        {
                            if (formToUpdate != null)
                            {
                                // 更新該工單的Manday
                                formToUpdate.ManDay = fvm.EstimatedTime;
								formToUpdate.UpdatedTime = DateTime.UtcNow;
								_context.Update(formToUpdate);
                            }
                        }


                        // 抓上一筆UserId,  UserActivity, DepartmentId
                        // 抓該UserId的職等
                        var previousDetails = GetBackDetails(fvm, user.UserId, latestDetails.UserActivityId);
                        var previousGradeId = _context.User
                                                            .Where(u => u.UserId == previousDetails.UserId)
                                                            .Select(c => c.GradeId)
                                                            .FirstOrDefault();

                        // 創建新的審核中 FormRecord
                        var addNextReview = new FormRecord
                        {
                            ProcessingRecordId = formRecordIdList[1],
                            Remark = "",
                            FormId = fvm.FormId,
                            DepartmentId = previousDetails.DepartmentId,
                            UserId = previousDetails.UserId,
                            ResultId = "RS4",
                            UserActivityId = previousDetails.UserActivityId,
                            GradeId = previousGradeId,
                            Date = DateTime.UtcNow,
                            CreatedTime = DateTime.UtcNow,
                            UpdatedTime = DateTime.UtcNow,
                        };

                        _context.Add(addReject);
                        _context.Add(addNextReview);
                        _context.SaveChanges();

                        if (formToUpdate != null)
                        {
                            // 更新該工單的 ProcessNodeId
                            formToUpdate.ProcessNodeId = previousDetails.ProcessNodeId;
                            formToUpdate.UpdatedTime = DateTime.UtcNow;
                            // 保存更改
                            _context.Update(formToUpdate);
                            await _context.SaveChangesAsync();
                        }
                        // 找出formrecord更新完的userId找出其資料
                        var currentEmailEmp = await _context.FormRecord
                            .Where(u => u.FormId == fvm.FormId)
                            .OrderByDescending(d => d.ProcessingRecordId)
                            .Select(e => e.UserId)
                            .FirstOrDefaultAsync();
                        var recieveEmp = await _context.User.Where(u => u.UserId == currentEmailEmp).Select(c => c).FirstOrDefaultAsync();
                        emailService.SendFormReviewEmail(recieveEmp, fvm.FormId);
                    }
                    await UploadFiles(fvm);
                    return RedirectToAction("Index", "ToDoList");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json(new { status = false, messege = "指定人員不符合或不存在 ! " });
            }
        }

        /* ------------ 方法層 -------------*/

        // 查詢下一位資料
        public (string UserActivityId, string UserId, string DepartmentId, string ProcessNodeId) GetNextDetails(FormReviewViewModel fvm, string userId, string userActivityId)
        {

            // 抓出該工單流程節點的總長度
            var NodeLength = _context.ProcessNodes
            .Where(f => f.FormId == fvm.FormId)
            .Select(c => new
            {
                c.UserActivityId,
                c.UserId,
                c.DepartmentId,
                c.ProcessNodeId,
            })
            .ToList();

            // 找出目前該工單的流程節點表
            var currentDetails = _context.ProcessNodes.Where(f => f.FormId == fvm.FormId && f.UserId == userId && f.UserActivityId == userActivityId)
                .Select(pn => new
                {
                    pn.UserActivityId,
                    pn.UserId,
                    pn.DepartmentId,
                    pn.ProcessNodeId,
                })
                .FirstOrDefault();

            // 判定當前索引位置
            var currentIndex = NodeLength.FindIndex(n =>
                n.UserActivityId == currentDetails.UserActivityId &&
                n.UserId == currentDetails.UserId &&
                n.DepartmentId == currentDetails.DepartmentId &&
                n.ProcessNodeId == currentDetails.ProcessNodeId
            );



            // 宣告變數對其index做+1, 回傳tuple
            string nextUserActivity = null;
            string nextUserId = null;
            string nextDepartmentId = null;
            string nextProcessNodeId = null;
            if (currentIndex >= 0 && currentIndex < NodeLength.Count() - 1)
            {
                var nextIndex = NodeLength[currentIndex + 1];
                nextUserActivity = nextIndex.UserActivityId;
                nextUserId = nextIndex.UserId;
                nextDepartmentId = nextIndex.DepartmentId;
                nextProcessNodeId = nextIndex.ProcessNodeId;
            }
            return (nextUserActivity, nextUserId, nextDepartmentId, nextProcessNodeId);
        }

        // 查詢上一位資料
        public (string UserActivityId, string UserId, string DepartmentId, string ProcessNodeId) GetBackDetails(FormReviewViewModel fvm, string UserId, string userActivityId)
        {

            // 抓出該工單流程節點的總長度
            var NodeLength = _context.ProcessNodes
            .Where(f => f.FormId == fvm.FormId)
            .Select(c => new
            {
                c.UserActivityId,
                c.UserId,
                c.DepartmentId,
                c.ProcessNodeId,
            })
            .ToList();

            // 找出目前該工單的流程節點表
            var currentDetails = _context.ProcessNodes.Where(f => f.FormId == fvm.FormId && f.UserId == UserId && f.UserActivityId == userActivityId)
                .Select(pn => new
                {
                    pn.UserActivityId,
                    pn.UserId,
                    pn.DepartmentId,
                    pn.ProcessNodeId,
                })
                .FirstOrDefault();

            // 判定當前索引位置
            var currentIndex = NodeLength.FindIndex(n =>
                n.UserActivityId == currentDetails.UserActivityId &&
                n.UserId == currentDetails.UserId &&
                n.DepartmentId == currentDetails.DepartmentId &&
                n.ProcessNodeId == currentDetails.ProcessNodeId
            );

            // 宣告變數對其index做+1, 回傳tuple
            string previousUserActivity = null;
            string previousUserId = null;
            string previousDepartmentId = null;
            string previousProcessNodeId = null;
            if (currentIndex >= 0 && currentIndex < NodeLength.Count() - 1)
            {
                var previousIndex = NodeLength[currentIndex - 1];
                previousUserActivity = previousIndex.UserActivityId;
                previousUserId = previousIndex.UserId;
                previousDepartmentId = previousIndex.DepartmentId;
                previousProcessNodeId = previousIndex.ProcessNodeId;
            }
            return (previousUserActivity, previousUserId, previousDepartmentId, previousProcessNodeId);
        }

        // 檔案上傳
        public async Task UploadFiles(FormReviewViewModel fvm)
        {
            try
            {
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/upload", fvm.FormId);

                // 檢查資料夾是否存在，如果不存在則創建一個新資料夾
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // 檢查是否有上傳的檔案
                if (fvm.Files != null && fvm.Files.Count > 0)
                {
                    foreach (var file in fvm.Files)
                    {
                        // 檔案存放的完整路徑
                        var filePath = Path.Combine(folderPath, DateTime.UtcNow.AddHours(8).ToString("yyyy-MM-dd-HHmmss-") + file.FileName);

                        // 保存檔案
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("檔案上傳失敗: " + ex.Message);
            }
        }

        // 下載檔案
        [HttpPost]
        public IActionResult Download(string id)
        {
            //讀取檔案
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "upload");
            filePath = Path.Combine(filePath, id);

            if (!Directory.Exists(filePath))
            {
                return Content("無檔案可下載");
            }

            string[] allFiles = Directory.GetFiles(filePath, "*", SearchOption.AllDirectories);

            if(allFiles.Length == 0)
            {
                return Content("無檔案可下載");
            }

            byte[] data = null;
            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (var file in allFiles)
                    {
                        archive.CreateEntryFromFile(file, Path.GetFileName(file));
                    }
                }
                memoryStream.Seek(0, SeekOrigin.Begin);
                data = memoryStream.ToArray();
            }

            return File(data, "application/zip", "test.zip");

        }


        // 判斷檔案的MIME類型
        private string GetContentType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return extension switch
            {
                ".pdf" => "application/pdf",
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".txt" => "text/plain",
                ".doc" => "application/msword",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".xls" => "application/vnd.ms-excel",
                ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                _ => "application/octet-stream", // 預設二進位檔案類型
            };
        }

        // 回傳ViewModel
        private async Task<FormReviewViewModel> GetFormReviewViewModel(string formId, string userId)
        {
            User user = await GetAuthorizedUser();
            // 目前最新進度
            var latestStatus = await _context.FormRecord
                .AsNoTracking()
                .Where(c => c.FormId == formId && userId == c.UserId)
                .OrderByDescending(t => t.ProcessingRecordId)
                .Select(c => new
                {
                    resultId = c.ResultId,
                    userActivityId = c.UserActivityId,
                    userId = c.UserId,
                    userName = user.UserName,
                })
                .FirstOrDefaultAsync();


            //顯示在View 欄位的畫面
            var formViewModel = await _context.Form
                .AsNoTracking()
                .Where(c => c.FormId == formId)
                .Select(m => new FormReviewViewModel
                {
                    FormId = m.FormId,
                    UserName = latestStatus.userName,
                    UserId = userId,
                    Date = m.Date,
                    CategoryId = m.CategoryId,
                    CategoryDescription = m.Category.CategoryDescription,
                    DepartmentId = m.DepartmentId,
                    DepartmentName = m.Department.DepartmentName,
                    CurrentResults = latestStatus.resultId ?? "Unknown",
                    CurrentResultsDescription = _context.Result
                        .Where(r => r.ResultId == latestStatus.resultId)
                        .Select(r => r.ResultDescription)
                        .FirstOrDefault(),
                    NeedEmployees = _context.User
                        .Where(u => u.UserId == m.UserId)
                        .Select(u => u.UserName)
                        .FirstOrDefault(),
                    HopeFinishDate = m.ExpectedFinishedDay,
                    BelongProjects = m.Project.ProjectName,
                    EstimatedTime = m.ManDay.HasValue ? (int)m.ManDay : 0,
                    Content = m.Content,
                    UserActivityId = latestStatus.userActivityId,
                    ProcessNodeId = m.ProcessNodeId,
                    FormProcessFlow = _context.ProcessNodes.Include(c => c.UserActivity)
                     .Where(c => c.FormId == formId)
                     .AsNoTracking()
                     .AsSplitQuery()
                     .Select(c => new FormReviewProcessFlowViewModel
                     {
                         ProcessNodeId = c.ProcessNodeId,
                         UserActivityId = c.UserActivityId,
                         UserActivityIdDescription = c.UserActivity.UserActivityIdDescription

                     }).ToList(),
                    FormRecordList = _context.FormRecord.Where(fr => fr.FormId == formId)
                        .Select(fr => new FormReviewFormProcessViewModel
                        {
                            Date = fr.Date,
                            UserActivityDes = fr.UserActivity.UserActivityIdDescription,
                            UserName = fr.User.UserName,
                            Remark = fr.Remark,
                            ResultDes = fr.Result.ResultDescription,
                        }).ToList(),
                })
                    .FirstOrDefaultAsync();

            //流程進度
            // 該工單的流程節點全部
            var processNode = _context.ProcessNodes.Include(c => c.UserActivity)
                 .Where(c => c.FormId == formId)
                 .AsNoTracking()
                 .AsSplitQuery()
                 .Select(c => new FormReviewProcessFlowViewModel
                 {
                     ProcessNodeId = c.ProcessNodeId,
                     UserActivityId = c.UserActivityId,
                     UserActivityIdDescription = c.UserActivity.UserActivityIdDescription

                 }).ToList();

            if (processNode.Any(c => c.ProcessNodeId == formViewModel.ProcessNodeId)) //Any適用判斷true/false
            {
                var node = processNode.FirstOrDefault(c => c.ProcessNodeId == formViewModel.ProcessNodeId);
                node.IsHightLight = true;
            }
            formViewModel.FormProcessFlow = processNode;

            return formViewModel;
        }

        // 寄信給下一位
        //private async void FindAndSendNextEmployeeEmail(FormReviewViewModel fvm)
        //{
        //    var currentEmailEmp = await _context.FormRecord
        //        .Where(u => u.FormId == fvm.FormId)
        //        .OrderByDescending(d => d.ProcessingRecordId)
        //        .Select(e => e.UserId)
        //        .FirstOrDefaultAsync();

        //    var recieveEmp = await _context.User.Where(u => u.UserId == currentEmailEmp).Select(c => c).FirstOrDefaultAsync();

        //    emailService.SendFormReviewEmail(recieveEmp.Email, recieveEmp.UserName, fvm.FormId);
        //}
    }
}
