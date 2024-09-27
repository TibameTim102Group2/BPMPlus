using BPMPlus.Data;
using BPMPlus.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using NuGet.Packaging.Signing;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BPMPlus.Controllers
{
    public class QueryFormsController : BaseController

    {
        private readonly ApplicationDbContext _context;
        public QueryFormsController(ApplicationDbContext context) : base(context)
        {
            _context = context;

        }
        //進入主頁撈資料
        // GET: QueryForms
        [Authorize]
        public async Task<ActionResult> Index()
        {
            User user = await GetAuthorizedUser();

            //加入撈取資料的判斷條件(必須要和登入者同部門，且還在活動的工單)
            var alllist = _context.Form.AsNoTracking().Where(f => f.DepartmentId == user.DepartmentId && f.FormIsActive == true);

            //把類別變成選項
            ViewBag.CategoryId = new SelectList(_context.Category, "CategoryId", "CategoryDescription");
            //把專案變成選項
            ViewBag.ProjectId = new SelectList(_context.Project, "ProjectId", "ProjectName") ;
            
            //把申請者變成選項
            ViewBag.UserId = new SelectList(_context.User.AsNoTracking()
                .Where(f => f.DepartmentId == user.DepartmentId)
                .Select(f => new {
                    UserId = f.UserId,
                    DisplayText = f.UserId + " - " + f.UserName
                }),
                "UserId",
                "DisplayText");
            var applicationDbContext = alllist.Include(f => f.Category).Include(f => f.ProcessNode);
            //把部門變數導入
            var departments = await _context.Department.AsNoTracking().ToDictionaryAsync(d => d.DepartmentId, d => d.DepartmentName);
            ViewBag.Departments = departments;
            //把申請者變數導入
            var userName = await _context.User.AsNoTracking().ToDictionaryAsync(d => d.UserId, d => d.UserName);
            ViewBag.UserName = userName;
            //把專案變數導入
            
            var projectName = await _context.Project.AsNoTracking().ToDictionaryAsync(d => d.ProjectId, d => d.ProjectName)??null;
            ViewBag.ProjectName = projectName;
            //把類別變數導入

            var Category = await _context.Category.AsNoTracking().ToDictionaryAsync(d => d.CategoryId, d => d.CategoryDescription);
            ViewBag.category = Category;
            //把工單狀態變數導入
            var situation = await _context.ProcessNodes.AsNoTracking().ToDictionaryAsync(d => d.ProcessNodeId, d => d.UserActivityId);
            ViewBag.process = situation;

            var userActivity = await _context.UserActivity.AsNoTracking().ToDictionaryAsync(d => d.UserActivityId, d => d.UserActivityIdDescription);
            ViewBag.situation = userActivity;





            return View(applicationDbContext);
        }

        //依照工單id去找
        //GET: QueryForms/SearchByFormId
        public async Task<IActionResult> SearchByFormId(string formId)
        {

            //判斷登入者是誰
            User user = await GetAuthorizedUser();
            //加入撈取資料的判斷條件(必須要和登入者同部門，且還在活動的工單)
            var alllist = _context.Form.AsNoTracking().Where(f => f.DepartmentId == user.DepartmentId && f.FormIsActive == true);
            var findForm = await alllist.Include(c => c.Category).Include(c => c.Project).FirstOrDefaultAsync(f => f.FormId == formId);
            if (formId == null || findForm == null)
            {
                return Json(new { success = false, message = "查無此資料請重新輸入" });
            }
            //把時間轉換
            var creatTime = findForm.Date.ToLocalTime().ToString("yyyy-MM-dd HH:mm");
            //找出工單的流程節點 並且回傳功能的id
            var UserActivity = (await _context.ProcessNodes.AsNoTracking().FirstOrDefaultAsync(c => c.ProcessNodeId == findForm.ProcessNodeId))?.UserActivityId;

            var result = new
            {
                findForm.FormId,
                findForm.Category.CategoryDescription,
                DepartmentName = (await _context.Department.AsNoTracking().FirstOrDefaultAsync(c => c.DepartmentId == findForm.DepartmentId))?.DepartmentName,
                UserName = (await _context.User.AsNoTracking().FirstOrDefaultAsync(c => c.UserId == findForm.UserId))?.UserName,
                Situation = (await _context.UserActivity.AsNoTracking().FirstOrDefaultAsync(c => c.UserActivityId == UserActivity))?.UserActivityIdDescription,
                CreatedTime = creatTime,
                ProjectName = findForm.Project != null ? findForm.Project.ProjectName : "",
            };
            return Json(new { success = true, data = result });


        }

        //依照申請者的id去找
        //Get :QueryForms/UserName
        public async Task<IActionResult> UserName(string userId)
        {
            User user = await GetAuthorizedUser();
            //加入撈取資料的判斷條件(必須要和登入者同部門，且還在活動的工單)
            var alllist = _context.Form.AsNoTracking().Where(f => f.DepartmentId == user.DepartmentId && f.FormIsActive == true);
            var findForm = await alllist.Include(c => c.Category)
                            .Include(c => c.Project)
                            .Where(f => f.UserId == userId)
                            .Select(f => new {
                                f.FormId,
                                f.DepartmentId,
                                f.UserId,
                                f.CategoryId,
                                f.ProjectId,
                                f.ProcessNodeId,
                                f.Date,
                                createdTime = f.Date.ToLocalTime().ToString("yyyy-MM-dd HH:mm")
                            }).ToListAsync();

            //各個欄位資料
            var departments = await _context.Department.AsNoTracking().ToDictionaryAsync(d => d.DepartmentId, d => d.DepartmentName);
            var userName = await _context.User.AsNoTracking().ToDictionaryAsync(d => d.UserId, d => d.UserName);
            var projectName = await _context.Project.AsNoTracking().ToDictionaryAsync(d => d.ProjectId, d => d.ProjectName);
            var category = await _context.Category.AsNoTracking().ToDictionaryAsync(d => d.CategoryId, d => d.CategoryDescription);
            var situation = await _context.ProcessNodes.AsNoTracking().ToDictionaryAsync(d => d.ProcessNodeId, d => d.UserActivityId);
            var processNodes = await _context.ProcessNodes.AsNoTracking().ToDictionaryAsync(d => d.ProcessNodeId, d => d.UserActivityId);
            var userActivity = await _context.UserActivity.AsNoTracking().ToDictionaryAsync(d => d.UserActivityId, d => d.UserActivityIdDescription);
            //回傳json格式
            var result = new
            {
                findForm,
                departments,
                userName,
                projectName,
                category,
                processNodes,
                userActivity,

            };
            if (!findForm.Any())
            {
                //加入警示的viewbag
                ViewBag.noForm = "您的部門沒有任何工單";
                //一樣回傳空的 
                return Json(new { success = false, message = "這位使用者沒有工單" });
            }

            return Json(new { success = true, data = result });

        }


        //依照需求類別
        //Get :QueryForms/Category
        public async Task<IActionResult> Category(string categoryId)
        {
            User user = await GetAuthorizedUser();
            //加入撈取資料的判斷條件(必須要和登入者同部門，且還在活動的工單)
            var alllist = _context.Form.AsNoTracking().Where(f => f.DepartmentId == user.DepartmentId && f.FormIsActive == true);
            var findForm = await alllist.Include(c => c.Category)
                            .Include(c => c.Project)
                            .Where(f => f.CategoryId == categoryId)//改以需求類別判斷
                            .Select(f => new {
                                f.FormId,
                                f.DepartmentId,
                                f.UserId,
                                f.CategoryId,
                                f.ProjectId,
                                f.ProcessNodeId,
                                f.Date,
                                createdTime = f.Date.ToLocalTime().ToString("yyyy-MM-dd HH:mm")
                            }).ToListAsync();
    
            //各個欄位資料
            var departments = await _context.Department.AsNoTracking().ToDictionaryAsync(d => d.DepartmentId, d => d.DepartmentName);
            var userName = await _context.User.AsNoTracking().ToDictionaryAsync(d => d.UserId, d => d.UserName);
            var projectName = await _context.Project.AsNoTracking().ToDictionaryAsync(d => d.ProjectId, d => d.ProjectName);
            var category = await _context.Category.AsNoTracking().ToDictionaryAsync(d => d.CategoryId, d => d.CategoryDescription);
            var situation = await _context.ProcessNodes.AsNoTracking().ToDictionaryAsync(d => d.ProcessNodeId, d => d.UserActivityId);
            var processNodes = await _context.ProcessNodes.AsNoTracking().ToDictionaryAsync(d => d.ProcessNodeId, d => d.UserActivityId);
            var userActivity = await _context.UserActivity.AsNoTracking().ToDictionaryAsync(d => d.UserActivityId, d => d.UserActivityIdDescription);
            //回傳json格式
            var result = new
            {
                findForm,
                departments,
                userName,
                projectName,
                category,
                processNodes,
                userActivity,
     
            };
            if (!findForm.Any())
            {
                //一樣回傳空的 
                return Json(new { success = false, message = "您的部門沒有相關類別的工單" });
            }

            return Json(new { success = true, data = result });

        }

        //依照專案名稱
        //Get:QueryForms/Project
        public async Task<IActionResult> Project(string projectId)
        {
            User user = await GetAuthorizedUser();
            var alllist = _context.Form.AsNoTracking().Where(f => f.DepartmentId == user.DepartmentId && f.FormIsActive == true);
            var findForm = await alllist.Include(c => c.Category)
                .Include(c => c.Project)
                .Where(c => c.ProjectId == projectId)
                .Select(c => new
                {
                    c.FormId,
                    c.DepartmentId,
                    c.UserId,
                    c.CategoryId,
                    c.ProjectId,
                    c.ProcessNodeId,
                    c.Date,
                    createdTime = c.Date.ToLocalTime().ToString("yyyy-MM-dd HH:mm")
                })
                .ToListAsync();
            //各個欄位資料
            var departments = await _context.Department.AsNoTracking().ToDictionaryAsync(d => d.DepartmentId, d => d.DepartmentName);
            var userName = await _context.User.AsNoTracking().ToDictionaryAsync(d => d.UserId, d => d.UserName);
            var projectName = await _context.Project.AsNoTracking().ToDictionaryAsync(d => d.ProjectId, d => d.ProjectName);
            var category = await _context.Category.AsNoTracking().ToDictionaryAsync(d => d.CategoryId, d => d.CategoryDescription);
            var situation = await _context.ProcessNodes.AsNoTracking().ToDictionaryAsync(d => d.ProcessNodeId, d => d.UserActivityId);
            var processNodes = await _context.ProcessNodes.AsNoTracking().ToDictionaryAsync(d => d.ProcessNodeId, d => d.UserActivityId);
            var userActivity = await _context.UserActivity.AsNoTracking().ToDictionaryAsync(d => d.UserActivityId, d => d.UserActivityIdDescription);
            //回傳json格式
            var result = new
            {
                findForm,
                departments,
                userName,
                projectName,
                category,
                processNodes,
                userActivity,
            };
            if (!findForm.Any())
            {
                //一樣回傳空的 
                return Json(new { success = false, message = "您的部門沒有相關專案的工單" });
            }

            return Json(new { success = true, data = result });

        }

        //依照日期篩選
        //Get:QueryForms/createTime
        public async Task<IActionResult> CreatDate(DateTime date)
        {
            User user = await GetAuthorizedUser();
            //加入撈取資料的判斷條件(必須要和登入者同部門，且還在活動的工單)
            var alllist = _context.Form.AsNoTracking().Where(f => f.DepartmentId == user.DepartmentId && f.FormIsActive == true);
            var findForm = await alllist.Include(c => c.Category)
                            .Include(c => c.Project)
                            .Where(f => f.Date.AddHours(8).Date == date.Date)
                            .Select(f => new {
                                f.FormId,
                                f.DepartmentId,
                                f.UserId,
                                f.CategoryId,
                                f.ProjectId,
                                f.ProcessNodeId,
                                f.Date,
                                createdTime = f.Date.ToLocalTime().ToString("yyyy-MM-dd HH:mm")
                            }).ToListAsync();
            
            //各個欄位資料
            var departments = await _context.Department.AsNoTracking().ToDictionaryAsync(d => d.DepartmentId, d => d.DepartmentName);
            var userName = await _context.User.AsNoTracking().ToDictionaryAsync(d => d.UserId, d => d.UserName);
            var projectName = await _context.Project.AsNoTracking().ToDictionaryAsync(d => d.ProjectId, d => d.ProjectName);
            var category = await _context.Category.AsNoTracking().ToDictionaryAsync(d => d.CategoryId, d => d.CategoryDescription);
            var situation = await _context.ProcessNodes.AsNoTracking().ToDictionaryAsync(d => d.ProcessNodeId, d => d.UserActivityId);
            var processNodes = await _context.ProcessNodes.AsNoTracking().ToDictionaryAsync(d => d.ProcessNodeId, d => d.UserActivityId);
            var userActivity = await _context.UserActivity.AsNoTracking().ToDictionaryAsync(d => d.UserActivityId, d => d.UserActivityIdDescription);
            //回傳json格式
            var result = new
            {
                findForm,
                departments,
                userName,
                projectName,
                category,
                processNodes,
                userActivity,
     
            };
            if (!findForm.Any())
            {
                return Json(new { success = false, message = "這日期沒有建立相關工單" });
            }

            return Json(new { success = true, data = result });

        }


    }



}