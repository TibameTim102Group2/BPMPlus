using BPMPlus.Data;
using BPMPlus.Models;
using BPMPlus.ViewModels;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Hosting;
using System.IO.Compression;
using System.IO;

namespace BPMPlus.Controllers
{
    
    public class FormDetailsController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FormDetailsController(ApplicationDbContext context, IWebHostEnvironment WebHostEnvironment) : base(context)
        {
            _context = context;
            _webHostEnvironment = WebHostEnvironment;
        }

   

        [Authorize]

        [HttpGet]
        // GET: FormDetails
        public async Task<IActionResult> Index(string id)
    {
            User user = await GetAuthorizedUser();

            var formdata = _context.Form
                  .Include(c => c.ProcessNode).ThenInclude(p => p.UserActivity)
                  .Include(c => c.FormRecord)
                  .Include(c => c.Project)
                  .Include(c => c.Category)
                  .Include(c => c.Department)
                  .Include(c => c.User)
                  .Where(c => c.FormId == id.ToString())
                  .AsNoTracking()
                  .AsSplitQuery()
                  .Select(c => new FormDetailsViewModel   //FormDetailsViewModel欄位
                  {
                    FormId = c.FormId,
                    Id=c.FormId,
                    UserId=c.UserId,
                    ProjectName = c.Project.ProjectName,
                    CategoryDescription = c.Category.CategoryDescription,
                    Tel = c.Tel,
                    ExpectedFinishedDay = c.ExpectedFinishedDay.AddHours(8).ToString("yyyy-MM-dd"),
                    Content = c.Content,
                    Date = c.Date.AddHours(8).ToString("yyyy-MM-dd"), //當地時間加8小時
                    DepartmentName = c.Department.DepartmentName,
                    EmployeeName = c.User.UserName,
                    ProcessNodeId=c.ProcessNodeId,
                    FormIsActive=c.FormIsActive,
                    FormDetailsProcessNodes = c.ProcessNode.Select(d => new FormDetailsProcessNodeViewModel
                    {
                      ProcessNodeId=d.ProcessNodeId
                    }).ToList(),
                    FormDetailsFormProcesses = c.FormRecord.Select(d => new FormDetailsFormProcessViewModel
                    {
                      Date = d.Date.AddHours(8).ToString("yyyy-MM-dd HH:mm:ss"), //當地時間加8小時
                      UserActivityIdDescription = d.UserActivity.UserActivityIdDescription,
                      EmployeeName = d.User.UserName,
                      Remark = d.Remark,
                      ResultDescription = d.Result.ResultDescription
                    }).ToList()
                  }).FirstOrDefault();

            //流程進度
            var processFlow = _context.ProcessNodes.Include(c=>c.UserActivity)
                 .Where(c=>c.FormId==formdata.FormId)
                 .AsNoTracking()
                 .AsSplitQuery()
                 .Select(c => new FormDetailsProcessFlowViewModel
                 {
                     ProcessNodeId=c.ProcessNodeId,
                     UserActivityId = c.UserActivityId,
                     UserActivityIdDescription = c.UserActivity.UserActivityIdDescription

                 }).ToList();

            //判斷登入者是否工單申請人
            if (formdata.UserId == user.UserId)
                formdata.IsUser = true;

            //判斷流程節點位置
            if (processFlow.Any(c => c.ProcessNodeId == formdata.ProcessNodeId)) //Any適用判斷true/false
                processFlow.FirstOrDefault(c=>c.ProcessNodeId== formdata.ProcessNodeId).IsHightLight = true;

            //抓取流程節點清單include功能
            var ProcessNodes = _context.ProcessNodes.Include(c=>c.UserActivity)
                        .Where(c=>c.FormId==formdata.FormId)
                        .ToList();

            //判斷功能是否申請人階段
            if (ProcessNodes.FirstOrDefault(c=> c.ProcessNodeId==formdata.ProcessNodeId).UserActivityId=="01")
                formdata.IsApply = true;




            //FormDetailsGroupViewModel
            var result = new FormDetailsGroupViewModel
            {
                FormDetails = formdata,
                FormDetailsProcesseFlows = processFlow,

            };


            return View(result ?? new FormDetailsGroupViewModel());
    }
        [HttpPost]
		[Authorize]
		public async Task<IActionResult> Invalid(string id)
        {


            //確認傳入參數id是否有值
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("ID cannot be null or empty.");
            }

            //找出要傳入的單筆紀錄和欄位
            var invalidId = _context.Form.FirstOrDefault(c => c.FormId == id);
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

       

        [HttpPost]
		[Authorize]
		public IActionResult Download(string id)
        {
            //讀取檔案
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "upload");
            filePath = Path.Combine(filePath,id);

            // 檢查資料夾是否存在
            if (!Directory.Exists(filePath))
            {
                return Content("無檔案可下載");
            }

            string[] allFiles = Directory.GetFiles(filePath, "*", SearchOption.AllDirectories);

            // 如果資料夾中沒有檔案
            if (allFiles.Length == 0)
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



        private bool FormExists(string id)
        {
            return _context.Form.Any(e => e.FormId == id);
        }

    }
}
