using BPMPlus.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public JsonResult IndexJson() 
        {
            return Json(_context.Project);
        }
    }
}
