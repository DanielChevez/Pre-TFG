using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace Medical_Records_UNA.Areas.MedicalConditions.Controllers
{
    [Area("MedicalConditions")]
    [Authorize]
    public class SurgeryController : Controller
    {
        // GET: SurgeryController
        public ActionResult Index()
        {
            return View();
        }

        // GET: SurgeryController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SurgeryController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SurgeryController/Create
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

        // GET: SurgeryController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SurgeryController/Edit/5
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

        // GET: SurgeryController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SurgeryController/Delete/5
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
