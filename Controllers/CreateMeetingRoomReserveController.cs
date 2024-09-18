using BPMPlus.Data;
using Microsoft.AspNetCore.Mvc;

namespace BPMPlus.Controllers
{
    public class CreateMeetingRoomReserveController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public CreateMeetingRoomReserveController(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
