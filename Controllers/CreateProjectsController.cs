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
using static System.Runtime.InteropServices.JavaScript.JSType;

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
		[Authorize]
		public async Task<IActionResult> Create(CreateProjectsInPutModel project)
        {
            User user = await GetAuthorizedUser();

            try 
            {

                if (project.ProjectName == null)
                {
                    return Json(new { success = false, message = "專案名稱不可為空" });
                }

                if (project.DeadLine == null || project.DeadLine < (DateTime.UtcNow.AddDays(-1)))
                {
                    return Json(new { success = false, message = "專案期限不可為空或早於今日" });
                }

                if (project.Summary == null)
                {
                    return Json(new { success = false, message = "專案概要不可為空" });
                }

                if (project.Summary.Length > 300)
                {
                    return Json(new { success = false, message = "專案概要不可超過300字元" });
                }


                List<string> pIdList = await CreateProjectIdListAsync(1);

                //var addUser = await _context.Project
                //    .Include(c => c.Users)
                //    .SelectMany(c=>c.Users)
                //    .FirstOrDefaultAsync(c => c.UserId == user.UserId);


               

                Project insert = new Project()
                {
                    ProjectId = pIdList[0],
                    ProjectName = project.ProjectName,
                    ProjectManagerId=user.UserId,
                    Summary = project.Summary,
                    DeadLine = project.DeadLine,
                    CreatedTime = DateTime.UtcNow,
                    UpdatedTime = DateTime.UtcNow,
                    Users = new List<User>() { user }
                };
                


                await _context.Project.AddAsync(insert);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "送出成功" });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "送出失敗" });
            }
            

           
        }

    }
}
