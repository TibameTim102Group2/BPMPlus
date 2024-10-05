using BPMPlus.Attributes;
using BPMPlus.Data;
using BPMPlus.Models;
using BPMPlus.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
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
		[Authorize]
		public async Task<IActionResult> GetMeetingRoomInfo(string id)
        {
            var accommdation = _context.MeetingRooms
                .Where(n => n.MeetingRoomId == id)
                .Select(n => n.Accommodation);

            return Json(new { success=true, data = accommdation });
        }

		//GetBookedTime
		[Authorize]
		public async Task<IActionResult> CheckMeetingRooms(string BookingDate)
        {
            var bookedTimes = await _context.Meeting
                .Where(b=>b.StartTime.Date == DateTime.Parse(BookingDate).Date)
                .Join(_context.User, // 假設您的使用者表格名為 "User"
						m => m.MeetingHost,
						u => u.UserId,
						(m, u) => new {
							StartTime = m.StartTime.AddHours(8).ToString("HH:mm"),
							EndTime = m.EndTime.AddHours(8).ToString("HH:mm"),
							roomId = m.MeetingRoomId,
							host = m.MeetingHost,
							userName = u.UserName
						})
                .ToListAsync();
            return Json(new { success = true, data = bookedTimes });
        }

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> SubmitBooking([FromBody] BookingMeetingRoomVM vm)
		{
			var bookedTime = await _context.Meeting
				.Where(d => d.StartTime.Date == DateTime.Parse(vm.Date) && d.MeetingRoomId == vm.Room)
				.Select(d => new { d.StartTime, d.EndTime })
				.ToListAsync();

			if (ModelState.IsValid == false)
			{
				return Json(new { success = false, message = "預約失敗! 請重新預約!" });
			}
			else
			{
				var StartTime = DateTime.Parse($"{vm.Date} {vm.StartTime}").AddHours(-8);
				var EndTime = DateTime.Parse($"{vm.Date} {vm.EndTime}").AddHours(-8);

				if (bookedTime.Count != 0)
				{
					//建立空陣列 存放已被預約的時段和要存的時段
					int[] booked = new int[14];
					int[] select = new int[14];

					//選擇的日期和會議室有幾個預約時段
					int count = 0;

					int selectStart = Int32.Parse(vm.StartTime.Substring(0, 2));
					int selectEnd = Int32.Parse(vm.EndTime.Substring(0, 2));

					// 取得會議室已被預約的時段
					foreach (var time in bookedTime)
					{
						int startTiming = time.StartTime.AddHours(8).Hour;
						int endTiming = time.EndTime.AddHours(8).Hour;
						for (var i = startTiming; i <= endTiming - 1; i++)
						{
							booked[count] = i;
							count++;
						}
					}

					count = 0;
					for (int i = selectStart; i <= selectEnd - 1; i++)
					{
						select[count] = i;
						count++;
					}

					//比對booked和select時段
					var filterBookTime = booked.Where(x => x != 0).ToArray();
					var filterSeletedTime = select.Where(x => x != 0).ToArray();
					//已被預約時間回傳
					var repeatTimes = filterBookTime.Intersect(filterSeletedTime).ToArray();
					if (repeatTimes.Any())
					{
						return Json(new { success = false, message = "選取時段已被預約!" });
					}
				}
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
				return Json(new { success = true, data = "預約成功!" });
			}

		}
	}
}
