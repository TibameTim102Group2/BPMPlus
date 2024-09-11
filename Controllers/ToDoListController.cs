using BPMPlus.Data;
using BPMPlus.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BPMPlus.Controllers
{
    public class ToDoListController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public ToDoListController(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        [Authorize]
        // GET: ToDoList
        public async Task<IActionResult> Index()
        {
            User user = await GetAuthorizedUser();

            //篩資料的條件 工單的流程節點編號的責成人員 是此時使用者登入的人員 且他的功能編號不為1&10的

            var applicationDbContext = _context.Form.Include(f => f.Category)
                .Include(f => f.ProcessNode).Where(f => f.ProcessNode.Any(pn => pn.ProcessNodeId == f.ProcessNodeId && pn.UserId == user.UserId && pn.UserActivityId != "01" && pn.UserActivityId != "10" && f.FormIsActive == true));

            //把部門變數導入

            var departments = await _context.Department.ToDictionaryAsync(d => d.DepartmentId, d => d.DepartmentName);
            ViewBag.Departments = departments;
            //把使用者變數導入
            var userName = await _context.User.ToDictionaryAsync(d => d.UserId, d => d.UserName);
            ViewBag.UserName = userName;

            var Category = await _context.Category.ToDictionaryAsync(d => d.CategoryId, d => d.CategoryDescription);
            ViewBag.category = Category;

            var situation = await _context.ProcessNodes.ToDictionaryAsync(d => d.ProcessNodeId, d => d.UserActivityId);
            ViewBag.process = situation;

            var userActivity = await _context.UserActivity.ToDictionaryAsync(d => d.UserActivityId, d => d.UserActivityIdDescription);
            ViewBag.situation = userActivity;



            return View(applicationDbContext);
        }


    }
}