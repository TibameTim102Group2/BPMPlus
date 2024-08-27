using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BPMPlus.Controllers
{
    public class CreateForms : Controller
    {
        // GET: CreateForms
        public ActionResult Index()
        {
            return View();
        }

        // GET: CreateForms/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CreateForms/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CreateForms/Create
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

        // GET: CreateForms/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CreateForms/Edit/5
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

        // GET: CreateForms/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CreateForms/Delete/5
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
