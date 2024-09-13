using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BPMPlus.Data;
using BPMPlus.Models;
using BPMPlus.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace BPMPlus.Controllers
{
    public class ProcessTemplatesController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public ProcessTemplatesController(ApplicationDbContext context):base(context)
        {
            _context = context;
        }

        // GET: ProcessTemplates
        public async Task<IActionResult> Index()
        {
            //var applicationDbContext = _context.ProcessTemplate.Include(p => p.Category).Include(p => p.UserActivity);
            return View();
        }

        // GET: ProcessTemplates/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var processTemplate = await _context.ProcessTemplate
                .Include(p => p.Category)
                .Include(p => p.UserActivity)
                .FirstOrDefaultAsync(m => m.ProcessTemplateId == id);
            if (processTemplate == null)
            {
                return NotFound();
            }

            return View(processTemplate);
        }

        // GET: ProcessTemplates/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> CreateCategory([FromBody] CreateCategory model)
        {
            User user = await GetAuthorizedUser();

            //functionId:  13 -> 新增需求類別

            if (!user.PermittedTo("13"))
            {
                return Json(new { errorCode = 200, message = $"使用者無權新增工單類別" });
            }
            

            return Json(new { errorCode = 200, message = $"新增需求類別成功!"});
        }

        // POST: ProcessTemplates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProcessTemplateId,UserActivityId,DepartmentId,CategoryId,CreatedTime,UpdatedTime")] ProcessTemplate processTemplate)
        {
            if (ModelState.IsValid)
            {
                _context.Add(processTemplate);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryId", processTemplate.CategoryId);
            ViewData["UserActivityId"] = new SelectList(_context.UserActivity, "UserActivityId", "UserActivityId", processTemplate.UserActivityId);
            return View(processTemplate);
        }

        // GET: ProcessTemplates/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var processTemplate = await _context.ProcessTemplate.FindAsync(id);
            if (processTemplate == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryId", processTemplate.CategoryId);
            ViewData["UserActivityId"] = new SelectList(_context.UserActivity, "UserActivityId", "UserActivityId", processTemplate.UserActivityId);
            return View(processTemplate);
        }

        // POST: ProcessTemplates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ProcessTemplateId,UserActivityId,DepartmentId,CategoryId,CreatedTime,UpdatedTime")] ProcessTemplate processTemplate)
        {
            if (id != processTemplate.ProcessTemplateId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(processTemplate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProcessTemplateExists(processTemplate.ProcessTemplateId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryId", processTemplate.CategoryId);
            ViewData["UserActivityId"] = new SelectList(_context.UserActivity, "UserActivityId", "UserActivityId", processTemplate.UserActivityId);
            return View(processTemplate);
        }

        // GET: ProcessTemplates/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var processTemplate = await _context.ProcessTemplate
                .Include(p => p.Category)
                .Include(p => p.UserActivity)
                .FirstOrDefaultAsync(m => m.ProcessTemplateId == id);
            if (processTemplate == null)
            {
                return NotFound();
            }

            return View(processTemplate);
        }

        // POST: ProcessTemplates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var processTemplate = await _context.ProcessTemplate.FindAsync(id);
            if (processTemplate != null)
            {
                _context.ProcessTemplate.Remove(processTemplate);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProcessTemplateExists(string id)
        {
            return _context.ProcessTemplate.Any(e => e.ProcessTemplateId == id);
        }
    }
}
