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
            ViewBag.ProjectId = ProjectId;
            ViewBag.ProjectName = Project.ProjectName;
            ViewBag.Summary = Project.Summary;
            ViewBag.DeadLine = (Project.DeadLine.Date).ToString("yyyy.MM.dd");

            List<ProjectUsersViewModels> projectUsersViewModels = new List<ProjectUsersViewModels>()
            {
                new ProjectUsersViewModels("王曉明", "A001", "人資部", "工程師", "V")
            };
            List<ProjectFormsViewModels> projectFormsViewModels = new List<ProjectFormsViewModels>()
            {
                new ProjectFormsViewModels("F00001", "人資部", "A001", "王曉明", "資訊需求單", "申請人送出")
            };


            var tupleModel = new Tuple<List<ProjectUsersViewModels>, List<ProjectFormsViewModels>>(projectUsersViewModels, projectFormsViewModels);
            return View(tupleModel);
        }
    }
}
