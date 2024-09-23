using BPMPlus.Data;
using BPMPlus.Models;
using BPMPlus.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Globalization;
using JsonSerializer = System.Text.Json.JsonSerializer;

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

            var allrooms = await _context.MeetingRooms.AsNoTracking().Select(n => n.MeetingRoomId).ToListAsync();
            ViewBag.allRooms = JsonSerializer.Serialize(allrooms);

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

        // 撈會議室可容納人數
        public async Task<IActionResult> GetMeetingRoomInfo(string id)
        {
            var accommdation = _context.MeetingRooms
                .Where(n => n.MeetingRoomId == id)
                .Select(n => n.Accommodation);

            return Json(new { success=true, data=accommdation });
        }

        //GetBookedTime
        public async Task<IActionResult> CheakMeetingRooms(string RoomId, string BookingDate)
        {
            var bookedTimes = await _context.Meeting
                .Where(b=>b.MeetingRoomId == RoomId && b.StartTime.Date == DateTime.Parse(BookingDate).Date)
                .Select(b=>new {
                    StartTime = b.StartTime.AddHours(8).ToString("HH:mm"),
                    EndTime = b.EndTime.AddHours(8).ToString("HH:mm"),
                    roomId = b.MeetingRoomId
                })
                .ToListAsync();
            return Json(new { success = true, data = bookedTimes });
        }


		public async Task<ActionResult> GetMeetingBook(string id)
		{
			//找出所當日有被預約的時間
			var hasBookedData = await _context.Meeting.AsNoTracking().Where(m => m.StartTime.Date.ToString() == id)
				.Select(n => new
				{
					meetingRoomId = n.MeetingRoomId,
					startTime = n.StartTime.AddHours(8).Hour,
					endTime = n.EndTime.AddHours(8).Hour,
				})
				.ToListAsync();
			//選取只需要的資料
			return Json(new { success = true, data = hasBookedData });
		}



		[HttpPost]
        public async Task<IActionResult> SubmitBooking([FromBody] BookingMeetingRoomVM vm)
        {

            List<string> id = await CreateMeetingIdListAsync(1);

            if (ModelState.IsValid)
            {
                


                Meeting meeting = new Meeting();

                var host = await _context.User
                    .Where(u => u.UserName == vm.MeetingHost)
                    .Select(h => h.UserId)
                    .FirstOrDefaultAsync();

                meeting.MeetingId = id.FirstOrDefault();
                meeting.MeetingRoomId = vm.Room;
                meeting.StartTime = DateTime.ParseExact(vm.StartDate, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture).AddHours(-8);
                meeting.EndTime = DateTime.ParseExact(vm.EndDate, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture).AddHours(-8);
                meeting.MeetingHost = host;
                meeting.Note = vm.Note;
                meeting.CreatedTime = DateTime.Now.AddHours(-8);
                meeting.UpdatedTime = DateTime.Now.AddHours(-8);

                await _context.Meeting.AddAsync(meeting);
                await _context.SaveChangesAsync();
            }
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
            return RedirectToAction("Index", "CurrentMeetingRoom");
        }

    }
}
