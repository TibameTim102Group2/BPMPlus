using BPMPlus.Data;
using BPMPlus.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BPMPlus.Controllers
{
    
    public class CreateFormsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CreateFormsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<User> GetAuthorizedUser()
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserId")?.Value;

            var user = await _context.User.FirstOrDefaultAsync(m => m.UserId == userId && m.UserIsActive == true);
            if (user == null)
            {
                throw new Exception("User is null, Server Error");
            }
            return user;
        }
        // GET: CreateForms
        [Authorize]

        public async Task<ActionResult> Index()
        {
            
            
            return View();
        }


    }
}
