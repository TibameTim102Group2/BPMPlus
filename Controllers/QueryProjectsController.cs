using BPMPlus.Data;
using BPMPlus.ViewModels;
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
                     DeadLine = c.DeadLine,
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

            //Select選單
            var userList = await _context.User
                .Include(c => c.Projects)
                .Include(c => c.Department)
                .AsNoTracking()
                .AsSplitQuery()
                .Select(c => new QueryProjectsSearchListViewModel
                {
                    EmployeeId = c.UserId,
                    EmployeeName = c.UserName,
                    DepartmentName = c.Department.DepartmentName
                }).FirstOrDefaultAsync();

            //selectMany

            var result = new QueryProjectsViewModel
            {
                QueryProjectsProjectContents = tableData,
                QueryProjectsSearchInput = projectData,
                QueryProjectsSearchList = new QueryProjectsSearchListViewModel()
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
                     DeadLine = c.DeadLine,
                     ProjectManager = _context.User.FirstOrDefault(n => n.UserId == c.ProjectManagerId).UserName
                 }).ToListAsync();




            return Json(tableData);
        }

        [HttpPost]
        public async Task<JsonResult> Filter(string projectId, string projectName, string employeeDepaetmentName, string employeeName, string employeeId)
        {

            //Project表格資訊
            var tableData = _context.Project
                 .Include(c=>c.Users)
                 .ThenInclude(c=>c.Department)
                 .AsNoTracking()
                 .AsSplitQuery();

            if (!string.IsNullOrEmpty(projectId))
            {
                tableData = tableData.Where(c => c.ProjectId.Contains(projectId));
            }

            if (!string.IsNullOrEmpty(projectName))
            {
                tableData = tableData.Where(c => c.ProjectName.Contains(projectName));
            }

            if (!string.IsNullOrEmpty(employeeDepaetmentName))
            {
                var projectIds = await _context.Department
                    .Include(c => c.Users)
                    .Where(c => c.DepartmentName == employeeDepaetmentName)
                    .SelectMany(c => c.Users)
                    .SelectMany(c => c.Projects)
                    .Select(c => c.ProjectId)
                    .ToListAsync();
                tableData = tableData.Where(c => projectIds.Contains(c.ProjectId));
            }

            if (!string.IsNullOrEmpty(employeeId))
            {
                var projectIds = await _context.User
                    .Where(c => c.UserId == employeeId)
                    .SelectMany(c => c.Projects)
                    .Select(c => c.ProjectId)
                    .ToListAsync();
                tableData = tableData.Where(c => projectIds.Contains(c.ProjectId));
            }

            var users = _context.User.ToList();

            var result = tableData.Select(c => new QueryProjectsProjectContentViewModel
            {
                ProjectId = c.ProjectId,
                ProjectName = c.ProjectName,
                Summary = c.Summary,
                DeadLine = c.DeadLine,
                ProjectManager = users.FirstOrDefault(d => d.UserId == c.ProjectManagerId).UserName
            }).ToList();

            return Json(result);
        }
    }
}
