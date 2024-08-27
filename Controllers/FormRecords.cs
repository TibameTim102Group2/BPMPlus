using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BPMPlus.Controllers
{
    public class FormRecords : Controller
    {
        // GET: FormRecords
        public ActionResult Index()
        {
            return View();
        }

        // GET: FormRecords/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: FormRecords/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FormRecords/Create
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

        // GET: FormRecords/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: FormRecords/Edit/5
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

        // GET: FormRecords/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: FormRecords/Delete/5
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
