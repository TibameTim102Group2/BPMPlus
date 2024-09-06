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

            
            

            var Forms = await _context.Form.ToListAsync();
            if (Forms == null)
            {
                throw new Exception("Forms is null, Server Error");
            }


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
            var x = model;
            User user = await GetAuthorizedUser();
            //functionId:  01 -> 需求方申請人送出

            if (!user.PermittedTo("01"))
            {
                throw new Exception("User is not permitted)");
            }
            if (ModelState.IsValid)
            {
                //// 假設有資料庫上下文 db
                //_context.Form.Add(model); // 假設模型對應到資料庫
                //_context.SaveChanges();

                return Json(new { success = true, message = "Form submitted successfully!" });
            }

            return Json(new { success = false, message = "Form submission failed. Please check your data." });
        }
    }
}
