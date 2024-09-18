using BPMPlus.Data;
using Microsoft.AspNetCore.Mvc;

namespace BPMPlus.Controllers
{
    public class CurrentMeetingRoomController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public CurrentMeetingRoomController(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
