using BPMPlus.Data;
using BPMPlus.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BPMPlus.Controllers
{
    public class BaseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BaseController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize]
        public async Task<User> GetAuthorizedUser()
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserId")?.Value;

            var user = await _context.User
                .Include(u => u.PermissionGroups)
                .ThenInclude(pg => pg.UserActivities)
                .Include(u => u.Projects)         // 加載 Projects
                .Include(u => u.Meetings)
                .FirstOrDefaultAsync(m => m.UserId == userId && m.UserIsActive == true);
            
            if (user == null)
            {
                throw new Exception("Server error, User is null");
            }
            return user;
        }
        
    }
}
