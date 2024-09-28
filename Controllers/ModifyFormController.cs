using BPMPlus.Data;
using BPMPlus.Models;
using BPMPlus.Service;
using BPMPlus.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Threading.Channels;

namespace BPMPlus.Controllers
{
    public class ModifyFormController : BaseController
    {

        private readonly ApplicationDbContext _context;
        private readonly EmailService emailService;

        public ModifyFormController(ApplicationDbContext context, EmailService emailService) : base(context)
        {
            _context = context;
            this.emailService = emailService;
        }

        [Authorize]

        [HttpGet]
        public IActionResult Index(string id)
        {

            ////CategoryViewModel選單用
            //var categories = _context.Category
            //     .AsNoTracking()
            //     .AsSplitQuery()
            //     .Select(c => new ModifyFormCategoryViewModel
            //     {
            //         CategoryDescription = c.CategoryDescription,
            //     }).ToList();

            ////ProjectViewModel選單用
            //var projects = _context.Project
            //  .AsNoTracking()
            //  .AsSplitQuery()
            //  .Select(c => new ModifyFormProjectViewModel
            //  {
            //      ProjectName = c.ProjectName
            //  }).ToList();

            //ModifyFormViewModel
            var formdata = _context.Form
                .Include(c => c.User)
                .Include(c => c.Department)
                .Include(c => c.Category)
                .Include(c => c.Project)
                .Where(c => c.FormId == id)
                .AsNoTracking()
                .AsSplitQuery()
                .Select(c => new ModifyFormViewModel
                {
                    FormId = c.FormId,
                    Id=c.FormId,
                    EmployeeName = c.User.UserName,
                    Date = c.Date.AddHours(8).ToString("yyyy-MM-dd"),
                    CategoryDescription = c.Category.CategoryDescription,
                    ProjectName = c.Project.ProjectName,
                    ExpectedFinishedDay = c.ExpectedFinishedDay.AddHours(8).ToString("yyyy-MM-dd"),
                    DepartmentName = c.Department.DepartmentName,
                    Content = c.Content,
                    Tel = c.Tel,
                    FormIsActive = c.FormIsActive
                }).FirstOrDefault();

            //FormGroupViewModel
            var result = new ModifyFormGroupViewModel
            {
                ModifyFrom = formdata??new ModifyFormViewModel(),
                //Categories = categories,
                //Projects = projects
            };


            return View(result ?? new ModifyFormGroupViewModel());
        }


		[Authorize]
		[HttpPost]
        public async Task<IActionResult>Invalid(string id) 
        {

            //確認傳入參數id是否有值
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("ID cannot be null or empty.");
            }

            //找出要傳入的單筆紀錄和欄位
            var invalidId = _context.Form.FirstOrDefault(c=>c.FormId== id);
            invalidId.FormIsActive = false;

            await _context.SaveChangesAsync();

            //try
            //{
            //    _context.Update(invalidId);
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //if (!FormExists(model.ModifyFrom.FormId))
            //{
            //    return NotFound();
            //}
            //else
            //{
            //    throw;
            //}
            //}

            //返回查詢工單頁面
            return Json(new { success = true, message = "作廢成功" });

        }

