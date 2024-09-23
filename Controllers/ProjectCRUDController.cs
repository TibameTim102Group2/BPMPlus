using BPMPlus.Data;
using BPMPlus.Models;
using BPMPlus.ViewModels.Project;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Graph;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BPMPlus.Controllers
{

    public class ProjectCRUDController : BaseController
    {
        private readonly ApplicationDbContext _context;
        public ProjectCRUDController(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        
        [HttpPost]
        [Authorize]
        public async Task<JsonResult> CheckModify([FromBody] ModifyProjectViewModels model)
        {
            User user = await GetAuthorizedUser();
            if (!user.PermittedTo("11"))
            {
                return Json(new { errorCode = 400, message = "您的權限無法檢視及修改專案細節" });
            }
            Project project = await _context.Project
                .Include(p => p.Users)
                .FirstOrDefaultAsync(p => p.ProjectId == model.ProjectId);
            if (project == null) {
                return Json(new { errorCode = 400, message = "修改異常，無此專案" });
            }
            var xLine = DateTime.Parse(model.Deadline);
            if (xLine.Date < DateTime.UtcNow.AddHours(8).Date)
            {
                return Json(new { errorCode = 400, message = "專案截止日期不可以早於今日" });
            }
            if (model.ProjectName == "")
            {
                return Json(new { errorCode = 400, message = "專案名稱不可為空" });
            }
            if (model.ProjectDescription == "")
            {
                return Json(new { errorCode = 400, message = "專案描述不可為空" });
            }
            if (model.ProjectName.Length > 100)
            {
                return Json(new { errorCode = 400, message = "專案名稱長度不可超過100字元" });
            }
            if (model.ProjectDescription.Length > 1000)
            {
                return Json(new { errorCode = 400, message = "專案概述不可超過1000字元" });
            }
            if (model.UserIds.Count == 0)
            {
                return Json(new { errorCode = 400, message = "專案成員至少需要一位" });
            }
            if (!model.UserIds.Contains(project.ProjectManagerId))
            {
                return Json(new { errorCode = 400, message = "專案經理不可移除" });
            }

            string retString = "";

            List<User> updatedUserList = await _context.User.Where(u => model.UserIds.Contains(u.UserId)).ToListAsync();
            List<User> addedPeople = updatedUserList.Except(project.Users).ToList();
            List<User> removedPeople = project.Users.Except(updatedUserList).ToList();

            if (!(project.ProjectManagerId == user.UserId) && (addedPeople.Count != 0 || removedPeople.Count != 0))
            {
                return Json(new { errorCode = 400, message = "非專案經理無法增刪人員" });
            }

            return Json(new { errorCode = 200, message = "CheckOK"});
        }

        [HttpGet("ProjectCRUD/ProjectDetails/{projectId}")]
        [Authorize]
        public async Task<ActionResult> ProjectDetails(string ProjectId)
        {
            User user = await GetAuthorizedUser();
            //functionId:  01 -> 需求方申請人送出
            if (!user.PermittedTo("11"))
            {
                ViewBag.NotPermittedToCreateForm = "您的權限無法檢視專案細節";
                return View("~/Views/Home/Index.cshtml");
            }
            
            var Project= await _context.Project
                .Include(x => x.Users)
                .ThenInclude(u => u.Grade)
                .Include(x => x.Users)
                .ThenInclude(u => u.Department)
                .FirstOrDefaultAsync(d => d.ProjectId == ProjectId);            
            if (Project == null) {
				ViewBag.NotPermittedToCreateForm = "專案編號無效";
				return View("~/Views/Home/Index.cshtml");
			}
            if(!Project.Users.Contains(user))
            {
                ViewBag.NotPermittedToCreateForm = "您不屬於這個專案";
                return View("~/Views/Home/Index.cshtml");
            }
            string allowModify = "false";
            if(user.UserId == Project.ProjectManagerId)
            {
                allowModify = "true";
            }
            var Forms = await _context.Form
                .Where(x => x.ProjectId == Project.ProjectId)
                .Include(f => f.User)
                .Include(f => f.Category)
                .Include(f => f.FormRecord)
                .Include(f => f.ProcessNode)
                .ThenInclude(p => p.Department)
                .Include(f => f.ProcessNode)
                .ThenInclude(p => p.UserActivity)
                .Join(_context.ProcessNodes,               // 第二個表 ProcessNode
                    f => f.ProcessNodeId,          // Form 表的聯接鍵
                    pN => pN.ProcessNodeId, // ProcessNode 表的聯接鍵
                    (f, pN) => new           // 聯接結果的選擇器
                    {
                        Form = f,
                        PN = pN
                })
                .ToListAsync();

            ViewBag.ProjectId = ProjectId;
            ViewBag.ProjectName = Project.ProjectName;
            ViewBag.Summary = Project.Summary;
            ViewBag.DeadLine = (Project.DeadLine.Date).ToString("yyyy.MM.dd");
            ViewBag.ModMode = false;
            List<ProjectUsersViewModels> projectUsersViewModels = new List<ProjectUsersViewModels>();
            foreach(var u in Project.Users)
            {
                if(u.UserId == Project.ProjectManagerId)
                {
                    projectUsersViewModels.Insert(0, new ProjectUsersViewModels(u.UserName, u.UserId, u.Department.DepartmentName, u.Grade.GradeName, "專案經理"));
                }
                if(u.UserId != Project.ProjectManagerId)
                    projectUsersViewModels.Add(new ProjectUsersViewModels(u.UserName, u.UserId, u.Department.DepartmentName, u.Grade.GradeName, "組員"));
            }
            List<ProjectFormsViewModels> projectFormsViewModels = new List<ProjectFormsViewModels>();
            List<GanttData> FormGanttList = new List<GanttData>();
            List<int> formIndexList = new List<int>();
            List<int> formNodeCountList = new List<int>();
            foreach(var form in Forms)
            {
                projectFormsViewModels.Add(new ProjectFormsViewModels(
                    form.Form.FormId, 
                    form.Form.Department.DepartmentName, 
                    form.Form.UserId,
                    form.Form.User.UserName, 
                    form.Form.Category.CategoryDescription,
                    (form.PN.UserActivity.UserActivityIdDescription)
                ));
                
                int formNodeCount = form.Form.ProcessNode.Count()-1;
                int index = 0;
                var pnList = form.Form.ProcessNode.OrderBy(p => p.ProcessNodeId);
                foreach (var node in pnList)
                {
                    if(node.ProcessNodeId == form.Form.ProcessNodeId)
                    {
                        break;
                    }
                    index++;
                }
                int formProgress = (100 / formNodeCount) * index;

                FormGanttList.Add(
                    new GanttData(
                        form.Form.FormId,
                        form.Form.Category.CategoryDescription,
                        form.Form.Date.AddHours(8).Date.ToString("yyyy-MM-dd"),
                        form.Form.ExpectedFinishedDay.AddHours(8).Date.ToString("yyyy-MM-dd"),
                        formProgress,
                        null
                    )
                );
                formIndexList.Add( index );
                formNodeCountList.Add( formNodeCount );
            }
            string formGantListStr = "";
            foreach(var node in FormGanttList)
            {
                formGantListStr += node.Id+", ";
            }
            var prg = 0;
            if(!(formNodeCountList.Sum() == 0)) prg = (formIndexList.Sum() * (100/formNodeCountList.Sum()));
            ProjectChartViewModel projectChartViewModel = new ProjectChartViewModel(
                new List<GanttData>() {
                    new GanttData(
                        Project.ProjectId,
                        Project.ProjectName,
                        Project.CreatedTime.AddHours(8).Date.ToString("yyyy-MM-dd"),
                        Project.DeadLine.AddHours(8).Date.ToString("yyyy-MM-dd"),
                        prg,
                        null
                    )
                },
                FormGanttList
            );
            return View(
                new ProjectDetailsViewModel(
                    allowModify,
                    projectUsersViewModels,
                    projectFormsViewModels,
                    projectChartViewModel
                )                
            );
        }
        [HttpGet("ProjectCRUD/ModifyProjectDetails/{projectId}")]
        [Authorize]
        public async Task<ActionResult> ModifyProjectDetails(string ProjectId)
        {
            User user = await GetAuthorizedUser();
            //functionId:  01 -> 需求方申請人送出
            if (!user.PermittedTo("11"))
            {
                ViewBag.NotPermittedToCreateForm = "您的權限無法檢視專案細節";
                return View("~/Views/Home/Index.cshtml");
            }

            var Project = await _context.Project
                .Include(x => x.Users)
                .ThenInclude(u => u.Grade)
                .Include(x => x.Users)
                .ThenInclude(u => u.Department)
                .FirstOrDefaultAsync(d => d.ProjectId == ProjectId);
            if (Project == null)
            {
                ViewBag.NotPermittedToCreateForm = "專案編號無效";
                return View("~/Views/Home/Index.cshtml");
            }
            if (!Project.Users.Contains(user))
            {
                ViewBag.NotPermittedToCreateForm = "您不屬於這個專案";
                return View("~/Views/Home/Index.cshtml");
            }
            string allowModify = "false";
            if (user.UserId == Project.ProjectManagerId)
            {
                allowModify = "true";
            }
            var Forms = await _context.Form
                .Where(x => x.ProjectId == Project.ProjectId)
                .Include(f => f.User)
                .Include(f => f.Category)
                .Include(f => f.FormRecord)
                .Include(f => f.ProcessNode)
                .ThenInclude(p => p.Department)
                .Include(f => f.ProcessNode)
                .ThenInclude(p => p.UserActivity)
                .Join(_context.ProcessNodes,               // 第二個表 ProcessNode
                    f => f.ProcessNodeId,          // Form 表的聯接鍵
                    pN => pN.ProcessNodeId, // ProcessNode 表的聯接鍵
                    (f, pN) => new           // 聯接結果的選擇器
                    {
                        Form = f,
                        PN = pN
                    })
                .ToListAsync();


            var AllUsers = await _context.User
            .Include(u => u.Grade)
            .Include(u => u.Department)
            .Select(u => new { u.UserId, u.UserName, u.Department, u.Grade })
            .ToListAsync();
            List<RenderUser> uList = new List<RenderUser>();
            foreach(var u in AllUsers)
            {
                uList.Add(
                    new RenderUser(
                        u.UserId,
                        u.UserName,
                        new DepartmentPart(
                            u.Department.DepartmentId,
                            u.Department.DepartmentName
                        ),
                        new GradePart(
                            u.Grade.GradeId,
                            u.Grade.GradeName
                        )
                        
                    )
                );
            }
            var AllForms = await _context.Form
                .Include(f => f.Department)
                .Include(f => f.User)
                .Include(f => f.Category)
                .Include(f => f.ProcessNode)
                .Join(_context.ProcessNodes,               // 第二個表 ProcessNode
                    f => f.ProcessNodeId,          // Form 表的聯接鍵
                    pN => pN.ProcessNodeId, // ProcessNode 表的聯接鍵
                    (f, pN) => new           // 聯接結果的選擇器
                    {
                        Form = f,
                        PN = pN
                    })
                .Select(f => new { f.Form.FormId, f.Form.Department.DepartmentName, f.Form.User.UserId, f.Form.User.UserName, f.Form.Category.CategoryDescription, f.PN.UserActivity.UserActivityIdDescription})
                .ToListAsync();
            List<RenderForm> fList = new List<RenderForm>();
            foreach (var f in AllForms)
            {
                fList.Add(
                    new RenderForm(
                        f.FormId,
                        f.DepartmentName,
                        f.UserId,
                        f.UserName,
                        f.CategoryDescription,
                        f.UserActivityIdDescription
                    )
                );
            }
            ViewBag.ProjectId = ProjectId;
            ViewBag.ProjectName = Project.ProjectName;
            ViewBag.Summary = Project.Summary;
            ViewBag.DeadLine = Project.DeadLine.AddHours(8);
            ViewBag.MinDate = DateTime.Now.AddHours(8);
            ViewBag.ModMode = true;

            ViewBag.AllUserList = JsonSerializer.Serialize(uList, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve });
            ViewBag.AllFormList = JsonSerializer.Serialize(fList, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve });

            List<ProjectUsersViewModels> projectUsersViewModels = new List<ProjectUsersViewModels>();
            foreach (var u in Project.Users)
            {
                if (u.UserId == Project.ProjectManagerId)
                {
                    projectUsersViewModels.Insert(0, new ProjectUsersViewModels(u.UserName, u.UserId, u.Department.DepartmentName, u.Grade.GradeName, "專案經理"));
                }
                if (u.UserId != Project.ProjectManagerId)
                    projectUsersViewModels.Add(new ProjectUsersViewModels(u.UserName, u.UserId, u.Department.DepartmentName, u.Grade.GradeName, "組員"));
            }
            List<ProjectFormsViewModels> projectFormsViewModels = new List<ProjectFormsViewModels>();
            List<GanttData> FormGanttList = new List<GanttData>();
            List<int> formIndexList = new List<int>();
            List<int> formNodeCountList = new List<int>();
            foreach (var form in Forms)
            {
                projectFormsViewModels.Add(new ProjectFormsViewModels(
                    form.Form.FormId,
                    form.Form.Department.DepartmentName,
                    form.Form.UserId,
                    form.Form.User.UserName,
                    form.Form.Category.CategoryDescription,
                    (form.PN.UserActivity.UserActivityIdDescription)
                ));

                int formNodeCount = form.Form.ProcessNode.Count() - 1;
                int index = 0;
                var pnList = form.Form.ProcessNode.OrderBy(p => p.ProcessNodeId);
                foreach (var node in pnList)
                {
                    if (node.ProcessNodeId == form.Form.ProcessNodeId)
                    {
                        break;
                    }
                    index++;
                }
                int formProgress = (100 / formNodeCount) * index;

                FormGanttList.Add(
                    new GanttData(
                        form.Form.FormId,
                        form.Form.Category.CategoryDescription,
                        form.Form.Date.AddHours(8).Date.ToString("yyyy-MM-dd"),
                        form.Form.ExpectedFinishedDay.AddHours(8).Date.ToString("yyyy-MM-dd"),
                        formProgress,
                        null
                    )
                );
                formIndexList.Add(index);
                formNodeCountList.Add(formNodeCount);
            }
            string formGantListStr = "";
            foreach (var node in FormGanttList)
            {
                formGantListStr += node.Id + ", ";
            }
            var prg = 0;
            if (!(formNodeCountList.Sum() == 0)) prg = (formIndexList.Sum() * (100 / formNodeCountList.Sum()));
            ProjectChartViewModel projectChartViewModel = new ProjectChartViewModel(
                new List<GanttData>() {
                    new GanttData(
                        Project.ProjectId,
                        Project.ProjectName,
                        Project.CreatedTime.AddHours(8).Date.ToString("yyyy-MM-dd"),
                        Project.DeadLine.AddHours(8).Date.ToString("yyyy-MM-dd"),
                        prg,
                        null
                    )
                },
                FormGanttList
            );
            return View(
                new ProjectDetailsViewModel(
                    allowModify,
                    projectUsersViewModels,
                    projectFormsViewModels,
                    projectChartViewModel
                )
            );
        }
    }
}
