using BPMPlus.Data;
using BPMPlus.Models;
using BPMPlus.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
                    projectUsersViewModels.Insert(0, new ProjectUsersViewModels(u.UserName, u.UserId, u.Department.DepartmentName, u.Grade.GradeName, "組長"));
                }
                if(u.UserId != Project.ProjectManagerId)
                    projectUsersViewModels.Add(new ProjectUsersViewModels(u.UserName, u.UserId, u.Department.DepartmentName, u.Grade.GradeName, "組員"));
            }
            List<ProjectFormsViewModels> projectFormsViewModels = new List<ProjectFormsViewModels>();
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
            }
            ProjectChartViewModel projectChartViewModel = new ProjectChartViewModel();  
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
