using BPMPlus.Data;
using BPMPlus.Models;
using BPMPlus.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Globalization;
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

		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
		public async Task<ActionResult> editMeetingBook(string id)
		{
			//找此筆資料的成員

			var userWithMeeting = _context.Meeting
			.Include(u => u.Users)
			 .FirstOrDefault(u => u.MeetingId == id);
			var meetingMember= userWithMeeting.Users.Select(u => new
			{
				UserId = u.UserId,
				UserName = u.UserName,
				DepartmentId = u.DepartmentId,
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
		//進行編輯資料
		[HttpPost]
		[Authorize]
		public async Task<ActionResult> editMeetingBook([FromBody] BookingMeetingRoomEditVM editData)
		{
			//先建立一個陣列放 已經被預定的時間
			List<int> bookedTime = new List<int>();
			List<int> selectTime = new List<int>();
			//把時間轉成int
			int selectStart = Int32.Parse(editData.StartTime.Substring(0,2));
			int selectEnd = Int32.Parse(editData.EndTime.Substring(0, 2));

			//要時間更改的判斷
			//要比較的對像是 在所選取的日期下 特定的會議室 內部有沒有重疊的時間
			var meetingTimeRepeat =await _context.Meeting.Where(n => n.StartTime.AddHours(8).Date == DateTime.Parse(editData.Date) &&
			n.MeetingRoomId==editData.MeetingRoom && n.MeetingId !=editData.MeetingId
			).ToListAsync();

            //要先把同一日且同一會議室 不同預約的 時間做成 一個區段
            foreach (var item in meetingTimeRepeat)
            {

				int startTiming = item.StartTime.AddHours(8).Hour;
				int endTiming = item.EndTime.AddHours(8).Hour;
				for (int i = startTiming; i < endTiming; i++)
				{

					bookedTime.Add(i);
				}
            }
			//先把前端選的時間轉成陣列

			for(int i = selectStart; i < selectEnd; i++)
			{
				selectTime.Add(i);
			}
			//要每一筆去比對時段
			//找出兩者的交集
			var repeatTimes = bookedTime.Intersect(selectTime).ToArray();
			if (repeatTimes.Any())
			{
				return Json(new { success=false,message="此時段已被預約"});
			}

			//先把時間合體成日期與時間
			string startTimeString = $"{editData.Date} {editData.StartTime}";
			string endTimeString = $"{editData.Date} {editData.EndTime}";
			//找到當筆資料
			var meetingEdit = _context.Meeting.Include(x => x.Users).FirstOrDefault(x => x.MeetingId == editData.MeetingId);
			if (ModelState.IsValid)
			{
				try
				{
					meetingEdit.MeetingRoomId = editData.MeetingRoom;
					meetingEdit.Note = editData.Note;
					meetingEdit.StartTime = DateTime.Parse(startTimeString).AddHours(-8);
					meetingEdit.EndTime = DateTime.Parse(endTimeString).AddHours(-8);
					meetingEdit.UpdatedTime = DateTime.UtcNow;
					//撈出現在所有的會議成員
					var NowMeetingMembers = meetingEdit.Users.ToList();
					
					//先把關連表全刪
					foreach (var item in NowMeetingMembers)
					{

						if (item != null)
						{
							meetingEdit.Users.Remove(item); //刪除關連
							_context.SaveChanges();
						}
					}

					//當不為空的話
					//全部重新新增
					//建立會議與人員的關聯
					foreach (var item in editData.MeetingMembers)
					{

						//重新建關聯
						var newMeetingMembers = _context.User.FirstOrDefault(pg => pg.UserId == item.UserId);
						if (newMeetingMembers != null)
						{
							meetingEdit.Users.Add(newMeetingMembers);
							_context.SaveChanges();
						}
					}



					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if ((editData.MeetingId ==null))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
			}
			return Json(new { success = true, message = "修改預約成功" });
		}


	}
}
