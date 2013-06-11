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
    public class AdvertCampaignDetailController : Controller
    {
        private OMKTDB _db = new OMKTDB();

        //
        public PartialViewResult IndexByCampaign(int id)
        {
            ViewBag.AdvertCampaignId = id;
            var oCampaign = _db.AdvertCampaigns.FirstOrDefault(i => i.AdvertCampaignId == id);
            ViewBag.AdvertCampaign = oCampaign;
            var oAdvertCampaignDetails = _db.AdvertCampaignDetails.Include(i => i.AdvertCampaign).Where(i => i.AdvertCampaignId == id);
            return PartialView("Index", oAdvertCampaignDetails.ToList());
        }

        // GET: /AdvertCampaignDetail/

        public ViewResult Index()
        {
            var advertcampaigndetails = _db.AdvertCampaignDetails.Include(a => a.Advert);
            return View(advertcampaigndetails.ToList());
        }

        //
        // GET: /AdvertCampaignDetail/Details/5

        public ViewResult Details(int id)
        {
            AdvertCampaignDetail advertcampaigndetail = _db.AdvertCampaignDetails.Find(id);
            return View(advertcampaigndetail);
        }

        //
        // GET: /AdvertCampaignDetail/Create

        public ActionResult Create(int? id)
        {
            ViewBag.AdvertID = new SelectList(_db.Adverts, "AdvertId", "Name");
            AdvertCampaignDetail oDetail = null;
            if (id.HasValue)
            {
                var oCampaign = _db.AdvertCampaigns.Find(id);                    
                if (oCampaign != null)
                    oDetail = new AdvertCampaignDetail { AdvertCampaignId = id.Value, AdvertCampaign = oCampaign, EndDate = DateTime.Now.AddDays(30) };
                
            }
            return PartialView("Create", oDetail);
        }

        //
        // POST: /AdvertCampaignDetail/Create

        [HttpPost]
        public ActionResult Create(AdvertCampaignDetail advertcampaigndetail)
        {
            ViewBag.AdvertID = new SelectList(_db.Adverts, "AdvertId", "Name", advertcampaigndetail.AdvertID);
            if (ModelState.IsValid)
            {
                var check = _db.AdvertCampaignDetails.Where(a => a.AdvertID == advertcampaigndetail.AdvertID && a.AdvertCampaignId == advertcampaigndetail.AdvertCampaignId).FirstOrDefault();
                if (check == null)
                {
                    advertcampaigndetail.StartDate = DateTime.Now;
                    _db.AdvertCampaignDetails.Add(advertcampaigndetail);
                    try
                    {
                        _db.SaveChanges();
                        ViewBag.Success = "El anuncio fue agregado satisfactoriamente.";
                    }
                    catch (Exception)
                    {
                        ViewBag.Error = "Lo sentimos, ocurrió un error al procesar la solicitud.";
                        return PartialView("Create", advertcampaigndetail);
                    }
                }
                else
                { //TODO
                }
                var oCampaign = 
                    (from cam in _db.AdvertCampaigns
                     where cam.AdvertCampaignId == advertcampaigndetail.AdvertCampaignId
                     select cam).FirstOrDefault();
                ViewBag.Campaign = oCampaign;
                return PartialView("Index", _db.AdvertCampaignDetails.Where(cd => cd.AdvertCampaignId == advertcampaigndetail.AdvertCampaignId).Include(i => i.Advert));
            }
            return PartialView("Create", advertcampaigndetail);
        }

        //
        // GET: /AdvertCampaignDetail/Edit/5

        public ActionResult Edit(int id)
        {
            AdvertCampaignDetail advertcampaigndetail = _db.AdvertCampaignDetails.Find(id);
            ViewBag.AdvertID = new SelectList(_db.Adverts, "AdvertId", "Name", advertcampaigndetail.AdvertID);
            return PartialView(advertcampaigndetail);
        }

        //
        // POST: /AdvertCampaignDetail/Edit/5

        [HttpPost]
        public ActionResult Edit(AdvertCampaignDetail advertcampaigndetail)
        {
            ViewBag.AdvertID = new SelectList(_db.Adverts.OrderBy(c => c.Name), "AdvertId", "Name", advertcampaigndetail.AdvertID);
            if (ModelState.IsValid)
            {
                var oCampaign = _db.AdvertCampaigns.Find(advertcampaigndetail.AdvertCampaignId);
                oCampaign.LastUpdate = DateTime.Now;
                ViewBag.Campaign = oCampaign;
                _db.Entry(oCampaign).State = EntityState.Modified;
                _db.Entry(advertcampaigndetail).State = EntityState.Modified;
                try
                {
                    _db.SaveChanges();
                    ViewBag.Success = "El anuncio fue agregado satisfactoriamente.";                    
                }
                catch (Exception)
                {
                    ViewBag.Error = "Lo sentimos, ocurrió un error al procesar la solicitud.";
                    return PartialView("Edit", advertcampaigndetail);
                }
                return PartialView("Index", _db.AdvertCampaignDetails.Where(cd => cd.AdvertCampaignId == advertcampaigndetail.AdvertCampaignId).Include(i => i.Advert));
            }
            
            return PartialView("Edit", advertcampaigndetail);
        }

        //
        // GET: /AdvertCampaignDetail/Delete/5

        public ActionResult Delete(int id)
        {
            AdvertCampaignDetail advertcampaigndetail = _db.AdvertCampaignDetails.Find(id);
            return PartialView(advertcampaigndetail);
        }

        //
        // POST: /AdvertCampaignDetail/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            AdvertCampaignDetail advertcampaigndetail = _db.AdvertCampaignDetails.Find(id);
            if (advertcampaigndetail != null)
            {
                _db.AdvertCampaignDetails.Remove(advertcampaigndetail);
                _db.SaveChanges();
                return RedirectToAction("IndexByCampaign", "AdvertCampaignDetail", new { id = advertcampaigndetail.AdvertCampaignId });
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