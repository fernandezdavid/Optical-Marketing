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
            var oAdvertCampaignDetails = _db.AdvertCampaignDetails.Include(i => i.AdvertCampaign).Where(i => i.AdvertCampaignId == id && i.Status == "OK");
            return PartialView("Index", oAdvertCampaignDetails.ToList());
        }

        // GET: /AdvertCampaignDetail/

        public ViewResult Index()
        {
            var advertcampaigndetails = _db.AdvertCampaignDetails.Where(c => c.Status == "OK").Include(a => a.Advert);
            if (advertcampaigndetails == null) throw new HttpException(404, "The resource cannot be found");
            return View(advertcampaigndetails.ToList());
        }

        //
        // GET: /AdvertCampaignDetail/Details/5

        public ViewResult Details(int id)
        {
            var advertcampaigndetail = _db.AdvertCampaignDetails.Where(a => a.AdvertCampaignDetailId == id && a.Status == "OK");
            if (advertcampaigndetail == null) throw new HttpException(404, "The resource cannot be found");
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
                var check = _db.AdvertCampaignDetails.Where(a => a.AdvertID == advertcampaigndetail.AdvertID).FirstOrDefault();
                if (check == null)
                {
                    advertcampaigndetail.StartDate = DateTime.Now;
                    advertcampaigndetail.Status = "OK";
                    _db.AdvertCampaignDetails.Add(advertcampaigndetail);
                    try
                    {
                        _db.SaveChanges();
                        ViewBag.Success = "El anuncio fue agregado satisfactoriamente";
                    }
                    catch (Exception)
                    {
                        ViewBag.Error = "Lo sentimos, ocurrió un error al procesar la solicitud";
                    }
                }
                else if (check.Status == "DELETED")
                {
                    check.LastUpdate = DateTime.Now;
                    check.Status = "OK";
                    check.StartDate = advertcampaigndetail.StartDate;
                    check.EndDate = advertcampaigndetail.EndDate;
                    _db.Entry(check).State = EntityState.Modified;
                    try
                    {
                        _db.SaveChanges();
                        ViewBag.Success = "El anuncio fue restaurado exitosamente";
                    }
                    catch (Exception)
                    {
                        ViewBag.Error = "Lo sentimos, ocurrió un error mientras se procesaba la solicitud";
                    }
                }
                else
                {
                    ViewBag.Error = "Parece que este anuncio ya fue agregado!";
                }
                var oCampaign = _db.AdvertCampaigns.Find(advertcampaigndetail.AdvertCampaignId);
                ViewBag.Campaign = oCampaign;
                return PartialView("Index", _db.AdvertCampaignDetails.Where(cd => cd.AdvertCampaignId == advertcampaigndetail.AdvertCampaignId && cd.Status == "OK").Include(i => i.Advert));
            }
            return PartialView("Create", advertcampaigndetail);
        }

        //
        // GET: /AdvertCampaignDetail/Edit/5

        public ActionResult Edit(int id)
        {
            var advertcampaigndetail = _db.AdvertCampaignDetails.FirstOrDefault(a => a.AdvertCampaignId == id && a.Status == "OK");
            if (advertcampaigndetail == null) throw new HttpException(404, "The resource cannot be found");
            ViewBag.AdvertID = new SelectList(_db.Adverts.Where(a => a.Status == "OK"), "AdvertId", "Name", advertcampaigndetail.AdvertID);
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
                ViewBag.Campaign = oCampaign;
                advertcampaigndetail.LastUpdate = DateTime.Now;
                _db.Entry(advertcampaigndetail).State = EntityState.Modified;
                try
                {
                    _db.SaveChanges();
                    ViewBag.Success = "El anuncio fue agregado satisfactoriamente.";
                }
                catch (Exception)
                {
                    ViewBag.Error = "Lo sentimos, ocurrió un error al procesar la solicitud.";
                }

                return PartialView("Index", _db.AdvertCampaignDetails.Where(cd => cd.AdvertCampaignId == advertcampaigndetail.AdvertCampaignId && cd.Status == "OK").Include(i => i.Advert));
            }
            return PartialView("Edit", advertcampaigndetail);
        }

        //
        // GET: /AdvertCampaignDetail/Delete/5

        public ActionResult Delete(int id)
        {
            var advertcampaigndetail = _db.AdvertCampaignDetails.FirstOrDefault(c => c.AdvertCampaignDetailId == id && c.Status == "OK");
            if (advertcampaigndetail == null) throw new HttpException(404, "The resource cannot be found");
            return PartialView(advertcampaigndetail);
        }

        //
        // POST: /AdvertCampaignDetail/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var advertcampaigndetail = _db.AdvertCampaignDetails.FirstOrDefault(c => c.AdvertCampaignDetailId == id && c.Status == "OK");
            if (advertcampaigndetail != null)
            {
                advertcampaigndetail.Status = "DELETED";
                advertcampaigndetail.LastUpdate = DateTime.Now;
                _db.Entry(advertcampaigndetail).State = EntityState.Modified;
                try
                {
                    ViewBag.Success = "El anuncio fue quitado exitosamente!";
                    _db.SaveChanges();
                }
                catch (Exception)
                {
                    ViewBag.Error = "Lo sentimos, ocurrió un error mientras se procesaba la solicitud.";
                }
            }
            else { ViewBag.Error = "Lo sentimos, no pudimos encontrar el detalle."; }
            return PartialView("Index", _db.AdvertCampaignDetails.Where(cd => cd.AdvertCampaignId == advertcampaigndetail.AdvertCampaignId && cd.Status == "OK").Include(i => i.AdvertCampaign));            
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}