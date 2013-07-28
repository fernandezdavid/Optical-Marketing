using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using OMKT.Business;
using OMKT.Context;
using OMKT.Models;
using Paging;

namespace OMKT.Controllers
{
    [Authorize]
    public class CatalogController : Controller
    {
        private readonly OMKTDB _db = new OMKTDB();
        private readonly int _defaultPageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["DefaultPaginationSize"]);

        public ViewResultBase Search(string text, string from, string to, int? page, int? pagesize)
        {
            Session["Text"] = text;
            Session["From"] = from;
            Session["To"] = to;

            var catalogs = _db.Catalogs.Include(i => i.AdvertDetails);

            if (!string.IsNullOrWhiteSpace(from))
            {
                DateTime fromDate;
                if (DateTime.TryParse(from, CultureInfo.CurrentUICulture, DateTimeStyles.AssumeLocal, out fromDate))
                    catalogs = catalogs.Where(t => t.StartDatetime >= fromDate);
            }
            if (!string.IsNullOrWhiteSpace(to))
            {
                DateTime toDate;
                if (DateTime.TryParse(to, CultureInfo.CurrentUICulture, DateTimeStyles.AssumeLocal, out toDate))
                    catalogs = catalogs.Where(t => t.StartDatetime <= toDate);
            }

            if (!string.IsNullOrWhiteSpace(text))
            {
                //filtro de texto
                catalogs = catalogs.Where(t => t.Name.ToLower().IndexOf(text.ToLower(), System.StringComparison.Ordinal) > -1);
            }

            var currentPageIndex = page.HasValue ? page.Value - 1 : 0;

            //var catalogsPaged = catalogs.OrderByDescending(i => i.EndDatetime).ToPagedList(currentPageIndex, (pagesize.HasValue) ? pagesize.Value : _defaultPageSize);
            var oUser = (User)Session["User"];
            var campaigns = _db.AdvertCampaignDetails;
            IPagedList<AdvertCampaignDetail> catalogsPaged = campaigns.Where(c => c.AdvertCampaign.CustomerId == oUser.CustomerId).OrderByDescending(i => i.Advert.CreatedDate).ToPagedList(currentPageIndex, (pagesize.HasValue) ? pagesize.Value : _defaultPageSize);
            if (Request.IsAjaxRequest())
                return PartialView("Index", catalogsPaged);
            return View("Index", catalogsPaged);
        }

        public PartialViewResult LatestCatalogs(int? top)
        {
            if (!top.HasValue)
                top = 10;
            var oUser = (User)Session["User"];
            var catalogs = _db.Catalogs.OrderByDescending(i => i.Name).Take(top.Value);
            return PartialView("CatalogListPartial", catalogs.ToList());
        }

        public PartialViewResult DashboardCatalogs(int? top)
        {
            if (!top.HasValue)
                top = 10;
            var oUser = (User)Session["User"];
            var catalogs = _db.Catalogs.OrderByDescending(i => i.Name).Take(top.Value);
            return PartialView("CatalogListSlimPartial", catalogs.ToList());
        }

        //
        // GET: /Catalog/

        public ActionResult Index()
        {
            return View();
        }

        // GET: /Catalog/Details/5

        public ViewResult Details(int id)
        {
            Catalog catalog = _db.Catalogs.Find(id);
            return View(catalog);
        }

        //
        // GET: /Catalog/Create

        public ActionResult Create()
        {
            ViewBag.SortTypeId = new SelectList(_db.SortTypes.OrderBy(x => x.Name), "SortTypeId", "Name");
            ViewBag.AdvertStateId = new SelectList(_db.AdvertStates.OrderBy(x => x.Description), "AdvertStateId", "Description");
            var oCatalog = new Catalog { StartDatetime = DateTime.Now, EndDatetime = DateTime.Now.AddDays(30) };

            return View(oCatalog);
        }

        //
        // POST: /Catalog/Create

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Catalog catalog)
        {
            ViewBag.SortTypeId = new SelectList(_db.SortTypes.OrderBy(x => x.Name), "SortTypeId", "Name", catalog.SortTypeId);
            ViewBag.AdvertStateId = new SelectList(_db.AdvertStates.OrderBy(x => x.Description), "AdvertStateId", "Description", catalog.AdvertStateId);
            if (ModelState.IsValid)
            {
                catalog.AdvertType = _db.AdvertTypes.Find(1);
                catalog.CreatedDate = DateTime.Now;
                catalog.StartDatetime = DateTime.Now;
                _db.Catalogs.Add(catalog);
                try
                {
                    _db.SaveChanges();
                    ViewBag.Success = "El catálogo fue registrado satisfactoriamente.";
                    return RedirectToAction("Edit", "Catalog", new { id = catalog.AdvertId });
                }
                catch (Exception)
                {
                    ViewBag.Error = "Lo sentimos, ocurrió un error mientras se procesaba la solicitud.";
                    return View(catalog);
                }
            }
            return View(catalog);
        }

        //
        // GET: /Catalog/Edit/5

        public ActionResult Edit(int id)
        {
            Catalog catalog = _db.Catalogs.Find(id);
            ViewBag.SortTypeId = new SelectList(_db.SortTypes.OrderBy(x => x.Name), "SortTypeId", "Name", catalog.SortTypeId);
            ViewBag.AdvertStateId = new SelectList(_db.AdvertStates.OrderBy(x => x.Description), "AdvertStateId", "Description", catalog.AdvertStateId);

            return View(catalog);
        }

        //
        // POST: /Catalog/Edit/5

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Catalog catalog)
        {
            ViewBag.SortTypeId = new SelectList(_db.SortTypes.OrderBy(x => x.Name), "SortTypeId", "Name", catalog.SortTypeId);
            ViewBag.AdvertStateId = new SelectList(_db.AdvertStates.OrderBy(x => x.Description), "AdvertStateId", "Description", catalog.AdvertStateId);

            var user = (User)Session["User"];
            if (ModelState.IsValid)
            {
                catalog.LastUpdate = DateTime.Now;
                _db.Entry(catalog).State = EntityState.Modified;
                try
                {
                    _db.SaveChanges();
                    ViewBag.Success = "El catálogo fue editado satisfactoriamente.";
                    return View(catalog);
                }
                catch (Exception)
                {
                    ViewBag.Error = "Lo sentimos, ocurrió un error mientras se procesaba la solicitud.";
                    return View(catalog);
                }
            }
            return View(catalog);
        }

        //
        // GET: /Catalog/Delete/5

        public ActionResult Delete(int id)
        {
            Catalog catalog = _db.Catalogs.Find(id);
            return PartialView(catalog);
        }

        //
        // POST: /Catalog/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Catalog catalog = _db.Catalogs.Find(id);
            _db.Catalogs.Remove(catalog);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult CatalogsOverview()
        {
            var oUser = (User)Session["User"];
            var advertDetails = _db.AdvertCampaignDetails.Where(c => c.AdvertCampaign.CustomerId == oUser.CustomerId);
            var interactions = new List<CatalogOverview>();
            var views = 0;
            foreach (var cat in advertDetails)
            {
                views = _db.AdvertCampaignDetailInteractions.Where(c => c.AdvertID == cat.AdvertID).Count();
                var oCO = new CatalogOverview();
                oCO.Views = views;
                oCO.CatalogtName = cat.Advert.Name;
                interactions.Add(oCO);
            }
            return PartialView("CatalogsOverview", interactions.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}