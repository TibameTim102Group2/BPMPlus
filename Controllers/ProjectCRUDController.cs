using BPMPlus.Data;
using BPMPlus.Models;
using BPMPlus.ViewModels.Project;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Graph;
using Microsoft.EntityFrameworkCore;

namespace BPMPlus.Controllers
{

    public class ProjectCRUDController : BaseController
    {
        private readonly ApplicationDbContext _context;
        public ProjectCRUDController(ApplicationDbContext context):base(context)
        {
            _context = context;
        }
        // GET: ProjectCRUDController

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
                    projectUsersViewModels,
                    projectFormsViewModels,
                    projectChartViewModel
                )                
            );
        }
    }
}
