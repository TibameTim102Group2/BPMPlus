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

namespace BPMPlus.Controllers
{
    public class UsersController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View();
        }
        // GET: Users
        [HttpGet]
        public async Task<JsonResult> userData()
        {
            //所有的使用者集合
            var userslist = await _context.User.AsNoTracking()
                .Include(u => u.Department)
                .Include(u => u.Grade)
                .Select(c => new bKUsersViewModel
                {
                    UserId = c.UserId,
                    UserName = c.UserName,
                    DepartmentName = c.Department.DepartmentName,
                    GradeName = c.Grade.GradeName,
                    Email = c.Email,
                    IncertDataTime = c.CreatedTime.AddHours(8).ToString("yyyy-MM-dd"),
                    UserIsActive = c.UserIsActive,
                }
            ).ToListAsync();

            return Json(userslist);
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .Include(u => u.Department)
                .Include(u => u.Grade).AsNoTracking()
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (!user.UserIsActive)
            {
                ViewBag.UserIsActive = "不在職";
            }
            else
            {
                ViewBag.UserIsActive = "在職";
            }
            var userWithGroups = _context.User
            .Include(u => u.PermissionGroups)
             .FirstOrDefault(u => u.UserId == id);

            var userPermission = userWithGroups.PermissionGroups.Select(u=>u.PermissionGroupName).ToList();
            ViewBag.userPermission = userPermission;

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            ViewData["DepartmentId"] = new SelectList(_context.Department, "DepartmentId", "DepartmentId");
            ViewData["GradeId"] = new SelectList(_context.Grade, "GradeId", "GradeId");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,UserName,DepartmentId,GradeId,Email,Password,UserIsActive,CreatedTime,UpdatedTime,TEL")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentId"] = new SelectList(_context.Department, "DepartmentId", "DepartmentId", user.DepartmentId);
            ViewData["GradeId"] = new SelectList(_context.Grade, "GradeId", "GradeId", user.GradeId);
            return View(user);
        }

        //編輯
        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["DepartmentId"] = new SelectList(_context.Department, "DepartmentId", "DepartmentId", user.DepartmentId);
            ViewData["GradeId"] = new SelectList(_context.Grade, "GradeId", "GradeId", user.GradeId);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("UserId,UserName,DepartmentId,GradeId,Email,Password,UserIsActive,CreatedTime,UpdatedTime,TEL")] User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
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
            ViewData["DepartmentId"] = new SelectList(_context.Department, "DepartmentId", "DepartmentId", user.DepartmentId);
            ViewData["GradeId"] = new SelectList(_context.Grade, "GradeId", "GradeId", user.GradeId);
            return View(user);
        }

        // Delete: Users/Invalid/5
        [HttpDelete]
        public async Task<IActionResult> Invalid(string id)
        {

            //確認傳入參數userid是否有值
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("ID cannot be null or empty.");
            }

            var invalidId = _context.User.FirstOrDefault(c => c.UserId == id);
            invalidId.UserIsActive = false;
            _context.Update(invalidId);
            await _context.SaveChangesAsync();
            //返回查詢人員
            return RedirectToAction("Index", "Users");
        }




        private bool UserExists(string id)
        {
            return _context.User.Any(e => e.UserId == id);
        }
    }
}
