using BPMPlus.Data;
using BPMPlus.Models;
using BPMPlus.ViewModels;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;

namespace BPMPlus.Controllers
{
    
    public class FormDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FormDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

    // GET: FormDetails
    //public IActionResult Index()
    //{
    //    return View();
    //}


    //[HttpPost]
    // GET: FormDetails
    public IActionResult Index(string id)
    {
      var formdata = _context.Form
              .Include(c => c.ProcessNode).ThenInclude(c => c.UserActivity)
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
                ProjectName = c.Project.ProjectName,
                CategoryDescription = c.Category.CategoryDescription,
                Tel = c.Tel,
                ExpectedFinishedDay = c.ExpectedFinishedDay.ToString("yyyy-MM-dd"),
                Content = c.Content,
                Date = c.Date.AddHours(8).ToString("yyyy-MM-dd"),
                DepartmentName = c.Department.DepartmentName,
                EmployeeName = c.User.UserName,
                ProcessNodeId=c.ProcessNodeId,
                FormIsActive=true,
                FormDetailsProcessNodes = c.ProcessNode.Select(d => new FormDetailsProcessNodeViewModel
                {
                  ProcessNodeId=d.ProcessNodeId,
                }).ToList(),
                FormDetailsFormProcesses = c.FormRecord.Select(d => new FormDetailsFormProcessViewModel
                {
                  Date = d.Date.AddHours(8).ToString("yyyy-MM-dd HH:mm:ss"),
                  UserActivityIdDescription = d.UserActivity.UserActivityIdDescription,
                  EmployeeName = d.User.UserName,
                  Remark = d.Remark,
                  ResultDescription = d.Result.ResultDescription
                }).ToList()
              }).FirstOrDefault();

            //FormDetailsUserActivityViewModel
            var processNode = _context.ProcessNodes.Include(c=>c.UserActivity)
                 .Where(c=>c.FormId==formdata.FormId)
                 .AsNoTracking()
                 .AsSplitQuery()
                 .Select(c => new FormDetailsProcessFlowViewModel
                 {
                     ProcessNodeId=c.ProcessNodeId,
                     UserActivityId = c.UserActivityId,
                     UserActivityIdDescription = c.UserActivity.UserActivityIdDescription

                 }).ToList();

            if(processNode.Any(c => c.ProcessNodeId == formdata.ProcessNodeId)) //Any適用判斷true/false
                processNode.FirstOrDefault(c=>c.ProcessNodeId== formdata.ProcessNodeId).IsHightLight = true;

            //FormDetailsGroupViewModel
            var result = new FormDetailsGroupViewModel
            {
                FormDetails = formdata,
                FormDetailsProcesseFlows = processNode,

            };


            return View(result ?? new FormDetailsGroupViewModel());
    }

        [HttpPost]
        public async Task<IActionResult> Invalid(string id, FormDetailsGroupViewModel model)
        {

            //確認傳入參數id是否有值
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("ID cannot be null or empty.");
            }

            //找出要傳入的單筆紀錄和欄位
            var invalidId = _context.Form.FirstOrDefault(c => c.FormId == id);
            invalidId.FormIsActive = false;


            try
            {
                _context.Update(invalidId);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FormExists(model.FormDetails.FormId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            //返回查詢工單頁面
            return RedirectToAction("Index", "QueryForms");
        }

      

        private bool FormExists(string id)
        {
            return _context.Form.Any(e => e.FormId == id);
        }

    }
}
