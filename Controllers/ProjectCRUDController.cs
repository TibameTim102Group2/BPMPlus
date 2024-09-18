using BPMPlus.Data;
using BPMPlus.Models;
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
            var g = user.PermittedTo("11");
            if (!user.PermittedTo("11"))
            {
                ViewBag.NotPermittedToCreateForm = "您的權限無法檢視專案細節";
                return View("~/Views/Home/Index.cshtml");
            }

            var Project= await _context.Project
                 .FirstOrDefaultAsync(d => d.ProjectId == ProjectId);
            
            return View();
        }
    }
}
