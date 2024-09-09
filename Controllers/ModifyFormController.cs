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
        public async Task<IActionResult>Invalid(string id) 
        {

            //確認傳入參數id是否有值
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("ID cannot be null or empty.");
            }

            //找出要傳入的單筆紀錄和欄位
            var invalidId = _context.Form.FirstOrDefault(c=>c.FormId== id);
            invalidId.FormIsActive = false;

            await _context.SaveChangesAsync();

            //try
            //{
            //    _context.Update(invalidId);
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //if (!FormExists(model.ModifyFrom.FormId))
            //{
            //    return NotFound();
            //}
            //else
            //{
            //    throw;
            //}
            //}

            //返回查詢工單頁面
            return RedirectToAction("Index", "QueryForms");
       
        }

        [HttpPost]
        public async Task<IActionResult> Upload(UploadInputModel data)
        {

            try
            {
                var form = _context.Form.FirstOrDefault(x => x.FormId == data.Id);

                form.Content = data.Content;
                form.Tel = data.Tel;
                form.ExpectedFinishedDay = data.Enddate;

                // 指定專案資料夾名稱
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/upload", data.Id);

                // 檢查資料夾是否存在，如果不存在則創建一個新資料夾
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // 檢查是否有上傳的檔案
                if (data.Files != null && data.Files.Count > 0)
                {
                    foreach (var file in data.Files)
                    {  
                        // 檔案存放的完整路徑
                        var filePath = Path.Combine(folderPath, file.FileName);

                        // 保存檔案
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                    }
                }

                //if (data.File != null)
                //{
                //    // 獲得文件檔案類型
                //    string extension = Path.GetExtension(data.File.FileName);

                //    // 設置路徑
                //    string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UploadFiles", data.File.FileName);

                //    // 確認目錄
                //    Directory.CreateDirectory(Path.GetDirectoryName(uploadPath));

                //    // 保存文件 
                //    using (var stream = new FileStream(uploadPath, FileMode.Create))
                //    {
                //        await data.File.CopyToAsync(stream);
                //    }
                //}
                _context.SaveChanges();
                return Json(new { success = true, message = "上傳成功" });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "上傳失敗" });
            }


            
        }

        private bool FormExists(string id)
        {
            return _context.Form.Any(e => e.FormId == id);
        }
    }
}
