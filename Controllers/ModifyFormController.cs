using BPMPlus.Data;
using BPMPlus.Models;
using BPMPlus.ViewModels;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Threading.Channels;

namespace BPMPlus.Controllers
{
    public class ModifyFormController : Controller
    {

        private readonly ApplicationDbContext _context;

        public ModifyFormController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: ModifyForm
        //public ActionResult Index()
        //{
        //    return View();
        //}

        [HttpGet]
        public IActionResult Index(string id)
        {

            ////CategoryViewModel選單用
            //var categories = _context.Category
            //     .AsNoTracking()
            //     .AsSplitQuery()
            //     .Select(c => new ModifyFormCategoryViewModel
            //     {
            //         CategoryDescription = c.CategoryDescription,
            //     }).ToList();

            ////ProjectViewModel選單用
            //var projects = _context.Project
            //  .AsNoTracking()
            //  .AsSplitQuery()
            //  .Select(c => new ModifyFormProjectViewModel
            //  {
            //      ProjectName = c.ProjectName
            //  }).ToList();

            //ModifyFormViewModel
            var formdata = _context.Form
                .Include(c => c.User)
                .Include(c => c.Department)
                .Include(c => c.Category)
                .Include(c => c.Project)
                .Where(c => c.FormId == id)
                .AsNoTracking()
                .AsSplitQuery()
                .Select(c => new ModifyFormViewModel
                {
                    FormId = c.FormId,
                    Id=c.FormId,
                    EmployeeName = c.User.UserName,
                    Date = c.Date.AddHours(8).ToString("yyyy-MM-dd"),
                    CategoryDescription = c.Category.CategoryDescription,
                    ProjectName = c.Project.ProjectName,
                    ExpectedFinishedDay = c.ExpectedFinishedDay.AddHours(8).ToString("yyyy-MM-dd"),
                    DepartmentName = c.Department.DepartmentName,
                    Content = c.Content,
                    Tel = c.Tel,
                    FormIsActive = true
                }).FirstOrDefault();

            //FormGroupViewModel
            var result = new ModifyFormGroupViewModel
            {
                ModifyFrom = formdata??new ModifyFormViewModel(),
                //Categories = categories,
                //Projects = projects
            };


            return View(result ?? new ModifyFormGroupViewModel());
        }


        [HttpPost]
      
        public async Task<IActionResult> Edit(string id, ModifyFormGroupViewModel model)
        {

            //確認傳入參數id是否有值
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("ID cannot be null or empty.");
            }

            //找出要傳入的單筆紀錄和欄位
            var editForm = _context.Form.FirstOrDefault(s => s.FormId == id);
            editForm.Content = model.ModifyFrom.Content;
            editForm.Tel = model.ModifyFrom.Tel;
            editForm.ExpectedFinishedDay = Convert.ToDateTime(model.ModifyFrom.ExpectedFinishedDay);
            

            try
            {
                _context.Update(editForm); 
                await _context.SaveChangesAsync();  
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FormExists(model.ModifyFrom.FormId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            

            //返回頁面:工單細節
            return RedirectToAction("Index","FormDetails",new { id = editForm.FormId });
        }

        [HttpPost]
        public async Task<IActionResult>Invalid(string id, ModifyFormGroupViewModel model) 
        {

            //確認傳入參數id是否有值
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("ID cannot be null or empty.");
            }

            //找出要傳入的單筆紀錄和欄位
            var invalidId = _context.Form.FirstOrDefault(c=>c.FormId== id);
            invalidId.FormIsActive = false;


            try
            {
                _context.Update(invalidId);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FormExists(model.ModifyFrom.FormId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            //返回查詢工單頁面
            return RedirectToAction("Index", "QueryForms");
        }

        private bool FormExists(string id)
        {
            return _context.Form.Any(e => e.FormId == id);
        }
    }
}