		[Authorize]
		[HttpPost]
        public async Task<IActionResult> Edit(UploadInputModel data)
        {

            try
            {
                /// <summary>
                /// 表單修改
                /// </summary>
               

                if (data.Content == null)
                {
                    return Json(new { success = false, message = "需求內容不可為空" });
                }

                if (data.Content.Length > 10000)
                {
                    return Json(new { success = false, message = "需求內容不得超過一萬字元" });
                }

                if (data.Enddate == null || data.Enddate< (DateTime.UtcNow.AddDays(-1)))
                {
                    return Json(new { success = false, message = "完成日期不可為空或早於今日" });
                }

                var form = _context.Form.FirstOrDefault(x => x.FormId == data.Id);
                form.Content = data.Content; //需求內容
                form.ExpectedFinishedDay = data.Enddate;  //希望完成日期
                form.UpdatedTime = DateTime.UtcNow; //更新時間
                
                //產生某筆工單的所有ProcessNode
                List<ProcessNode> processNodes = await _context.ProcessNodes
                    .Where(p => p.FormId == data.Id)
                    .ToListAsync();

                form.ProcessNodeId = processNodes[1].ProcessNodeId; //流程節點+1


                /// <summary>
                /// 上傳附件
                /// </summary>

                // 指定專案資料夾名稱
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/upload", data.Id);

                // 檢查資料夾是否存在，如果不存在則創建一個新資料夾
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // 檢查是否有上傳的檔案
                if (data.Files != null && data.Files.Count > 0)
                {
                    foreach (var file in data.Files)
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


                /// <summary>
                /// 新增工單紀錄
                /// </summary>

                //修改工單者processNode對應責成人員
                User userModifier = await _context.User
                    .Include(c => c.Grade)
                    .FirstOrDefaultAsync(u => u.UserId == processNodes[0].UserId);

                //審核工單者processNode對應責成人員
                User userReviewer = await _context.User
                    .Include(c => c.Grade)
                    .FirstOrDefaultAsync(u => u.UserId == processNodes[1].UserId);


                List<string> formRecordIdList = await GetCreateFormRecordIdListAsync(2);
                FormRecord modifyFormRecord = new FormRecord(), firstReviewFormRecord = new FormRecord();

                modifyFormRecord.ProcessingRecordId = formRecordIdList[0];
                modifyFormRecord.Remark = "";
                modifyFormRecord.FormId = form.FormId;
                modifyFormRecord.DepartmentId = form.DepartmentId;
                modifyFormRecord.UserId = form.UserId;
                modifyFormRecord.ResultId = "RS2";
                modifyFormRecord.UserActivityId = processNodes[0].UserActivityId;
                modifyFormRecord.GradeId = userModifier.GradeId; 
                modifyFormRecord.Date = DateTime.UtcNow;
                modifyFormRecord.UpdatedTime = DateTime.UtcNow;
                modifyFormRecord.CreatedTime = DateTime.UtcNow;


                firstReviewFormRecord.ProcessingRecordId = formRecordIdList[1];
                firstReviewFormRecord.Remark = "";
                firstReviewFormRecord.FormId = form.FormId;
                firstReviewFormRecord.DepartmentId = form.DepartmentId;
                firstReviewFormRecord.UserId = processNodes[1].UserId;
                firstReviewFormRecord.ResultId = "RS4";
                firstReviewFormRecord.UserActivityId = processNodes[1].UserActivityId;
                firstReviewFormRecord.GradeId = userReviewer.GradeId;
                firstReviewFormRecord.Date = DateTime.UtcNow;
                firstReviewFormRecord.UpdatedTime = DateTime.UtcNow;
                firstReviewFormRecord.CreatedTime = DateTime.UtcNow;


                
                await _context.FormRecord.AddAsync(modifyFormRecord);
                await _context.FormRecord.AddAsync(firstReviewFormRecord);

                //儲存變更
                await _context.SaveChangesAsync();

                var currentEmailEmp = await _context.FormRecord
                    .Where(u => u.FormId == firstReviewFormRecord.FormId)
                    .OrderByDescending(d => d.ProcessingRecordId)
                    .Select(e => e.UserId)
                    .FirstOrDefaultAsync();
                var recieveEmp = await _context.User.Where(u => u.UserId == currentEmailEmp).Select(c => c).FirstOrDefaultAsync();
                emailService.SendFormReviewEmail(recieveEmp, firstReviewFormRecord.FormId);
                return Json(new { success = true, message = "修改成功" });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "修改失敗" });
            }


            
        }


        private bool FormExists(string id)
        {
            return _context.Form.Any(e => e.FormId == id);
        }
    }
}
