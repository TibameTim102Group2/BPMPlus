using BPMPlus.Data;
using BPMPlus.Models;
using BPMPlus.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BPMPlus.Controllers
{
    public class QueryProjectsController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public QueryProjectsController(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

		// GET: QueryProjectsController
		[Authorize]
		public async Task<IActionResult> Index()
        {
            //Project表格資訊
            var tableData = await _context.Project
                 .AsNoTracking()
                 .AsSplitQuery()
                 .Select(c => new QueryProjectsProjectContentViewModel
                 {
                     ProjectId = c.ProjectId,
                     ProjectName = c.ProjectName,
                     Summary = c.Summary,
                     DeadLine = c.DeadLine.ToString("yyyy-MM-dd"),
                     ProjectManager = _context.User.FirstOrDefault(n => n.UserId == c.ProjectManagerId).UserName
                 }).ToListAsync();


            //Input篩選項目
            var projectData = await _context.Project
                .AsNoTracking()
                .AsSplitQuery()
                .Select(c => new QueryProjectsSearchInputViewModel
                {
                    ProjectId = c.ProjectId,
                    ProjectName = c.ProjectName
                }).FirstOrDefaultAsync();

            //SelectDepartment選單
            var departmentList = await _context.User
                .Include(c => c.Department)
                .AsNoTracking()
                .AsSplitQuery()
                .Select(c => new QueryProjectsSearchDepartmentViewModel
                {
                    DepartmentName = c.Department.DepartmentName,
                    DepartmentId=c.DepartmentId
                })
                .Distinct()
                .ToListAsync();

            //SelectEmployee選單
            var employeeList = await _context.User
                .AsNoTracking()
                .AsSplitQuery()
                .Select(c => new QueryProjectsSearchEmployeeViewModel
                {
                   EmployeeId=c.UserId,
                   EmployeeName=c.UserName
                })
                .Distinct()
                .ToListAsync();



            var result = new QueryProjectsViewModel
            {
                QueryProjectsProjectContents = tableData,
                QueryProjectsSearchInput = projectData,
                QueryProjectsSearchDepartments = departmentList,
                QueryProjectsSearchEmployees= employeeList
            };


            return View(result);
        }

        //GET: QueryProjects/IndexJson
        public async Task<JsonResult> IndexJson() 
        {

            //Project表格資訊
            var tableData = await _context.Project
                 .AsNoTracking()
                 .AsSplitQuery()
                 .Select(c => new QueryProjectsProjectContentViewModel
                 {
                     ProjectId = c.ProjectId,
                     ProjectName = c.ProjectName,
                     Summary = c.Summary,
                     DeadLine = c.DeadLine.ToString("yyyy-MM-dd"),
                     ProjectManager = _context.User.FirstOrDefault(n => n.UserId == c.ProjectManagerId).UserName
                 }).ToListAsync();




            return Json(tableData);
        }

		[Authorize]
		[HttpPost]
        public async Task<JsonResult> Filter([FromBody]  QueryProjectInputModel data)
        {

            //Project表格資訊
            var tableData = _context.Project
                 .Include(c=>c.Users)
                 .ThenInclude(c=>c.Department)
                 .AsNoTracking()
                 .AsSplitQuery();

            if (data != null) 
            {
                if (!string.IsNullOrEmpty(data.ProjectId))
                {
                    tableData = tableData.Where(c => c.ProjectId.Contains(data.ProjectId));
                }

                if (!string.IsNullOrEmpty(data.ProjectName))
                {
                    tableData = tableData.Where(c => c.ProjectName.Contains(data.ProjectName));
                }

                //if (!(string.IsNullOrEmpty(data.EmployeeDepartmentName) || data.EmployeeDepartmentName== "請選擇部門"))
                //{
                //    var projectIds = await _context.Department
                //        .Include(c => c.Users)
                //        .Where(c => c.DepartmentId == data.EmployeeDepartmentName)
                //        .SelectMany(c => c.Users)
                //        .SelectMany(c => c.Projects)
                //        .Select(c => c.ProjectId)
                //        .ToListAsync();
                //    tableData = tableData.Where(c => projectIds.Contains(c.ProjectId));
                //}

                if (!string.IsNullOrEmpty(data.EmployeeId) && data.EmployeeId != "--請選擇員工--")
                {
                    var projectIds = await _context.User
                        .Where(c => c.UserId == data.EmployeeId)
                        .SelectMany(c => c.Projects)
                        .Select(c => c.ProjectId)
                        .ToListAsync();
                    tableData = tableData.Where(c => projectIds.Contains(c.ProjectId));
                }

            }

            var users = _context.User.ToList();

            var result = tableData.Select(c => new QueryProjectsProjectContentViewModel
            {
                ProjectId = c.ProjectId,
                ProjectName = c.ProjectName,
                Summary = c.Summary,
                DeadLine = c.DeadLine.ToString("yyyy-MM-dd"),
                ProjectManager = _context.User.FirstOrDefault(n => n.UserId == c.ProjectManagerId).UserName /*users.FirstOrDefault(d => d.UserId == c.ProjectManagerId).UserName*/

            }).ToList();
     
            return Json(new {success=true,data=result});
        }
    }
}
