using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using OMKT.Business;
using OMKT.Context;

namespace OMKT.Controllers
{
    /**
     * Manejador de las vistas de anuncios
     */
    public class AdvertController : Controller
    {
        private OMKTDB db = new OMKTDB();

        /**
         * Vista listado de anuncios
         * 
         * @since 04/04/2013
         * @return Vista listado de anuncios
         */

        public ViewResult Index()
        {
            var adverts = db.Adverts.Include(a => a.AdvertType).Include(a => a.AdvertState);
            return View(adverts.ToList());
        }

        /**
         * Vista de detalle de anuncio
         * ordenado por fecha
         * 
         * @since 04/04/2013
         * @param int id
         * @return Vista de detalle de anuncio
         */

        public ViewResult Details(int id)
        {
            Advert advert = db.Adverts.Find(id);
            return View(advert);
        }

        /**
          * Vista de creación de anuncio
          * 
          * @since 04/04/2013
          * @return Vista de creación de anuncio
          */

        public ActionResult Create()
        {
            ViewBag.AdvertTypeId = new SelectList(db.AdvertTypes, "AdvertTypeId", "Description");
            ViewBag.AdvertStateId = new SelectList(db.AdvertStates, "AdvertstateId", "Description");
            return View();
        }

        /**
         * Crea un anuncio
         * 
         * @since 04/04/2013
         * @param Modelo Anuncio
         * @return Vista edición de anuncio
         */

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

        /**
         * Vista de edición de anuncio
         * 
         * @since 04/04/2013
         * @param int id
         * @return Vista edición de anuncio
         */

        public ActionResult Edit(int id)
        {
            Advert advert = db.Adverts.Find(id);
            ViewBag.AdvertTypeId = new SelectList(db.AdvertTypes, "AdvertTypeId", "Description", advert.AdvertTypeId);
            ViewBag.AdvertStateId = new SelectList(db.AdvertStates, "AdvertstateId", "Description", advert.AdvertStateId);
            return View(advert);
        }

        /**
         * Edita un anuncio
         * 
         * @since 04/04/2013
         * @param Modeo anuncio
         * @return Vista edición de anuncio
         */

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

        /**
          * Vista borrado lógico de anuncio
          * 
          * @since 04/04/2013
          * @param int id
          * @return Vista borrado lógico de anuncio
          */

        public ActionResult Delete(int id)
        {
            Advert advert = db.Adverts.Find(id);
            return View(advert);
        }

        /**
         * Borra lógicamente un anuncio
         * 
         * @since 04/04/2013
         * @param int id
         * @return Mensaje de confirmación
         */

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