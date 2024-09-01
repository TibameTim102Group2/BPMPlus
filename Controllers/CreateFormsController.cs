using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BPMPlus.Controllers
{
    public class CreateFormsController : Controller
    {
        // GET: CreateForms
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }


    }
}
