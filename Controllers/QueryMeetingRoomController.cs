using BPMPlus.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BPMPlus.Controllers
{
    public class QueryMeetingRoomController : BaseController
    {
        private readonly ApplicationDbContext _context;

    public QueryMeetingRoomController(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public IActionResult Index()
        {
            return View();
        }
    }
}
