using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using OMKT.Business;
using OMKT.Context;

namespace OMKT.Controllers
{
    [Authorize]
    public class CatalogDetailsController : Controller
    {
        private readonly OMKTDB _db = new OMKTDB();

        //Custom
        public PartialViewResult IndexByCatalog(int id)
        {
            ViewBag.AdvertId = id;
            var oCatalog = _db.Catalogs.FirstOrDefault(i => i.AdvertId == id);
            ViewBag.Catalog = oCatalog;

            var oCatalogDetails = _db.CatalogDetails.Include(i => i.Catalog).Where(i => i.AdvertId == id);
            return PartialView("CatalogDetailsPartialList", oCatalogDetails.ToList());
        }

        public PartialViewResult IndexByCatalogCarousel(int id)
        {
            ViewBag.AdvertId = id;
            var oCatalog = _db.Catalogs.FirstOrDefault(i => i.AdvertId == id);
            ViewBag.Catalog = oCatalog;

            var oCatalogDetails = _db.CatalogDetails.Include(i => i.Catalog).Where(i => i.AdvertId == id);
            return PartialView("CarouselPartialList", oCatalogDetails.ToList());
        }

        public PartialViewResult IndexByCatalogSortable(int id)
        {
            ViewBag.AdvertId = id;
            var oCatalog = _db.Catalogs.FirstOrDefault(i => i.AdvertId == id);
            ViewBag.Catalog = oCatalog;

            var oCatalogDetails = _db.CatalogDetails.Include(i => i.Catalog).Where(i => i.AdvertId == id);
            return PartialView("SorteableList", oCatalogDetails.ToList());
        }

        // GET: /AdvertDetails/

        public ViewResult Index()
        {
            var oDetails = _db.CatalogDetails.Include(i => i.Catalog);
            return View(oDetails.ToList());
        }

        //
        // GET: /AdvertDetails/Details/5

        public ViewResult Details(int id)
        {
            CatalogDetail catalogdetail = _db.CatalogDetails.Find(id);
            return View(catalogdetail);
        }

        //
        // GET: /AdvertDetails/Create
        //Optional

        public ActionResult Create(int? id)
        {
            ViewBag.CommercialProductId = new SelectList(_db.CommercialProducts.OrderBy(c => c.ProductName), "CommercialProductId", "ProductName");
            CatalogDetail oDetail = null;
            if (id.HasValue)
            {
                Catalog oCatalog = _db.Catalogs.Find(id);// (from cat in _db.Catalogs where cat.AdvertId == id select cat).FirstOrDefault();
                if (oCatalog != null)
                {
                    oDetail = new CatalogDetail { AdvertId = id.Value, Catalog = oCatalog };
                }
                //ViewBag.AdvertId = new SelectList(_db.Catalogs, "AdvertId", "CatalogName", id.Value);
            }
            return PartialView("Create", oDetail);
        }

        //
        // POST: /AdvertDetails/Create

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
                    _db.CatalogDetails.Add(catalogdetail);
                    try
                    {
                        _db.SaveChanges();
                    }
                    catch (Exception)
                    {
                        //TODO
                    }
                }
                else
                {
                    //TODO
                }
                var oCatalog = _db.Catalogs.Find(catalogdetail.AdvertId);// (from cat in _db.Catalogs where cat.AdvertId == catalogdetail.AdvertId select cat).FirstOrDefault();
                ViewBag.Catalog = oCatalog;

                return PartialView("CatalogDetailsPartialList", _db.CatalogDetails.Where(cd => cd.AdvertId == catalogdetail.AdvertId).Include(i => i.CommercialProduct));
            }
            return PartialView("Create", catalogdetail);
        }

        //
        // GET: /AdvertDetails/Edit/5

        public ActionResult Edit(int id)
        {
            CatalogDetail catalogdetail = _db.CatalogDetails.Find(id);
            ViewBag.CommercialProductId = new SelectList(_db.CommercialProducts, "CommercialProductId", "ProductName", catalogdetail.CommercialProductId);
            return PartialView(catalogdetail);
        }

        //
        // POST: /AdvertDetails/Edit/5

        [HttpPost]
        public ActionResult Edit(CatalogDetail catalogdetail)
        {
            ViewBag.CommercialProductId = new SelectList(_db.CommercialProducts, "CommercialProductId", "ProductName", catalogdetail.CommercialProductId);
            if (ModelState.IsValid)
            {
                var oCatalog = _db.Catalogs.Find(catalogdetail.AdvertId);
                catalogdetail.LastUpdate = DateTime.Now;
                _db.Entry(catalogdetail).State = EntityState.Modified;
                ViewBag.Catalog = oCatalog;
                try
                {
                    _db.SaveChanges();
                }
                catch (Exception)
                {
                    //TODO
                }

                return PartialView("CatalogDetailsPartialList", _db.CatalogDetails.Where(cd => cd.AdvertId == catalogdetail.AdvertId).Include(i => i.CommercialProduct).ToList());
            }
            return PartialView("Edit", catalogdetail);
        }

        //
        // GET: /AdvertDetails/Delete/5

        public ActionResult Delete(int id)
        {
            CatalogDetail catalogdetail = _db.CatalogDetails.Find(id);
            return PartialView(catalogdetail);
        }

        //
        // POST: /AdvertDetails/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            CatalogDetail catalogdetail = _db.CatalogDetails.Find(id);
            if (catalogdetail != null)
            {
                _db.CatalogDetails.Remove(catalogdetail);
                try
                {
                    _db.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }

                return PartialView("CatalogDetailsPartialList", _db.CatalogDetails.Where(cd => cd.AdvertId == catalogdetail.AdvertId).Include(i => i.CommercialProduct));
            }
            return Content("El registro no fue encontrado.");
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}