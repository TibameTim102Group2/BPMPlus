using BPMPlus.Data;
using BPMPlus.Models;
using BPMPlus.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Data;
using System.Security.Claims;

namespace BPMPlus.Controllers
{
    
    public class CreateFormsController : BaseController
    {

        private readonly ApplicationDbContext _context;

        public CreateFormsController(ApplicationDbContext context):base(context)
        {
            _context = context;
        }
        [Authorize]
        public async Task<string> GetCreateFormId()
        {
            var LastForm = await _context.Form.OrderBy(f => f.FormId).LastAsync();
            if (LastForm == null)
                return "F00001";
            string id = LastForm.FormId;
            id = id[1..];//拿掉第一個 F
            int idNum = Convert.ToInt32(id);
            idNum++;
            return "F" + idNum.ToString().PadLeft(5, '0');
        }
        // GET: CreateForms
        [Authorize]

        public async Task<ActionResult> Index(string ReturnUrl)
        {
            User user = await GetAuthorizedUser();
            //functionId:  01 -> 需求方申請人送出
            
            if (!user.PermittedTo("01"))
            {
                return LocalRedirect(ReturnUrl);
            }

            var Department = await _context.Department
                 .FirstOrDefaultAsync(d => d.DepartmentId == user.DepartmentId);
            
            if (Department == null)
            {
                throw new Exception("Department is null, Server Error");
            }
            
            var Categories = await _context.Category.ToListAsync();
            if (Categories == null)
            {
                throw new Exception("Categories is null, Server Error");
            }
            
            
            

            var Forms = await _context.Form
            .Where(f => f.DepartmentId == user.DepartmentId)
            .ToListAsync(
                
            );



            ViewBag.DepartmentName = Department.DepartmentName;
            ViewBag.DepartmentId = Department.DepartmentId;
            ViewBag.UserId = user.UserId;
            ViewBag.UserTEL = user.TEL;
            ViewBag.Categories = Categories;
            ViewBag.Projects = user.Projects;
            ViewBag.Forms = Forms;

            return View();
            
        }
        [Authorize]
        [HttpGet]
        public async Task<JsonResult> GetFormById(string formId)
        {
            
            User user = await GetAuthorizedUser();
            //functionId:  01 -> 需求方申請人送出
            
            if (!user.PermittedTo("01"))
            {
                throw new Exception("User is not permitted)");
            }

            var form = await _context.Form.FirstOrDefaultAsync(
                f => f.FormId == formId
            );
            var catr = await _context.Category.FirstOrDefaultAsync(
                    c => c.CategoryId == form.CategoryId
                );
            var toFrontEnd = new GetFormForReferenceFormViewModel
            {
                FormId = form.FormId,
                UserId = form.UserId,
                CategoryId = form.CategoryId,
                Content = form.Content,
                CategoryDescription = catr.CategoryDescription
            };
            return Json(toFrontEnd);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> CreateNewForm([FromBody] NewFormViewModel model)
        {
            
            User user = await GetAuthorizedUser();
            
            //functionId:  01 -> 需求方申請人送出

            if (!user.PermittedTo("01"))
            {
                throw new Exception("User is not permitted)");
            }
            DateTime ExpectedFinishedDayDateTimeUtc8 = DateTime.Parse(model.ExpectedFinishedDay);
            if(ExpectedFinishedDayDateTimeUtc8.Date < (DateTime.UtcNow).AddHours(8).Date)
            {
                string msg = "希望完成時間需晚於當天日期";
                return Json(new { errorCode = 400, message = msg });
            }
            if (!ModelState.IsValid)
            {
                string msg = "資料缺漏，無法新建工單";
                return Json(new { errorCode=400 , message = msg });
            }
            List<ProcessTemplate> pTemplates = await _context.ProcessTemplate.Where(p => p.CategoryId == model.CategoryId).ToListAsync();
            
            Form newForm = new Form();
            newForm.FormId = await GetCreateFormId();
            newForm.DepartmentId = model.DepartmentId;
            newForm.Date = DateTime.UtcNow;
            newForm.CategoryId = model.CategoryId;
            newForm.UserId = user.UserId;
            if(model.ProjectId != "")
            {
                newForm.ProjectId = model.ProjectId;
            }
            newForm.DepartmentId = model.DepartmentId;
            newForm.Content = model.Content;
            newForm.ExpectedFinishedDay = ExpectedFinishedDayDateTimeUtc8.AddHours(-8);
            newForm.HandleDepartmentId = "處理部門";
            newForm.Tel = model.TEL;
            newForm.ProcessNodeId = "PT000001";
            newForm.FormIsActive = true;
            newForm.UpdatedTime = DateTime.Now;
            newForm.CreatedTime = DateTime.Now;

            _context.Form.Add(newForm);
            await _context.SaveChangesAsync();
            
            return Json(new { message = "Data received successfully!" });
        }
    }
}
