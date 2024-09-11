using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BPMPlus.Data;
using BPMPlus.Models;

namespace BPMPlus.Controllers
{
    public class AdminQueryFormsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminQueryFormsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AdminQueryForms
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Form.Include(f => f.Category).Include(f => f.Department).Include(f => f.Project).Include(f => f.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: AdminQueryForms/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var form = await _context.Form
                .Include(f => f.Category)
                .Include(f => f.Department)
                .Include(f => f.Project)
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.FormId == id);
            if (form == null)
            {
                return NotFound();
            }

            return View(form);
        }

        // GET: AdminQueryForms/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryId");
            ViewData["DepartmentId"] = new SelectList(_context.Department, "DepartmentId", "DepartmentId");
            ViewData["ProjectId"] = new SelectList(_context.Project, "ProjectId", "ProjectId");
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "UserId");
            return View();
        }

        // POST: AdminQueryForms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FormId,UserId,Date,CategoryId,ProjectId,DepartmentId,Content,ExpectedFinishedDay,Tel,ProcessNodeId,FormIsActive,ManDay,CreatedTime,UpdatedTime")] Form form)
        {
            if (ModelState.IsValid)
            {
                _context.Add(form);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryId", form.CategoryId);
            ViewData["DepartmentId"] = new SelectList(_context.Department, "DepartmentId", "DepartmentId", form.DepartmentId);
            ViewData["ProjectId"] = new SelectList(_context.Project, "ProjectId", "ProjectId", form.ProjectId);
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "UserId", form.UserId);
            return View(form);
        }

        // GET: AdminQueryForms/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var form = await _context.Form.FindAsync(id);
            if (form == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryId", form.CategoryId);
            ViewData["DepartmentId"] = new SelectList(_context.Department, "DepartmentId", "DepartmentId", form.DepartmentId);
            ViewData["ProjectId"] = new SelectList(_context.Project, "ProjectId", "ProjectId", form.ProjectId);
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "UserId", form.UserId);
            return View(form);
        }

        // POST: AdminQueryForms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("FormId,UserId,Date,CategoryId,ProjectId,DepartmentId,Content,ExpectedFinishedDay,Tel,ProcessNodeId,FormIsActive,ManDay,CreatedTime,UpdatedTime")] Form form)
        {
            if (id != form.FormId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(form);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FormExists(form.FormId))
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
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryId", form.CategoryId);
            ViewData["DepartmentId"] = new SelectList(_context.Department, "DepartmentId", "DepartmentId", form.DepartmentId);
            ViewData["ProjectId"] = new SelectList(_context.Project, "ProjectId", "ProjectId", form.ProjectId);
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "UserId", form.UserId);
            return View(form);
        }

        // GET: AdminQueryForms/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var form = await _context.Form
                .Include(f => f.Category)
                .Include(f => f.Department)
                .Include(f => f.Project)
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.FormId == id);
            if (form == null)
            {
                return NotFound();
            }

            return View(form);
        }

        // POST: AdminQueryForms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var form = await _context.Form.FindAsync(id);
            if (form != null)
            {
                _context.Form.Remove(form);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FormExists(string id)
        {
            return _context.Form.Any(e => e.FormId == id);
        }
    }
}
