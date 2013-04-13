using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using OMKT.Business;
using OMKT.Context;

namespace OMKT.Controllers
{
    public class AdvertController : Controller
    {
        private OMKTDB db = new OMKTDB();

        //
        // GET: /Advert/

        public ViewResult Index()
        {
            var adverts = db.Adverts.Include(a => a.AdvertType).Include(a => a.AdvertState);
            return View(adverts.ToList());
        }

        //
        // GET: /Advert/Details/5

        public ViewResult Details(int id)
        {
            Advert advert = db.Adverts.Find(id);
            return View(advert);
        }

        //
        // GET: /Advert/Create

        public ActionResult Create()
        {
            ViewBag.AdvertTypeId = new SelectList(db.AdvertTypes, "AdvertTypeId", "Description");
            ViewBag.AdvertStateId = new SelectList(db.AdvertStates, "AdvertstateId", "Description");
            return View();
        }

        //
        // POST: /Advert/Create

        [HttpPost]
        public ActionResult Create(Advert advert)
        {
            if (ModelState.IsValid)
            {
                db.Adverts.Add(advert);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AdvertTypeId = new SelectList(db.AdvertTypes, "AdvertTypeId", "Description", advert.AdvertTypeId);
            ViewBag.AdvertStateId = new SelectList(db.AdvertStates, "AdvertstateId", "Description", advert.AdvertStateId);
            return View(advert);
        }

        //
        // GET: /Advert/Edit/5

        public ActionResult Edit(int id)
        {
            Advert advert = db.Adverts.Find(id);
            ViewBag.AdvertTypeId = new SelectList(db.AdvertTypes, "AdvertTypeId", "Description", advert.AdvertTypeId);
            ViewBag.AdvertStateId = new SelectList(db.AdvertStates, "AdvertstateId", "Description", advert.AdvertStateId);
            return View(advert);
        }

        //
        // POST: /Advert/Edit/5

        [HttpPost]
        public ActionResult Edit(Advert advert)
        {
            if (ModelState.IsValid)
            {
                db.Entry(advert).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AdvertTypeId = new SelectList(db.AdvertTypes, "AdvertTypeId", "Description", advert.AdvertTypeId);
            ViewBag.AdvertStateId = new SelectList(db.AdvertStates, "AdvertstateId", "Description", advert.AdvertStateId);
            return View(advert);
        }

        //
        // GET: /Advert/Delete/5

        public ActionResult Delete(int id)
        {
            Advert advert = db.Adverts.Find(id);
            return View(advert);
        }

        //
        // POST: /Advert/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Advert advert = db.Adverts.Find(id);
            db.Adverts.Remove(advert);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}