using BPMPlus.Data;
using BPMPlus.Models;
using Microsoft.AspNetCore.Mvc;
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
            var allrooms= await _context.MeetingRooms.AsNoTracking().Select(n=>n.MeetingRoomId).ToListAsync();
            ViewBag.allRooms = JsonConvert.SerializeObject(allrooms);
            //var allPermission = _context.PermissionGroup.Select(p => new
            //{
            //    Value = p.PermissionGroupId,
            //    Text = p.PermissionGroupName,
            //}).ToList();

            return View();
        }
        [HttpGet]
        public async Task<ActionResult> getMeetingBook(string id)
        {
            //找出所當日有被預約的時間
            var hasBookedData = await _context.Meeting.AsNoTracking().Where(m => m.StartTime.Date.ToString() == id)
                .Select(n => new
                {
                    meetingRoomId = n.MeetingRoomId,
                    meetingHost = _context.User.Where(u => u.UserId == n.MeetingHost).Select(n => n.UserName).FirstOrDefault(),
                    startTime = n.StartTime.AddHours(8).Hour,
                    endTime = n.EndTime.AddHours(8).Hour,
                    note = n.Note,
                })
                .ToListAsync();
            //選取只需要的資料

            if (!hasBookedData.Any())
            {
                return Json(new { success = false, message = "這日期沒有任何會議室有被預約" });
            }
            return Json(new { success = true, data = hasBookedData });
        }



    
    }
}
