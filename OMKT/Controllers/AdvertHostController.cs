using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using OMKT.Business;
using OMKT.Context;

namespace OMKT.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class AdvertHostController : Controller
    {
        private OMKTDB db = new OMKTDB();

        //
        // GET: /AdvertHost/

        public ViewResult Index()
        {
            var adverthosts = db.AdvertHosts.Include(a => a.AdvertHostCategory).Include(a => a.Location);
            return View(adverthosts.ToList());
        }

        //
        // GET: /AdvertHost/Details/5

        public ViewResult Details(int id)
        {
            AdvertHost adverthost = db.AdvertHosts.Find(id);
            return View(adverthost);
        }

        //
        // GET: /AdvertHost/Create

        public ActionResult Create()
        {
            ViewBag.AdvertHostCategoryId = new SelectList(db.AdvertHostCategories, "AdvertHostCategoryId", "Name");
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "Latitude");
            return View();
        }

        //
        // POST: /AdvertHost/Create

        [HttpPost]
        public ActionResult Create(AdvertHost adverthost)
        {
            if (ModelState.IsValid)
            {
                db.AdvertHosts.Add(adverthost);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AdvertHostCategoryId = new SelectList(db.AdvertHostCategories, "AdvertHostCategoryId", "Name", adverthost.AdvertHostCategoryId);
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "Latitude", adverthost.LocationId);
            return View(adverthost);
        }

        //
        // GET: /AdvertHost/Edit/5

        public ActionResult Edit(int id)
        {
            AdvertHost adverthost = db.AdvertHosts.Find(id);
            ViewBag.AdvertHostCategoryId = new SelectList(db.AdvertHostCategories, "AdvertHostCategoryId", "Name", adverthost.AdvertHostCategoryId);
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "Latitude", adverthost.LocationId);
            return View(adverthost);
        }

        //
        // POST: /AdvertHost/Edit/5

        [HttpPost]
        public ActionResult Edit(AdvertHost adverthost)
        {
            if (ModelState.IsValid)
            {
                db.Entry(adverthost).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AdvertHostCategoryId = new SelectList(db.AdvertHostCategories, "AdvertHostCategoryId", "Name", adverthost.AdvertHostCategoryId);
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "Latitude", adverthost.LocationId);
            return View(adverthost);
        }

        //

        public ActionResult MapNetwork(int? campaignLocation)
        {
            return View();
        }
        public ActionResult Markers()
        {
            var markers = db.AdvertHosts.Include(a => a.Location).Include(a => a.AdvertHostCategory);
            return View("Markers", markers);
        }

        //
        // GET: /AdvertHost/Delete/5

        public ActionResult Delete(int id)
        {
            AdvertHost adverthost = db.AdvertHosts.Find(id);
            return View(adverthost);
        }

        //
        // POST: /AdvertHost/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            AdvertHost adverthost = db.AdvertHosts.Find(id);
            db.AdvertHosts.Remove(adverthost);
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