using BPMPlus.Data;
using BPMPlus.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BPMPlus.Controllers
{
    public class QueryProjectsController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public QueryProjectsController(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        // GET: QueryProjectsController
        public IActionResult Index()
        {
            return View();
        }

        //GET: QueryProjects/IndexJson
        public async Task<JsonResult> IndexJson() 
        {

            
            
            //Project相關資訊
            var tableData = await _context.Project
                 .Include(c => c.Users)
                 .AsNoTracking()
                 .AsSplitQuery()
                 .Select(c => new QueryProjectsViewModel
                 {
                     ProjectId=c.ProjectId,
                     ProjectName=c.ProjectName,
                     Summary=c.Summary,
                     DeadLine=c.DeadLine,
                     ProjectManager = _context.User.FirstOrDefault(n=>n.UserId==c.ProjectManagerId).UserName
                 }).ToListAsync();

            



            return Json(tableData);
        }
    }
}
