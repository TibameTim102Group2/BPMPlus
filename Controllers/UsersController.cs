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
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;
using NuGet.Protocol.Providers;

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

			var userPermission = userWithGroups.PermissionGroups.Select(u => u.PermissionGroupName).ToList();
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

			var user = await _context.User
				.Include(u => u.Department)
				.Include(u => u.Grade).AsNoTracking()
				.FirstOrDefaultAsync(m => m.UserId == id);
			//加入判斷是否在職字典
			Dictionary<int, bool> userIsActive = new Dictionary<int, bool>()
			{
				{ 0, false},
				{ 1, true },
			};

			List<SelectListItem> userNowAction = userIsActive.Values.Distinct()
				.Select(status => new SelectListItem
				{
					Text = status ? "在職" : "不在職",
					Value = status.ToString()
				}).ToList();
			ViewBag.UserIsAction = userNowAction;
			var userWithGroups = _context.User
			.Include(u => u.PermissionGroups)
			 .FirstOrDefault(u => u.UserId == id);

			var userPermission = userWithGroups.PermissionGroups.Select(u => new
			{
				Value = u.PermissionGroupId,
				Text = u.PermissionGroupName,
			}).ToList();
			ViewBag.userPermission = JsonConvert.SerializeObject(userPermission);
			var userPermissionId = userWithGroups.PermissionGroups.Select(u => u.PermissionGroupId).ToList();
			ViewBag.Department = new SelectList(_context.Department, "DepartmentId", "DepartmentName");
			ViewBag.Grade = new SelectList(_context.Grade, "GradeId", "GradeName");
			var allPermission = _context.PermissionGroup.Select(p => new
			{
				Value = p.PermissionGroupId,
				Text = p.PermissionGroupName,
			}).ToList();
			ViewBag.PermissionGroup = JsonConvert.SerializeObject(allPermission);
			return View(user);
		}

		// POST: Users/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		public async Task<IActionResult> Edit([FromBody] bKUserEditViewModel userData)
		{

			var user = _context.User.Include(x => x.PermissionGroups).FirstOrDefault(x => x.UserId == userData.UserId);
			if (ModelState.IsValid)
			{
				try
				{
					user.UserId = userData.UserId;
					user.UserName = userData.UserName;
					user.TEL = userData.Tel;
					user.GradeId = userData.GradeId;
					user.DepartmentId = userData.DepartmentId;
					//把收到的字串轉乘布林
					if (userData.UserIsActive == "True")
					{
						user.UserIsActive = true;
					}
					else
					{
						user.UserIsActive = false;
					}
					
					user.Email = userData.Email;
					//撈出使用者所有的permissiongroups
					var userNowPermission = user.PermissionGroups.ToList();
					//var userNowPermission = _context.PermissionGroup.Include(u => u.Users).Where(u => u.UserId == userData.UserId).ToList();
					
					//先把關連表全刪
					foreach (var item in userNowPermission)
					{

						if (item != null)
						{
							user.PermissionGroups.Remove(item); //刪除關連
							_context.SaveChanges();
						}
					}

					//當不為空的話
					//全部重新新增
					//要把關連表的table改為現在輸入的PermissionGroupId
					foreach (var item in userData.Permissions)
                    {

						//重新建關聯
						var newPermissionGroup = _context.PermissionGroup.FirstOrDefault(pg => pg.PermissionGroupId == item);
						if (newPermissionGroup != null)
						{
							user.PermissionGroups.Add(newPermissionGroup);
							_context.SaveChanges();
						}
					}


					user.PermissionGroups.Where(n => n.PermissionGroupId == userData.UserId).ToList();
					//user.PermissionGroups.Select(x => x.PermissionGroupId) = userData.Permissions;
					//_context.Update(userData);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!UserExists(userData.UserId))
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
			//ViewData["DepartmentId"] = new SelectList(_context.Department, "DepartmentId", "DepartmentName", formData.DepartmentId);
			//ViewData["GradeId"] = new SelectList(_context.Grade, "GradeId", "GradeName", formData.GradeId);
			return View(userData);
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
