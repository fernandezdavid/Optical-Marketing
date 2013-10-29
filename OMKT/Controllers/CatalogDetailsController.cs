using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using OMKT.Business;
using OMKT.Context;
using System.Web;

namespace OMKT.Controllers
{   
    /**
     * Manejador de vistas de Detalles de Catálogo
     */

    [Authorize]
    public class CatalogDetailsController : Controller
    {
        private readonly OMKTDB _db = new OMKTDB();

        /**
         * Vista de detalle de catálogo activo
         *
         * @since 04/04/2013
         * @param int CatalogId
         * @return Vista de detalle de catálogo
         */

        public PartialViewResult IndexByCatalog(int id)
        {
            ViewBag.AdvertId = id;
            var oCatalog = _db.Catalogs.FirstOrDefault(i => i.AdvertId == id);
            ViewBag.Catalog = oCatalog;

            var oCatalogDetails = _db.CatalogDetails.Include(i => i.Catalog).Where(i => i.AdvertId == id && i.Status == "OK");
            return PartialView("CatalogDetailsPartialList", oCatalogDetails.ToList());
        }

         /**
         * Vista carousel de detalle de catálogo activo
         *
         * @since 04/04/2013
         * @param int CatalogId
         * @return Vista carousel de detalle de catálogo
         */

        public PartialViewResult IndexByCatalogCarousel(int id)
        {
            ViewBag.AdvertId = id;
            var oCatalog = _db.Catalogs.FirstOrDefault(i => i.AdvertId == id);
            ViewBag.Catalog = oCatalog;

            var oCatalogDetails = _db.CatalogDetails.Include(i => i.Catalog).Where(i => i.AdvertId == id && i.Status == "OK");
            return PartialView("CarouselPartialList", oCatalogDetails.ToList());
        }

         /**
         * Vista ordenable de detalle de catálogo activo
         *
         * @since 04/04/2013
         * @param int CatalogId
         * @return Vista ordenable de detalle de catálogo
         */

        public PartialViewResult IndexByCatalogSortable(int id)
        {
            ViewBag.AdvertId = id;
            var oCatalog = _db.Catalogs.FirstOrDefault(i => i.AdvertId == id);
            ViewBag.Catalog = oCatalog;

            var oCatalogDetails = _db.CatalogDetails.Include(i => i.Catalog).Where(i => i.AdvertId == id && i.Status == "OK");
            return PartialView("SorteableList", oCatalogDetails.ToList());
        }

        /**
         * Vista sección detalle de catálogos
         *
         * @since 04/04/2013
         * @return Vista principal de detalle de catálogos
         * @deprecated
         */

        public ViewResult Index()
        {
            var oDetails = _db.CatalogDetails.Include(i => i.Catalog);
            return View(oDetails.ToList());
        }

        /**
         * Vista del detalle de un detalle de catálogo
         *
         * @since 04/04/2013
         * @param int id
         * @return Vista de detalle de un detalle de catálogo
         */

        public ViewResult Details(int id)
        {
            var catalogdetail = _db.CatalogDetails.Where(i => i.CatalogDetailId == id && i.Status == "OK").FirstOrDefault();
            if (catalogdetail == null) throw new HttpException(404, "The resource cannot be found");
            return View(catalogdetail);
        }

        /**
         * Vista de creación de detalle de catálogo
         *
         * @since 04/04/2013
         * @return Vista de creación de detalle de catálogo
         */

        public ActionResult Create(int? id)
        {
            ViewBag.CommercialProductId = new SelectList(_db.CommercialProducts.OrderBy(c => c.ProductName), "CommercialProductId", "ProductName");
            CatalogDetail oDetail = null;
            if (id.HasValue)
            {
                var pos = _db.CatalogDetails.Where(c => c.AdvertId == id && c.Status == "OK").Max(m => m.Position);
                pos++;
                Catalog oCatalog = _db.Catalogs.Find(id);
                if (oCatalog != null)
                {
                    oDetail = new CatalogDetail { AdvertId = id.Value, Catalog = oCatalog, Position = pos };
                }
            }
            return PartialView("Create", oDetail);
        }

        /**
         * Crea una detalle de catálogo
         *
         * @since 04/04/2013
         * @param Modelo Detalle de Catálogo
         * @return Vista de detalle de catálogo
         */

