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
            return PartialView("Index", oCatalogDetails.ToList());
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
            AdvertDetail catalogdetail = _db.CatalogDetails.Find(id);
            return View(catalogdetail);
        }

        //
        // GET: /AdvertDetails/Create
        //Optional

        public ActionResult Create(int? id)
        {
            //ViewBag.AdvertId = new SelectList(_db.Catalogs, "AdvertId", "CatalogName");
            ViewBag.CommercialProductId = new SelectList(_db.CommercialProducts.OrderBy(c => c.ProductName), "CommercialProductId", "ProductName");
            AdvertDetail oDetail = null;
            if (id.HasValue)
            {
                Catalog oCatalog = (from cat in _db.Catalogs where cat.AdvertId == id select cat).FirstOrDefault();
                if (oCatalog != null)
                {
                    oDetail = new AdvertDetail { AdvertId = id.Value, Catalog = oCatalog };
                }
                //ViewBag.AdvertId = new SelectList(_db.Catalogs, "AdvertId", "CatalogName", id.Value);
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("Create", oDetail);
            }
            return View("Create", oDetail);
        }

        //
        // POST: /AdvertDetails/Create

        [HttpPost]
        public ActionResult Create(AdvertDetail catalogdetail)
        {
            ViewBag.CommercialProductId = new SelectList(_db.CommercialProducts.OrderBy(c => c.ProductName), "CommercialProductId", "ProductName");
            if (ModelState.IsValid)
            {
                catalogdetail.CreatedDate = DateTime.Now;
                _db.CatalogDetails.Add(catalogdetail);
                _db.SaveChanges();

                var oCatalog = (from cat in _db.Catalogs where cat.AdvertId == catalogdetail.AdvertId select cat).FirstOrDefault();
                ViewBag.Catalog = oCatalog;

                return PartialView("Index", _db.CatalogDetails.Where(cd => cd.AdvertId == catalogdetail.AdvertId).Include(i => i.CommercialProduct));
            }
            //ViewBag.AdvertId = new SelectList(_db.Catalogs, "AdvertId", "CatalogName", catalogdetail.AdvertId);
            Response.StatusCode = 400;
            return PartialView("Create", catalogdetail);
        }

        //
        // GET: /AdvertDetails/Edit/5

        public ActionResult Edit(int id)
        {
            AdvertDetail catalogdetail = _db.CatalogDetails.Find(id);
            ViewBag.CommercialProductId = new SelectList(_db.CommercialProducts, "CommercialProductId", "ProductName", catalogdetail.CommercialProductId);
            return PartialView(catalogdetail);
        }

        //
        // POST: /AdvertDetails/Edit/5

        [HttpPost]
        public ActionResult Edit(AdvertDetail catalogdetail)
        {
            ViewBag.CommercialProductId = new SelectList(_db.CommercialProducts, "CommercialProductId", "ProductName", catalogdetail.CommercialProductId);
            if (ModelState.IsValid)
            {
                var oCatalog = _db.Catalogs.Find(catalogdetail.AdvertId);
                oCatalog.LastUpdate = DateTime.Now;
                _db.Entry(oCatalog).State = EntityState.Modified;
                catalogdetail.LastUpdate = DateTime.Now;
                _db.Entry(catalogdetail).State = EntityState.Modified;
                _db.SaveChanges();
                ViewBag.Catalog = oCatalog;

                return PartialView("Index", _db.CatalogDetails.Where(cd => cd.AdvertId == catalogdetail.AdvertId).Include(i => i.CommercialProduct).ToList());
            }
            ViewBag.AdvertId = new SelectList(_db.Catalogs, "AdvertId", "CatalogName", catalogdetail.AdvertId);
            Response.StatusCode = 400;
            return PartialView("Edit", catalogdetail);
        }

        //
        // GET: /AdvertDetails/Delete/5

        public ActionResult Delete(int id)
        {
            AdvertDetail catalogdetail = _db.CatalogDetails.Find(id);
            return PartialView(catalogdetail);
        }

        //
        // POST: /AdvertDetails/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            AdvertDetail catalogdetail = _db.CatalogDetails.Find(id);
            if (catalogdetail != null)
            {
                _db.CatalogDetails.Remove(catalogdetail);
                _db.SaveChanges();
                return RedirectToAction("IndexByCatalog", "CatalogDetails", new { id = catalogdetail.AdvertId });
            }
            Response.StatusCode = 400;
            return Content("El registro no fue encontrado.");
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}