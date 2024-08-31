using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BPMPlus.Controllers
{
    public class QueryFormsController : Controller
    {
        // GET: QueryForms
        public ActionResult Index()
        {
            return View();
        }
    }
}
