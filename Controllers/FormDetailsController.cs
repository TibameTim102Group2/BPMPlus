using BPMPlus.Data;
using BPMPlus.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BPMPlus.Controllers
{
    
    public class FormDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FormDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: FormDetails
        //public IActionResult Index()
        //{
        //    return View();
        //}


        //[HttpPost]
        // GET: FormDetails
        //public IActionResult Index(string id)
        //{
        //    var result = _context.Form.Include(c => c.ProcessNode).ThenInclude(c => c.UserActivity).Include(c => c.FormRecord).
        //        Include(c => c.Project).Include(c => c.Category).Include(c => c.Department).Include(c => c.User)
        //        .Where(c => c.FormId == id)
        //        .Select(c => new FormDetailsViewModel
        //        {
        //            FormId = c.FormId,
        //            ProjectName = c.Project.ProjectName,
        //            CategoryDescription = c.Category.CategoryDescription,
        //            Tel = c.Tel,
        //            ExpectedFinishedDay = c.ExpectedFinishedDay,
        //            Content = c.Content,
        //            CreatedTime = c.CreatedTime,
        //            DepartmentName = c.Department.DepartmentName,
        //            EmployeeName = c.User.UserName,
        //            FormDetailsProcessNodes = c.ProcessNode.Select(d => new FormDetailsProcessNodeViewModel
        //            {
        //                UserActivityIdDescription = d.UserActivity.UserActivityIdDescription,
        //                IsHightLight = true,
        //            }).ToList(),
        //            FormDetailsFormProcesses = c.FormRecord.Select(d => new FormDetailsFormProcessViewModel
        //            {
        //                CreatedTime = d.CreatedTime,
        //                UserActivityIdDescription = d.UserActivity.UserActivityIdDescription,
        //                EmployeeName = d.User.UserName,
        //                Remark = d.Remark,
        //                ResultDescription = d.Result.ResultDescription
        //            }).ToList()
        //        }).FirstOrDefault();

        //    return View(result);
        //}


        // GET: FormDetails
        public IActionResult Index()
        {
            return View(_context.Form.FirstOrDefault());
        }


    }
}
