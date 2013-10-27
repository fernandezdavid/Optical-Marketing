using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using OMKT.Business;
using OMKT.Context;

namespace OMKT.Controllers
{
    /**
     * Manejador de las vistas de Puntos de difusion
     */
    [Authorize(Roles = "Administrador")]
    public class AdvertHostController : Controller
    {
        private OMKTDB db = new OMKTDB();

       /**
         * Vista listado de puntos de difusion
         * 
         * @since 04/04/2013
         * @return Vista listado de puntos de difusion
         */

        public ViewResult Index()
        {
            var adverthosts = db.AdvertHosts.Include(a => a.AdvertHostCategory).Include(a => a.Location);
            return View(adverthosts.ToList());
        }

        /**
         * Vista de detalle de punto de difusion
         * 
         * @since 04/04/2013
         * @param int id
         * @return Vista de detalle de punto de difusion
         */

        public ViewResult Details(int id)
        {
            AdvertHost adverthost = db.AdvertHosts.Find(id);
            return View(adverthost);
        }

        /**
          * Vista de creación de punto de difusion
          * 
          * @since 04/04/2013
          * @return Vista de creación de punto de difusion
          */

        public ActionResult Create()
        {
            ViewBag.AdvertHostCategoryId = new SelectList(db.AdvertHostCategories, "AdvertHostCategoryId", "Name");
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "Latitude");
            return View();
        }

        /**
         * Crea un punto de difusion
         * 
         * @since 04/04/2013
         * @param Modelo Punto de difusion
         * @return Vista edición de punto de difusion
         */

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

        /**
         * Vista de edición de punto de difusion
         * 
         * @since 04/04/2013
         * @param int id
         * @return Vista edición de punto de difusion
         */

        public ActionResult Edit(int id)
        {
            AdvertHost adverthost = db.AdvertHosts.Find(id);
            ViewBag.AdvertHostCategoryId = new SelectList(db.AdvertHostCategories, "AdvertHostCategoryId", "Name", adverthost.AdvertHostCategoryId);
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "Latitude", adverthost.LocationId);
            return View(adverthost);
        }

        /**
         * Edita un punto de difusion
         * 
         * @since 04/04/2013
         * @param Modeo punto de difusion
         * @return Vista edición de punto de difusion
         */

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

        /**
         * Vista de puntos de difusion en mapa
         * 
         * @since 04/04/2013
         * @param int id
         * @return Vista de puntos de difusion en mapa
         */

        public ActionResult MapNetwork(int? campaignLocation)
        {
            return View();
        }

        /**
         * Vista de marcadores en mapa correspondiente a puntos de difusion
         * 
         * @since 04/04/2013
         * @param int id
         * @return Vista de mapa de puntos de difusion
         */

        public ActionResult Markers()
        {
            var markers = db.AdvertHosts.Include(a => a.Location).Include(a => a.AdvertHostCategory);
            return View("Markers", markers);
        }

        /**
          * Vista borrado lógico de punto de difusion
          * 
          * @since 04/04/2013
          * @param int id
          * @return Vista borrado lógico de punto de difusion
          */

        public ActionResult Delete(int id)
        {
            AdvertHost adverthost = db.AdvertHosts.Find(id);
            return View(adverthost);
        }

        /**
         * Borra lógicamente un punto de difusion
         * 
         * @since 04/04/2013
         * @param int id
         * @return Mensaje de confirmación
         */

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