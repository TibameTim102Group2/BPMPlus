﻿using BPMPlus.Data;
using BPMPlus.Models;
using BPMPlus.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using System.IO.Compression;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BPMPlus.Controllers
{
    public class FormReviewController : BaseController
    {
        ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FormReviewController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment) : base(context)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }


        [HttpGet]
        public async Task<JsonResult> CheckEmp(string id)
        {
            // 查詢該工單的流程節點表的功能編號是否跟最新工單紀錄的功能編號相同
            // 且該工單的責成人員是否跟工單紀錄最新的責成人員相同
            // 是的話抓取其UserId
            User user = await GetAuthorizedUser();

            var latestStatus = await _context.FormRecord
                                               .AsNoTracking()
                                               .Where(c => c.FormId == id && user.UserId == c.UserId)
                                               .OrderByDescending(t => t.CreatedTime)
                                               .Select(c => new
                                               {
                                                   resultId = c.ResultId,
                                                   userActivityId = c.UserActivityId,
                                                   userId = c.UserId,
                                                   formId = c.FormId,
                                               })
                                               .FirstOrDefaultAsync();

            if (latestStatus == null)
            {
                return Json(new { status = false });
            }

            var assignEmp = await _context.ProcessNodes
                                            .AsNoTracking()
                                            .Where(pn => pn.FormId == id && pn.UserActivityId == latestStatus.userActivityId && pn.UserId == latestStatus.userId)
                                            .Select(pn => pn.UserId)
                                            .FirstOrDefaultAsync();
            var pnUser = await _context.ProcessNodes.Where(pn => pn.FormId == id && pn.UserActivityId == "08").Select(pn => pn.UserId).FirstOrDefaultAsync();
            var Handler = await _context.User.Where(u => u.UserId == pnUser).Select(u => u.UserName).FirstOrDefaultAsync();

            // 一進到審核頁發請求詢問身分是否為接收方一級主管or 處理人員 or 驗收方
            if (assignEmp != null)
            {
                if (user.PermittedTo("07"))
                {
                    return Json(new { status = true, userPermit = "07", handler = Handler });
                }
                else if (user.PermittedTo("08"))
                {
                    return Json(new { status = true, userPermit = "08", handler = Handler });
                }
                else if (user.PermittedTo("09"))
                {
                    return Json(new { status = true, userPermit = "09", handler = Handler });
                }
            }
            else return Json(new { status = false });

            return Json(new { status = false });
        }

        //GET: /FormReview/Index/1

        [HttpGet]
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

            var todoReview = await _context.FormRecord
                                            .AsNoTracking()
                                            .Where(fr => fr.UserId == user.UserId && fr.ResultId == "RS4")
                                            .Select(fr => fr.UserId)
                                            .FirstOrDefaultAsync();

            if (user.UserId == todoReview)
            {
                var formVW = await GetFormReviewViewModel(id, user.UserId);
                return View(formVW);         // 審核方點進待辦清單=>點選單號=>回傳頁面
            }
            return RedirectToAction("Index", "ToDoList");
        }

        private async Task<FormReviewViewModel> GetFormReviewViewModel(string formId, string userId)
        {
            User user = await GetAuthorizedUser();
            // 目前進度
            var latestStatus = await _context.FormRecord
                .AsNoTracking()
                .Where(c => c.FormId == formId && userId == c.UserId)
                .OrderByDescending(t => t.CreatedTime)
                .Select(c => new
                {
                    resultId = c.ResultId,
                    userActivityId = c.UserActivityId,
                    userId = c.UserId,
                    userName = user.UserName,
                })
                .FirstOrDefaultAsync();


            //顯示在View 欄位的畫面
            var formVW = await _context.Form
                .AsNoTracking()
                .Where(c => c.FormId == formId)
                .Select(m => new FormReviewViewModel
                {
                    FormId = m.FormId,
                    UserName = latestStatus.userName,
                    UserId = userId,
                    Date = m.Date,
                    CategoryId = m.CategoryId,
                    DepartmentId = m.DepartmentId,
                    CurrentResults = latestStatus.resultId ?? "Unknown",
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

            if (processNode.Any(c => c.ProcessNodeId == formVW.ProcessNodeId)) //Any適用判斷true/false
            {
                var node = processNode.FirstOrDefault(c => c.ProcessNodeId == formVW.ProcessNodeId);
                node.IsHightLight = true;
            }
            formVW.FormProcessFlow = processNode;

            return formVW;
        }


        // POST :  FormReview/Create/1

        // 審核方輸入Remark 且點選核准or退回送出後觸發Action
        // 創建新的FormRecord來顯示目前審核過的紀錄
        // 同時創建新的FormRecord來顯示下一筆審核紀錄
        // 並更新該工單上的ProcessNodeId
        [HttpPost]
        public async Task<IActionResult> CreateAndUpdate(FormReviewViewModel fvm, string reviewResult)
        {

            User user = await GetAuthorizedUser();

            try
            {
                // 查詢該筆工單的最新部門審核者部門Id, UserId
                var currentDetails = await _context.FormRecord
                                                    .Where(fr => fr.FormId == fvm.FormId && user.UserId == fr.UserId)
                                                    .OrderByDescending(f => f.CreatedTime)
                                                    .Select(c => new 
                                                    {
                                                        crProcessingRecord = c.ProcessingRecordId,
                                                        crDepartment = c.DepartmentId,
                                                        crUserId = c.UserId,
                                                        crGrade = c.GradeId,
                                                        crUserActivityId = c.UserActivityId,
                                                        crRemark = c.Remark,
                                                    })
                                                    .FirstOrDefaultAsync();

                // 抓出UserActivtiy內的驗收id
                var check = _context.UserActivity
                                        .Where(c => c.UserActivityId == "09")
                                        .Select(m => m.UserActivityId)
                                        .FirstOrDefault();

                var poster = _context.UserActivity
                            .Where(c => c.UserActivityId == "01")
                            .Select(m => m.UserActivityId)
                            .FirstOrDefault();



                /* ------------ 如果審核方核准送出 -------------*/

                // 判斷如果審核方按下核准送出且目前功能編號不是驗收
                if (reviewResult == "approve")
                {
                    // 如果user不等於提單方or 驗收方
                    if (fvm.UserActivityId != check && fvm.UserActivityId != poster)
                    {
                        List<string> formRecordIdList = await GetCreateFormRecordIdListAsync(2);

                        // 創建新的核准FormRecord
                        var addApprove = new FormRecord
                        {
                            ProcessingRecordId = formRecordIdList[0],
                            Remark = fvm.Remark,
                            FormId = fvm.FormId,
                            DepartmentId = currentDetails.crDepartment,
                            UserId = currentDetails.crUserId,
                            ResultId = "RS2",
                            UserActivityId = fvm.UserActivityId,
                            GradeId = currentDetails.crGrade,
                            Date = DateTime.UtcNow,
                            CreatedTime = DateTime.UtcNow,
                            UpdatedTime = DateTime.UtcNow,
                        };

                        // 抓下一筆UserId,  UserActivity, DepartmentId
                        // 抓該UserId的職等
                        var nextDetails = GetNextDetails(fvm, user.UserId, currentDetails.crUserActivityId);
                        var nextGradeId = _context.User
                                                            .Where(u => u.UserId == nextDetails.UserId)
                                                            .Select(c => c.GradeId)
                                                            .FirstOrDefault();

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

                        _context.Add(addApprove);
                        _context.Add(addNextReview);
                        _context.SaveChanges();

                        // 查詢該工單要更新的欄位
                        var formToUpdate = await _context.Form
                                        .Where(f => f.FormId == fvm.FormId)
                                        .FirstOrDefaultAsync();



                        if (formToUpdate != null)
                        {
                            // 更新該工單的 ProcessNodeId
                            formToUpdate.ProcessNodeId = nextDetails.ProcessNodeId;
                            formToUpdate.ManDay = fvm.EstimatedTime;

                            // 保存更改
                            _context.Update(formToUpdate);
                            await _context.SaveChangesAsync();
                        }

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
                            DepartmentId = currentDetails.crDepartment,
                            UserId = currentDetails.crUserId,
                            ResultId = "RS2",
                            UserActivityId = fvm.UserActivityId,
                            GradeId = currentDetails.crGrade,
                            Date = DateTime.UtcNow,
                            CreatedTime = DateTime.UtcNow,
                            UpdatedTime = DateTime.UtcNow,
                        };

                        // 抓下一筆UserId,  UserActivity, DepartmentId
                        // 抓該UserId的職等
                        var nextDetails = GetNextDetails(fvm, user.UserId, currentDetails.crUserActivityId);
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
                    if (currentDetails.crUserActivityId != firstUserActivtyId)
                    {
                        List<string> formRecordIdList = await GetCreateFormRecordIdListAsync(2);

                        // 創建新的退回FormRecord
                        var addReject = new FormRecord
                        {
                            ProcessingRecordId = formRecordIdList[0],
                            Remark = fvm.Remark,
                            FormId = fvm.FormId,
                            DepartmentId = currentDetails.crDepartment,
                            UserId = currentDetails.crUserId,
                            ResultId = "RS1",
                            UserActivityId = currentDetails.crUserActivityId,
                            GradeId = currentDetails.crGrade,
                            Date = DateTime.UtcNow,
                            CreatedTime = DateTime.UtcNow,
                            UpdatedTime = DateTime.UtcNow,
                        };

                        // 抓上一筆UserId,  UserActivity, DepartmentId
                        // 抓該UserId的職等
                        var previousDetails = GetBackDetails(fvm, user.UserId, currentDetails.crUserActivityId);
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

                        // 查詢該工單要更新的欄位
                        var formToUpdate = await _context.Form
                                        .Where(f => f.FormId == fvm.FormId)
                                        .FirstOrDefaultAsync();

                        if (formToUpdate != null)
                        {
                            // 更新該工單的 ProcessNodeId
                            formToUpdate.ProcessNodeId = previousDetails.ProcessNodeId;

                            // 保存更改
                            _context.Update(formToUpdate);
                            await _context.SaveChangesAsync();
                        }
                    }
                    await UploadFiles(fvm);
                    // 目前工單紀錄功能編號是第一筆時
                    return RedirectToAction("Index", "ToDoList");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("請洽系統管理員");
            }
        }


        [HttpPost]
        public IActionResult Download(string id)
        {
            //讀取檔案
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "upload");
            filePath = Path.Combine(filePath, id);
            byte[] data = null;
            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    string[] allFiles = Directory.GetFiles(filePath, "*", SearchOption.AllDirectories);
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
            catch(Exception ex)
            {
                Console.WriteLine("檔案上傳失敗: " + ex.Message);
            }
        }

        // 新建工單
        //public async Task CreateApproveFormRecord(FormReviewViewModel fvm, dynamic details, List<string> formRecordIdList)
        //{
        //    var formRecord = new FormRecord
        //    {
        //        ProcessingRecordId = formRecordIdList[0],
        //        Remark = fvm.Remark,
        //        FormId = fvm.FormId,
        //        DepartmentId = details.DepartmentId,
        //        UserId = details.UserId,
        //        ResultId = "RS2",
        //        UserActivityId = fvm.UserActivityId,
        //        GradeId = details.
        //        Date = DateTime.UtcNow,
        //        CreatedTime = DateTime.UtcNow,
        //        UpdatedTime = DateTime.UtcNow,
        //    };

        //    _context.Add(formRecord);
        //    _context.SaveChanges();

        //} 

    }
}