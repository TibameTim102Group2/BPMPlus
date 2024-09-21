using BPMPlus.Data;
using BPMPlus.Models;
using BPMPlus.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol;
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
            ViewBag.MinDate = bookingDate;

            return View();
        }

        // 撈會議室可容納人數
        public async Task<IActionResult> GetMeetingRoomInfo(string id)
        {
            User user = await GetAuthorizedUser();
            var accommdation = _context.MeetingRooms
                .Where(n => n.MeetingRoomId == id)
                .Select(n => n.Accommodation);

            return Json(new { success=true, data=accommdation });
        }

        //確認預約狀況
        public async Task<IActionResult> CheakMeetingRooms(string RoomId, string BookingDate)
        {
            User user = await GetAuthorizedUser();

            var cheaking = await _context.Meeting
                .Where(b=>b.MeetingRoomId == RoomId && b.StartTime.Date == DateTime.Parse(BookingDate).Date)
                .Select(b=>new {
                    StartTime=b.StartTime.AddHours(8).ToString("HH"),
                    EndTime=b.EndTime.AddHours(8).ToString("HH")
                })
                .ToListAsync();

            return Json(new { success = true, data = cheaking });
        }


        [HttpPost]
        public async Task<IActionResult> SubmitBooking([FromBody] BookingMeetingRoomVM vm)
        {
            //if (ModelState.IsValid)
            //{
            //    Meeting meeting = new Meeting();
            //    meeting.MeetingId = "";
            //    meeting.MeetingRoomId = vm.Room;
            //    meeting.StartTime = DateTime.Parse(vm.StartTime);
            //    meeting.EndTime = DateTime.Parse(vm.EndTime);
            //    meeting.Note = vm.Note;
            //    await _context.Meeting.AddAsync(meeting);
            //    await _context.SaveChangesAsync();

            //}

            return RedirectToAction("Index", "CurrentMeetingRoom");
        }

    }
}
