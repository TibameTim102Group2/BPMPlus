using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BPMPlus.Data;
using BPMPlus.Models;
using Microsoft.AspNetCore.Authorization;
using BPMPlus.ViewModels;

namespace BPMPlus.Controllers
{
    public class CreateProjectsController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public CreateProjectsController(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        [Authorize]

        // GET: CreateProjects
        public async Task<IActionResult> Index()
        {
            User user = await GetAuthorizedUser();
            //專案管理員才可以新建專案
            var Permitted = false;
            foreach (var item in user.PermissionGroups)
            {
                if (item.PermissionGroupId == "G0010")
                {
                    Permitted = true; break;
                }

            }
            if (!Permitted)
            {
                ViewBag.NotPermittedToCreateForm = "您的權限無法新建專案";
                return View("~/Views/Home/Index.cshtml");
            }

            var projectdata = _context.User
                        .Include(c => c.Department)
                        .Where(c => c.UserId == user.UserId)
                        .Select(c => new CreateProjectsViewModel
                        {
                            UserId=c.UserId,
                            UserName=c.UserName,
                            DepartmentName=c.Department.DepartmentName
                        }).FirstOrDefault();
            
            
            return View(projectdata ?? new CreateProjectsViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProjectsInPutModel project)
        {
            User user = await GetAuthorizedUser();

            try 
            {

                List<string> pIdList = await CreateProjectIdListAsync(1);

                Project insert = new Project()
                {
                    ProjectId = pIdList[0],
                    ProjectName = project.ProjectName,
                    ProjectManagerId=user.UserId,
                    Summary = project.Summary,
                    DeadLine = project.DeadLine,
                    CreatedTime = DateTime.UtcNow,
                    UpdatedTime = DateTime.UtcNow,
                };

                await _context.Project.AddAsync(insert);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "成功" });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "失敗" });
            }
            

           
        }

    }
}
