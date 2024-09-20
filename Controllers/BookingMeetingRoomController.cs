using BPMPlus.Data;
using BPMPlus.Models;
using BPMPlus.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Text.Json;

namespace BPMPlus.Controllers
{
    public class BookingMeetingRoomController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public BookingMeetingRoomController(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            User user = await GetAuthorizedUser();

            //預約人部門
            var Department = await _context.Department
                 .FirstOrDefaultAsync(d => d.DepartmentId == user.DepartmentId);
            
            //抓今天日期 (設定預約日期不可早於今天)
            var bookingDate = DateTime.Now.ToString("yyyy-MM-dd");

            //User字典
            var departments = _context.User
                .Include(u => u.Department)
                .GroupBy(u => u.Department.DepartmentName)
                .ToDictionary(g => g.Key,
                    g => g.Select(u => new{ u.UserId, u.UserName}).ToList());

            // 將字典轉換成 JSON 格式
            string employees = JsonSerializer.Serialize(departments);
            ViewBag.Employees = employees;

            ViewBag.MeetingRooms = new SelectList(_context.MeetingRooms, "MeetingRoomId", "MeetingRoomId");
            ViewBag.MeetingHost = user.UserName;
            ViewBag.DepartmentName = Department.DepartmentName;
            ViewBag.StartDate = bookingDate;

            return View();
        }

        // 撈會議室可容納人數
        public async Task<IActionResult> GetMeetingRoomInfo(string id)
        {
            User user = await GetAuthorizedUser();
            var accommdation = _context.MeetingRooms.Where(n => n.MeetingRoomId == id).Select(n => n.Accommodation);

            return Json(new { success=true, data=accommdation });
        }

        //確認會議室狀況
        public async Task<IActionResult> CheakMeetingRooms(BookingMeetingRoomVM vm)
        {
            User user = await GetAuthorizedUser();


            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SubmitBooking([FromBody] BookingMeetingRoomVM vm)
        {
            


            return RedirectToAction("Index", "CurrentMeetingRoom");
        }

    }
}
