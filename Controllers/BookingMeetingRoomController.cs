﻿using BPMPlus.Data;
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
            var accommdation = _context.MeetingRooms
                .Where(n => n.MeetingRoomId == id)
                .Select(n => n.Accommodation);

            return Json(new { success=true, data=accommdation });
        }

        //確認預約狀況
        public async Task<IActionResult> CheakMeetingRooms(string RoomId, string BookingDate)
        {
            var bookedTimes = await _context.Meeting
                .Where(b=>b.MeetingRoomId == RoomId && b.StartTime.Date == DateTime.Parse(BookingDate).Date)
                .Select(b=>new {
                    StartTime = b.StartTime.AddHours(8).ToString("HH:mm"),
                    EndTime = b.EndTime.AddHours(8).ToString("HH:mm")
                })
                .ToListAsync();
            return Json(new { success = true, data = bookedTimes });
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
                meeting.StartTime = DateTime.ParseExact(vm.StartDate, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
                meeting.EndTime = DateTime.ParseExact(vm.EndDate, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
                meeting.MeetingHost = host;
                meeting.Note = vm.Note;
                meeting.CreatedTime = DateTime.Now;
                meeting.UpdatedTime = DateTime.Now;

                await _context.Meeting.AddAsync(meeting);
                await _context.SaveChangesAsync();

            }
            var meetingid = _context.Meeting.Include(x => x.Users).FirstOrDefault(x => x.MeetingId == id[0]);

            foreach (var item in vm.Members)
            {

                //重新建關聯
                var newMeetingMember = _context.User.FirstOrDefault(pg => pg.UserId == item);
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