        [HttpPost]
        public ActionResult Create(CatalogDetail catalogdetail)
        {
            ViewBag.CommercialProductId = new SelectList(_db.CommercialProducts.OrderBy(c => c.ProductName), "CommercialProductId", "ProductName");
            if (ModelState.IsValid)
            {
                var check = _db.CatalogDetails.Where(a => a.CommercialProductId == catalogdetail.CommercialProductId && a.AdvertId == catalogdetail.AdvertId).FirstOrDefault();
                if (check == null)
                {
                    catalogdetail.CreatedDate = DateTime.Now;
                    catalogdetail.Status = "OK";
                    _db.CatalogDetails.Add(catalogdetail);
                    try
                    {
                        _db.SaveChanges();
                        ViewBag.Success = "El producto fue agregado exitosamente";
                    }
                    catch (Exception)
                    {
                        ViewBag.Error = "Lo sentimos, ocurrió un error mientras se procesaba la solicitud";
                    }
                }
                else if (check.Status == "DELETED")
                {
                    check.LastUpdate = DateTime.Now;
                    check.Status = "OK";
                    check.Position = catalogdetail.Position;
                    check.Discount = catalogdetail.Discount;
                    check.QRCode = catalogdetail.QRCode;
                    _db.Entry(check).State = EntityState.Modified;
                    try
                    {
                        _db.SaveChanges();
                        ViewBag.Success = "El producto fue restaurado exitosamente";
                    }
                    catch (Exception)
                    {
                        ViewBag.Error = "Lo sentimos, ocurrió un error mientras se procesaba la solicitud";
                    }
                }
                else
                {
                    ViewBag.Error = "Parece que este producto ya fue agregado!";
                }
                var oCatalog = _db.Catalogs.Find(catalogdetail.AdvertId);
                ViewBag.Catalog = oCatalog;
                return PartialView("CatalogDetailsPartialList", _db.CatalogDetails.Where(cd => cd.AdvertId == catalogdetail.AdvertId && cd.Status == "OK").Include(i => i.CommercialProduct));
            }
            return PartialView("Create", catalogdetail);
        }

        /**
        * Vista de edición de detalle de catálogo
        *
        * @since 04/04/2013
        * @param int id
        * @return Vista de edición de detalle de catálogo
        */

        public ActionResult Edit(int id)
        {
            var catalogdetail = _db.CatalogDetails.Where(i => i.CatalogDetailId == id && i.Status == "OK").FirstOrDefault();
            if (catalogdetail == null) throw new HttpException(404, "The resource cannot be found");
            ViewBag.CommercialProductId = new SelectList(_db.CommercialProducts.Where(c => c.Status == "OK"), "CommercialProductId", "ProductName", catalogdetail.CommercialProductId);
            return PartialView(catalogdetail);
        }

        /**
        * Edita un detalle de catálogo
        *
        * @since 04/04/2013
        * @param Modelo Detalle de Catálogo
        * @return Vista de edición de detalle de catálogo
        */

        [HttpPost]
        public ActionResult Edit(CatalogDetail catalogdetail)
        {
            ViewBag.CommercialProductId = new SelectList(_db.CommercialProducts, "CommercialProductId", "ProductName", catalogdetail.CommercialProductId);
            if (ModelState.IsValid)
            {
                var oCatalog = _db.Catalogs.Find(catalogdetail.AdvertId);
                ViewBag.Catalog = oCatalog;
                catalogdetail.LastUpdate = DateTime.Now;
                _db.Entry(catalogdetail).State = EntityState.Modified;                
                try
                {
                    _db.SaveChanges();
                    ViewBag.Success = "El producto fue editado exitosamente";
                }
                catch (Exception)
                {
                    ViewBag.Error = "Lo sentimos, ocurrió un error mientras se procesaba la solicitud";
                }

                return PartialView("CatalogDetailsPartialList", _db.CatalogDetails.Where(cd => cd.AdvertId == catalogdetail.AdvertId && cd.Status == "OK").Include(i => i.CommercialProduct).ToList());
            }
            return PartialView("Edit", catalogdetail);
        }

        /**
         * Vista de borrado lógico de detalle de catálogo
         *
         * @since 04/04/2013
         * @param int id
         * @return Vista de borrado de detalle de catálogo
         */

        public ActionResult Delete(int id)
        {
            var catalogdetail = _db.CatalogDetails.FirstOrDefault(c => c.CatalogDetailId == id && c.Status == "OK");
            if (catalogdetail == null) throw new HttpException(404, "The resource cannot be found");
            return PartialView(catalogdetail);
        }

        /**
        * Borra lógicamente un detalle de catálogo
        *
        * @since 04/04/2013
        * @param int id
        * @return Mensaje de confirmación
        */

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var catalogdetail = _db.CatalogDetails.FirstOrDefault(c => c.CatalogDetailId == id && c.Status == "OK");
            if (catalogdetail != null)
            {
                catalogdetail.Status = "DELETED";
                catalogdetail.LastUpdate = DateTime.Now;
                _db.Entry(catalogdetail).State = EntityState.Modified;
                try
                {
                    ViewBag.Success = "El producto fue quitado exitosamente!";
                    _db.SaveChanges();
                }
                catch (Exception)
                {
                    ViewBag.Error = "Lo sentimos, ocurrió un error mientras se procesaba la solicitud.";
                }

            }
            else { ViewBag.Error = "Lo sentimos, no pudimos encontrar el detalle."; }
            return PartialView("CatalogDetailsPartialList", _db.CatalogDetails.Include(i => i.Catalog).Where(i => i.AdvertId == catalogdetail.AdvertId && i.Status == "OK"));            
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}