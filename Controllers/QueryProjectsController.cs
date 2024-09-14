using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BPMPlus.Controllers
{
    public class QueryProjectsController : Controller
    {
        // GET: QueryProjectsController
        public ActionResult Index()
        {
            return View();
        }

        // GET: QueryProjectsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: QueryProjectsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: QueryProjectsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: QueryProjectsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: QueryProjectsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: QueryProjectsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: QueryProjectsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
