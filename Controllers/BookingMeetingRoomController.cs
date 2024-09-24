using BPMPlus.Data;
using BPMPlus.Models;
using BPMPlus.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
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

            var rooms = await _context.MeetingRooms.Select(r => r.MeetingRoomId).ToArrayAsync();
            var timeSlots = GetTimeSlots(8, 22, 60);
            ViewBag.Rooms = rooms;
            ViewBag.TimeSlots = timeSlots;

            //預約人部門
            var Department = await _context.Department
                 .FirstOrDefaultAsync(d => d.DepartmentId == user.DepartmentId);
            
            //抓今天日期 (設定預約日期不可早於今天)
            var bookingDate = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.MinDate = bookingDate;

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

            return View();
        }

        
        private List<DateTime> GetTimeSlots(int start, int end, int Minutes)
        {
            var slots = new List<DateTime>();
            var currentTime = DateTime.Today.AddHours(start);
            var endTime = DateTime.Today.AddHours(end);

            while (currentTime <= endTime)
            {
                slots.Add(currentTime);
                currentTime = currentTime.AddMinutes(Minutes);
            }

            return slots;
        }

        // 撈會議室可容納人數
        public async Task<IActionResult> GetMeetingRoomInfo(string id)
        {
            var accommdation = _context.MeetingRooms
                .Where(n => n.MeetingRoomId == id)
                .Select(n => n.Accommodation);

            return Json(new { success=true, data = accommdation });
        }

        //GetBookedTime
        public async Task<IActionResult> CheakMeetingRooms(string BookingDate)
        {
            var bookedTimes = await _context.Meeting
                .Where(b=>b.StartTime.Date == DateTime.Parse(BookingDate).Date)
                .Select(b=>new {
                    StartTime = b.StartTime.AddHours(8).ToString("HH:mm"),
                    EndTime = b.EndTime.AddHours(8).ToString("HH:mm"),
                    roomId = b.MeetingRoomId
                })
                .ToListAsync();
            return Json(new { success = true, data = bookedTimes });
        }


        [HttpPost]
        public async Task<IActionResult> SubmitBooking([FromBody] BookingMeetingRoomVM vm)
        {
            var bookedTime = await _context.Meeting
                .Where(d => d.StartTime.Date == DateTime.Parse(vm.Date) && d.MeetingRoomId == vm.Room)
                .Select(d => new { d.StartTime, d.EndTime })
                .ToListAsync();


            string[] start = new string[14];
            string[] end = new string[14];
            foreach (var time in bookedTime)
            {

            }



            if (ModelState.IsValid)
            {
                var StartTime = DateTime.Parse($"{vm.Date} {vm.StartTime}").AddHours(-8);
                var EndTime = DateTime.Parse($"{vm.Date} {vm.EndTime}").AddHours(-8);

                


                Meeting meeting = new Meeting();
                List<string> id = await CreateMeetingIdListAsync(1);

                var host = await _context.User
                    .Where(u => u.UserName == vm.MeetingHost)
                    .Select(h => h.UserId)
                    .FirstOrDefaultAsync();


                meeting.MeetingId = id.FirstOrDefault();
                meeting.MeetingRoomId = vm.Room;
                meeting.StartTime = StartTime;
                meeting.EndTime = EndTime;
                meeting.MeetingHost = host;
                meeting.Note = vm.Note;
                meeting.CreatedTime = DateTime.Now.AddHours(-8);
                meeting.UpdatedTime = DateTime.Now.AddHours(-8);

                await _context.Meeting.AddAsync(meeting);
                await _context.SaveChangesAsync();


                var meetingid = _context.Meeting
                    .Include(x => x.Users)
                    .FirstOrDefault(x => x.MeetingId == id[0]);

                foreach (var memberId in vm.Members)
                {
                    //重新建關聯
                    var newMeetingMember = _context.User.FirstOrDefault(pg => pg.UserId == memberId);
                    if (newMeetingMember != null)
                    {
                        meetingid.Users.Add(newMeetingMember);
                        _context.SaveChanges();
                    }
                }
            }
            else
            {
                var falseText = "預約失敗! 請重新預約!";
                return Json(new { success = false, data = falseText });
            }
            return RedirectToAction("Index", "CurrentMeetingRoom");
        }

    }
}
