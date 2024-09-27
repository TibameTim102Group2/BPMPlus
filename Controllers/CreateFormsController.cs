using BPMPlus.Data;
using BPMPlus.Models;
using BPMPlus.Service;
using BPMPlus.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Data;
using System.Security.Claims;

namespace BPMPlus.Controllers
{
    
    public class CreateFormsController : BaseController
    {

        private readonly ApplicationDbContext _context;
        private readonly EmailService emailService;

        public CreateFormsController(ApplicationDbContext context, EmailService emailService) :base(context)
        {
            _context = context;
            this.emailService = emailService;
        }
        // GET: CreateForms
        [Authorize]

        public async Task<ActionResult> Index()
        {
            User user = await GetAuthorizedUser();
            //functionId:  01 -> 需求方申請人送出
            
            if (!user.PermittedTo("01"))
            {
                ViewBag.NotPermittedToCreateForm = "您的權限無法新建工單";
                return View("~/Views/Home/Index.cshtml");
            }

            var Department = await _context.Department
                 .FirstOrDefaultAsync(d => d.DepartmentId == user.DepartmentId);
            
            if (Department == null)
            {
                throw new Exception("Department is null, Server Error");
            }
            
            var Categories = await _context.Category.ToListAsync();
            if (Categories == null)
            {
                throw new Exception("Categories is null, Server Error");
            }
            
            
            

            var Forms = await _context.Form
            .Where(f => f.DepartmentId == user.DepartmentId)
            .ToListAsync(
                
            );


            ViewBag.MinDate = DateTime.Now.AddHours(8);
            ViewBag.DepartmentName = Department.DepartmentName;
            ViewBag.DepartmentId = Department.DepartmentId;
            ViewBag.UserId = user.UserId;
            ViewBag.UserTEL = user.TEL;
            ViewBag.Categories = Categories;
            ViewBag.Projects = user.Projects;
            ViewBag.Forms = Forms;

            return View();
            
        }
        [Authorize]
        [HttpGet]
        public async Task<JsonResult> GetFormById(string formId)
        {
            
            User user = await GetAuthorizedUser();
            //functionId:  01 -> 需求方申請人送出
            
            if (!user.PermittedTo("01"))
            {
                throw new Exception("User is not permitted)");
            }

            var form = await _context.Form.FirstOrDefaultAsync(
                f => f.FormId == formId
            );
            var catr = await _context.Category.FirstOrDefaultAsync(
                    c => c.CategoryId == form.CategoryId
                );
            var toFrontEnd = new GetFormForReferenceFormViewModel
            {
                FormId = form.FormId,
                UserId = form.UserId,
                CategoryId = form.CategoryId,
                Content = form.Content,
                CategoryDescription = catr.CategoryDescription
            };
            return Json(toFrontEnd);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> CreateNewForm([FromBody] NewFormViewModel model)
        {
            
            User user = await GetAuthorizedUser();
            
            //functionId:  01 -> 需求方申請人送出

            if (!user.PermittedTo("01"))
            {
                throw new Exception("User is not permitted");
            }
            DateTime ExpectedFinishedDayDateTimeUtc8 = DateTime.Parse(model.ExpectedFinishedDay);
            if(ExpectedFinishedDayDateTimeUtc8.Date < (DateTime.UtcNow).AddHours(8).Date)
            {
                string msg = "希望完成時間需晚於當天日期";
                return Json(new { errorCode = 400, message = msg });
            }
            if (!ModelState.IsValid)
            {
                string msg = "資料缺漏，無法新建工單";
                return Json(new { errorCode=400 , message = msg });
            }
            if(model.Content.Length > 10000)
            {
                string msg = "需求內容不得超過一萬字元";
                return Json(new { errorCode = 400, message = msg });
            }
            List<ProcessTemplate> pTemplates = await _context.ProcessTemplate.Where(p => p.CategoryId == model.CategoryId).ToListAsync();
            List<string> fIdList = await GetCreateFormIdListAsync(1);
            List<string> pNidList = await GetProcessNodeIdListAsync(pTemplates.Count);
            int ptIndex = 0;
            List<ProcessNode> processNodes = new List<ProcessNode>();
            foreach (var pt in pTemplates) {
                string dId = (pt.DepartmentId == "Requester" ? user.DepartmentId : pt.DepartmentId);
                List<User> DepartmentUser = await _context.User
                    .Where(u => u.DepartmentId == dId)
                    .Include(u => u.PermissionGroups)
                    .ThenInclude(pg => pg.UserActivities)
                    .ToListAsync();
                User owner = new User();
                bool noAvailableOwner = true;
                foreach (var u in DepartmentUser)
                {
                    if (u.PermittedTo(pt.UserActivityId))
                    {
                        owner = u;
                        noAvailableOwner = false;
                        break;
                    }
                }
                if (noAvailableOwner && pt.UserActivityId != "10")
                {
                    Department dep = (await _context.Department.FirstOrDefaultAsync(d => d.DepartmentId == pt.DepartmentId));
					UserActivity uat = (await _context.UserActivity.FirstOrDefaultAsync(u => u.UserActivityId== pt.UserActivityId));
					return Json(new { errorCode = 400, message = $"審核流程無法建立，因{dep.DepartmentName} 無人能執行功能 : {uat.UserActivityIdDescription}" });
                }
                if(pt.UserActivityId == "10")
                {
                    owner = user;
                }
                ProcessNode pn  = new ProcessNode();
                pn.ProcessNodeId = pNidList[ptIndex];
                pn.UserActivityId = pt.UserActivityId;
                pn.UserId = (pt.UserActivityId == "01" || pt.UserActivityId == "09")?user.UserId:owner.UserId;
                pn.DepartmentId = dId;
                pn.FormId = fIdList[0];
                pn.CreatedTime = DateTime.UtcNow;
                pn.UpdatedTime = DateTime.UtcNow;
                await  _context.ProcessNodes.AddAsync(pn);
                processNodes.Add(pn);
                ptIndex++;
            }
            Form newForm = new Form();
            newForm.FormId = fIdList[0];
            newForm.DepartmentId = model.DepartmentId;
            newForm.Date = DateTime.UtcNow;
            newForm.CategoryId = model.CategoryId;
            newForm.UserId = user.UserId;
            if(model.ProjectId != "")
            {
                newForm.ProjectId = model.ProjectId;
            }
            newForm.DepartmentId = model.DepartmentId;
            newForm.Content = model.Content;
            newForm.ExpectedFinishedDay = ExpectedFinishedDayDateTimeUtc8.AddHours(-8);
            newForm.Tel = model.TEL;
            newForm.ProcessNodeId = pNidList[1];
            newForm.FormIsActive = true;
            newForm.UpdatedTime = DateTime.UtcNow;
            newForm.CreatedTime = DateTime.UtcNow;
            await _context.Form.AddAsync(newForm);
            List<string> formRecordIdList = await GetCreateFormRecordIdListAsync(2);
            FormRecord createFormRecord = new FormRecord(), firstReviewFormRecord = new FormRecord();
            
            User createFormRecordUser = await _context.User.FirstOrDefaultAsync(u => u.UserId == processNodes[0].UserId);
            User firstReviewFormRecordUser = await _context.User.FirstOrDefaultAsync(u => u.UserId == processNodes[1].UserId);

            createFormRecord.ProcessingRecordId = formRecordIdList[0];
            createFormRecord.Remark = "";
            createFormRecord.FormId = processNodes[0].FormId;
            createFormRecord.DepartmentId = processNodes[0].DepartmentId;
            createFormRecord.UserId = processNodes[0].UserId;
            createFormRecord.ResultId = "RS2";
            createFormRecord.UserActivityId = processNodes[0].UserActivityId;
            createFormRecord.GradeId = createFormRecordUser.GradeId;
            createFormRecord.Date = DateTime.UtcNow;
            createFormRecord.UpdatedTime = DateTime.UtcNow;
            createFormRecord.CreatedTime = DateTime.UtcNow;

            firstReviewFormRecord.ProcessingRecordId = formRecordIdList[1];
            firstReviewFormRecord.Remark = "";
            firstReviewFormRecord.FormId = processNodes[1].FormId;
            firstReviewFormRecord.DepartmentId = processNodes[1].DepartmentId;
            firstReviewFormRecord.UserId = processNodes[1].UserId;
            firstReviewFormRecord.ResultId = "RS4";
            firstReviewFormRecord.UserActivityId = processNodes[1].UserActivityId;
            firstReviewFormRecord.GradeId = firstReviewFormRecordUser.GradeId;
            firstReviewFormRecord.Date = DateTime.UtcNow;
            firstReviewFormRecord.UpdatedTime = DateTime.UtcNow;
            firstReviewFormRecord.CreatedTime = DateTime.UtcNow;

            await _context.FormRecord.AddAsync(createFormRecord);
            await _context.FormRecord.AddAsync(firstReviewFormRecord);
            await _context.SaveChangesAsync();

            // 找出formrecord更新完的userId找出其資料
            var currentEmailEmp = await _context.FormRecord
                .Where(u => u.FormId == createFormRecord.FormId)
                .OrderByDescending(d => d.ProcessingRecordId)
                .Select(e => e.UserId)
                .FirstOrDefaultAsync();
            var recieveEmp = await _context.User.Where(u => u.UserId == currentEmailEmp).Select(c => c).FirstOrDefaultAsync();
            emailService.SendFormReviewEmail(recieveEmp, createFormRecord.FormId);

            return Json(new { errorCode = 200, message = $"新增工單成功! 單號 : {newForm.FormId}" , formId = newForm.FormId});
        }

        
    }
}
