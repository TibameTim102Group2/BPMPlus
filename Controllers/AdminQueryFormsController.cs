using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BPMPlus.Data;
using BPMPlus.Models;
using Microsoft.AspNetCore.Authorization;
using BPMPlus.ViewModels;
using Microsoft.SqlServer.Server;
using System.Data;

namespace BPMPlus.Controllers
{
    public class AdminQueryFormsController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public AdminQueryFormsController(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        [Authorize]
        // GET: AdminQueryForms/Index
        public async Task<IActionResult> Index()
        {
            // 判斷當前USER
            User user = await GetAuthorizedUser();


            // 查詢表一覽
            var allList = _context.Form
                .Include(c => c.User)
                .Include(d => d.Department)
                .Include(p => p.Project)
                .Include(ct => ct.Category)
                .Include(fr => fr.FormRecord)
                .ThenInclude(ua => ua.UserActivity)
                .OrderByDescending(d => d.CreatedTime)
                .Select(c => new AdminQueryFormsViewModel
                {
                    FormId = c.FormId,
                    DepartmentName = c.Department.DepartmentName,
                    EmployeeId = c.UserId,
                    UserName = c.User.UserName,
                    Categories = c.Category.CategoryDescription,
                    ProjectName = c.Project.ProjectName,
                    UserActivityDescription = c.FormRecord
                    .OrderByDescending(fr => fr.CreatedTime)
                    .Select(fr => fr.UserActivity.UserActivityIdDescription)
                    .FirstOrDefault(),
                    CreatedTime = c.CreatedTime.ToLocalTime().ToString("yyyy-MM-dd HH:mm"),
                    isFormActive = c.FormIsActive,
                    FormActive = c.FormIsActive ? "生效" : "作廢",
                }).ToList();

            // 設置前端下拉選單
            ViewBag.Department = new SelectList(_context.Department, "DepartmentId", "DepartmentName");
            ViewBag.Category = new SelectList(_context.Category, "CategoryId", "CategoryDescription");
            ViewBag.Project = new SelectList(_context.Project, "ProjectId", "ProjectName");

            // 設置前端下拉選單, 替換相對文字
            ViewBag.FormActive = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Text = "生效", Value = "True" },
                new SelectListItem { Text = "作廢", Value = "False" }
            }, "Value", "Text");


            return View(allList);
        }

		[Authorize]
		[HttpGet]
        public async Task<IActionResult> CompositeSearch(string formId, string departmentId, string categoryId, string projectId, string formIsActive, string createdDate)
        {
            var query = _context.Form
                .Include(c => c.User)
                .Include(d => d.Department)
                .Include(p => p.Project)
                .Include(ct => ct.Category)
                .Include(fr => fr.FormRecord)
                .ThenInclude(ua => ua.UserActivity)
                .AsQueryable();

            if (!string.IsNullOrEmpty(formId))
                query = query.Where(c => c.FormId.Contains(formId));

            if (!string.IsNullOrEmpty(departmentId))
                query = query.Where(c => c.DepartmentId == departmentId);

            if (!string.IsNullOrEmpty(categoryId))
                query = query.Where(c => c.CategoryId == categoryId);

            if (!string.IsNullOrEmpty(projectId))
                query = query.Where(c => c.ProjectId == projectId);

            if (!string.IsNullOrEmpty(formIsActive))
                query = query.Where(c => c.FormIsActive.ToString() == formIsActive);

            if (!string.IsNullOrEmpty(createdDate))
                query = query.Where(c => c.CreatedTime.AddHours(8).Date == DateTime.Parse(createdDate).Date);

            var result = await query.Select(c => new AdminQueryFormsViewModel
            {
                FormId = c.FormId,
                DepartmentName = c.Department.DepartmentName,
                EmployeeId = c.UserId,
                UserName = c.User.UserName,
                Categories = c.Category.CategoryDescription,
                ProjectName = c.Project.ProjectName,
                UserActivityDescription = c.FormRecord
                    .OrderByDescending(fr => fr.CreatedTime)
                    .Select(fr => fr.UserActivity.UserActivityIdDescription)
                    .FirstOrDefault(),
                CreatedTime = c.CreatedTime.ToLocalTime().ToString("yyyy-MM-dd HH:mm"),
                isFormActive = c.FormIsActive,
                FormActive = c.FormIsActive ? "生效" : "作廢",
            }).ToListAsync();

            if (result.Count == 0)
            {
                return Json(new { message = "沒有符合搜尋條件的工單" });
            }

            return Json(result);
        }

		[Authorize]
		[HttpGet]
        // GET: AdminQueryForms/GetAll
        // 全部工單搜尋使用
        public async Task<IActionResult> GetAll()

        {
            var query = _context.Form
              .Include(c => c.User)
              .Include(d => d.Department)
              .Include(p => p.Project)
              .Include(ct => ct.Category)
              .Include(fr => fr.FormRecord)
              .ThenInclude(ua => ua.UserActivity)
              .OrderByDescending(d => d.CreatedTime)
              .Select(c => new AdminQueryFormsViewModel
              {
                  FormId = c.FormId,
                  DepartmentName = c.Department.DepartmentName,
                  EmployeeId = c.UserId,
                  UserName = c.User.UserName,
                  Categories = c.Category.CategoryDescription,
                  ProjectName = c.Project.ProjectName,
                  UserActivityDescription = c.FormRecord
                  .OrderByDescending(fr => fr.CreatedTime)
                  .Select(fr => fr.UserActivity.UserActivityIdDescription)
                  .FirstOrDefault(),
                  CreatedTime = c.CreatedTime.ToLocalTime().ToString("yyyy-MM-dd HH:mm"),
                  isFormActive = c.FormIsActive,
                  FormActive = c.FormIsActive ? "生效" : "作廢",
              }).ToList();

            return Json(query);
        }

		[Authorize]
		[HttpPost]
        // POST: AdminQueryForms/DeleteSingle/formId
        // 單筆刪除
        public async Task<IActionResult> DeleteSingle(string formId)
        {
			User user = await GetAuthorizedUser();
            //  抓有關該工單的表
            var form = await _context.Form
                .Include(f => f.FormRecord)
                .Include(pn => pn.ProcessNode)
                .FirstOrDefaultAsync(f => f.FormId == formId);

            if (form == null)
            {
                return Json(new { success = false, message = "工單不存在" });
            }

            // 手動連鎖刪除
            _context.FormRecord.RemoveRange(form.FormRecord);
            _context.ProcessNodes.RemoveRange(form.ProcessNode);
            _context.Form.Remove(form);
            await _context.SaveChangesAsync();

			string folderName = formId;
			var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "upload");
			var folderPath = Path.Combine(uploadPath, folderName);

			if (Directory.Exists(folderPath))
			{
				try
				{
					Directory.Delete(folderPath, true);
				}
				catch (Exception ex)
				{
					Console.WriteLine($"刪除資料夾失敗: {folderPath}. 錯誤: {ex.Message}");
					return Json(new { success = false, message = $"刪除資料夾失敗: {ex.Message}" });
				}
			}

			return Json(new { success = true });
		}

		[Authorize]
		[HttpPost]
        // POST: AdminQueryForms/DeleteMany/formIds
        // 批次刪除
        public async Task<IActionResult> DeleteMany(List<string> formIds)
        {

            var forms = await _context.Form
                .Include(f => f.FormRecord)
                .Include(pn => pn.ProcessNode)
                .Where(f => formIds.Contains(f.FormId)).ToListAsync();

            if (forms.Count == 0)
            {
                return Json(new { success = false, message = "未找到工單" });
            }

            // 收集關聯表資料
            var formRecordDelete = forms.SelectMany(f => f.FormRecord).ToList();
            var processNodeDelte = forms.SelectMany(pn => pn.ProcessNode).ToList();

            // 手動連鎖刪除
            _context.FormRecord.RemoveRange(formRecordDelete);
            _context.ProcessNodes.RemoveRange(processNodeDelte);
            _context.Form.RemoveRange(forms);
            await _context.SaveChangesAsync();

			var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "upload");

			if (Directory.Exists(uploadPath))
			{
				foreach (var formId in formIds)
				{
					var folderPath = Path.Combine(uploadPath, formId);

					if (Directory.Exists(folderPath))
					{
						try
						{
							Directory.Delete(folderPath, true);
						}
						catch (Exception ex)
						{
							return Json(new { success = false, message = ex.Message });
						}
					}
					else
					{
						return Json(new { success = false, message = "該文件資料夾不存在" });
					}
				}
			}
			else
			{
				return Json(new { success = false, message = "檔案路徑不存在" });
			}

			return Json(new { success = true });
        }

		[Authorize]
		[HttpPost]
        // POST: AdminQueryForms/RecoverySingle/formId
        // 單筆復原
        public async Task<IActionResult> RecoverySingle(string formId)
        {
            //  抓該工單
            var RecoveryformActive = await _context.Form
                .Where(f => f.FormId == formId).FirstOrDefaultAsync();

            if (RecoveryformActive == null)
            {
                return Json(new { success = false, message = "工單不存在, 無法復原 ! " });
            }

            // 復原賦值
            RecoveryformActive.FormIsActive = true;

            // 手動復原
            _context.Update(RecoveryformActive);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

		[Authorize]
		[HttpPost]
        // POST: AdminQueryForms/RecoveryMany/formIds
        // 批次復原
        public async Task<IActionResult> RecoveryMany(List<string> formIds)
        {

            var RecoveryformActive = await _context.Form
                .Where(f => formIds.Contains(f.FormId)).ToListAsync();

            if (RecoveryformActive.Count == 0)
            {
                return Json(new { success = false, message = "未找到工單" });
            }

            foreach (var item in RecoveryformActive)
            {
                item.FormIsActive = true;
            }

            // 手動復原
            _context.UpdateRange(RecoveryformActive);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }
    }
}
