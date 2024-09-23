using BPMPlus.Data;
using BPMPlus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;
using System.Text.Json;

namespace BPMPlus.Controllers
{
	public class CurrentMeetingRoomController : BaseController
	{
		private readonly ApplicationDbContext _context;

		public CurrentMeetingRoomController(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			//判斷是誰登入
			User user = await GetAuthorizedUser();
			ViewBag.nowUserId = user.UserId;
			var allrooms = await _context.MeetingRooms.AsNoTracking().Select(n => n.MeetingRoomId).ToListAsync();
			ViewBag.allRooms = JsonConvert.SerializeObject(allrooms);
			var roomsInfo = await _context.MeetingRooms.AsNoTracking().Select(m => new
			{
				RoomId = m.MeetingRoomId,
				Capacity = m.Accommodation
			}).ToListAsync();
			ViewBag.roomsInfo = JsonConvert.SerializeObject(roomsInfo); ;

			//把所有員工放入
			//把字典放入
			var departments = await _context.User
			.Include(u => u.Department)
			.GroupBy(u => u.Department.DepartmentName)
			.ToDictionaryAsync(g => g.Key,
			g => g.Select(u => new { u.UserId,u.UserName,u.DepartmentId }).ToList());

			string employees = JsonConvert.SerializeObject(departments);

			ViewBag.Employees = employees;



			return View();
		}
		[HttpGet]
		public async Task<ActionResult> getMeetingBook(string id)
		{
			//找出所有當日有被預約的時間
			var hasBookedData = await _context.Meeting.AsNoTracking().Where(m => m.StartTime.Date.ToString() == id)
				.Select(n => new
				{
					meetingId = n.MeetingId,
					meetingRoomId = n.MeetingRoomId,
					meetingHost = _context.User.Where(u => u.UserId == n.MeetingHost).Select(n => n.UserName).FirstOrDefault(),
					startTime = n.StartTime.AddHours(8).Hour,
					endTime = n.EndTime.AddHours(8).Hour,
					note = n.Note,
					meetingHostUserId = _context.User.Where(u => u.UserId == n.MeetingHost).Select(u => u.UserId).FirstOrDefault(),
				})
				.ToListAsync();
			//選取只需要的資料

			if (!hasBookedData.Any())
			{
				return Json(new { success = false, message = "這日期沒有任何會議室有被預約" });
			}
			return Json(new { success = true, data = hasBookedData });
		}
		//刪除預約
		[HttpGet]
		public async Task<ActionResult> deleteMeetingBook(string id)
		{
			var deleteMeetingId = await _context.Meeting.Where(m => m.MeetingId == id).FirstOrDefaultAsync();
			if (deleteMeetingId == null)
			{
				return Json(new { success = false, data = "此筆預約不存在" });
			}
			_context.Meeting.Remove(deleteMeetingId);
			await _context.SaveChangesAsync();

			return Json(new { success = true, data = "刪除預約成功" });
		}
		//取得編輯資料
		[HttpGet]
		public async Task<ActionResult> editMeetingBook(string id)
		{
			//找此筆資料的成員

			var userWithMeeting = _context.Meeting
			.Include(u => u.Users)
			 .FirstOrDefault(u => u.MeetingId == id);
			var meetingMember= userWithMeeting.Users.Select(u => new
			{
				Value = u.UserId,
				Text = u.UserName,
				DepartmentName=_context.Department.Where(d=>d.DepartmentId==u.DepartmentId).FirstOrDefault()?.DepartmentName,
			}).ToList();


			var editMeetingId = await _context.Meeting.Where(m => m.MeetingId == id).
				Select(m => new
				{
					MeetingId = m.MeetingId,
					MeetingRoom = m.MeetingRoomId,
					Note = m.Note,
					startTime = m.StartTime.AddHours(8).ToString("HH:mm"),
					endTime = m.EndTime.AddHours(8).ToString("HH:mm"),
					Date = m.StartTime.Date.ToString("yyyy-MM-dd"),
					meetingMembers = meetingMember,
				})
				.FirstAsync();

			if (editMeetingId == null)
			{
				return Json(new { success = false, data = "此筆預約不存在" });
			}
			return Json(new { success = true, data = editMeetingId });
		}



	}
}
