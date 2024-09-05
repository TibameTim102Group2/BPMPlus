using BPMPlus.Data;
using BPMPlus.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
            var Projects = await _context.Project.ToListAsync();
            if(Projects == null)
            {
                throw new Exception("Projects is null, Server Error");
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
            ViewBag.Projects = Projects;
            ViewBag.Forms = Forms;

            return View();
            
        }
    }
}
