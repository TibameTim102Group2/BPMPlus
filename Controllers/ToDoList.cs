using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BPMPlus.Controllers
{
    public class ToDoList : Controller
    {
        // GET: ToDoList
        public ActionResult Index()
        {
            return View();
        }

       
    }
}
