using BPMPlus.Data;
using BPMPlus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;

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
            User user = await GetAuthorizedUser();
            ViewBag.nowUserId = user.UserId;
            var allrooms= await _context.MeetingRooms.AsNoTracking().Select(n=>n.MeetingRoomId).ToListAsync();
            ViewBag.allRooms = JsonConvert.SerializeObject(allrooms);
            return View();
        }
        [HttpGet]
        public async Task<ActionResult> getMeetingBook(string id)
        {
            //找出所當日有被預約的時間
            var hasBookedData = await _context.Meeting.AsNoTracking().Where(m => m.StartTime.Date.ToString() == id)
                .Select(n => new
                {
                    meetingId=n.MeetingId,
                    meetingRoomId = n.MeetingRoomId,
                    meetingHost = _context.User.Where(u => u.UserId == n.MeetingHost).Select(n => n.UserName).FirstOrDefault(),
                    startTime = n.StartTime.AddHours(8).Hour,
                    endTime = n.EndTime.AddHours(8).Hour,
                    note = n.Note,
                    meetingHostUserId=_context.User.Where(u=>u.UserId ==n.MeetingHost).Select(u=>u.UserId).FirstOrDefault(),
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
            var editMeetingId = await _context.Meeting.Where(m => m.MeetingId == id).
                Select(m => new
                {
                    MeetingRoom = m.MeetingRoomId,
                    Note = m.Note,
                    startTime=m.StartTime,
                    endTime=m.EndTime,
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
