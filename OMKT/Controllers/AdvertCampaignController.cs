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
    public class AdvertCampaignController : Controller
    {
        private readonly OMKTDB _db = new OMKTDB();

        /**
         * method LatestCampaings
         *
         * List a number of campaigns order by created date descending
         *
         * @since 04/04/2013
         * @return advertCampaign collection
         */

        public PartialViewResult LatestCampaigns(int? state, int? period)
        {
            var oUser = (User)Session["User"];
            var campaigns = _db.AdvertCampaigns;

            campaigns.Where(i => i.CustomerId == oUser.CustomerId);

            if (state.HasValue && state != 0)
            {
                campaigns.Where(i => i.CampaignStateId == state);
            }
            if (period.HasValue && period != 0)
            {
                campaigns.Where(i => i.StartDatetime >= i.StartDatetime.AddDays(-(double)period));
            }

            campaigns.OrderByDescending(i => i.CreatedDate);
            return PartialView("AdvertCampaignListPartial", campaigns.ToList());
        }

        /**
         * method DashboardCampaings
         *
         * List a number of campaigns order by created date descending
         *
         * @since 04/04/2013
         * @return advertCampaign collection
         */

        public PartialViewResult DashboardCampaigns(int? top)
        {
            if (!top.HasValue) top = 10;
            var oUser = (User)Session["User"];
            var campaigns = _db.AdvertCampaigns.Where(i => i.CustomerId == oUser.CustomerId).Take(top.Value);
            return PartialView("AdvertCampaignListSlimPartial", campaigns.ToList());
        }

        /**
         * method ActiveCampaigns
         *
         * List a number of active campaigns order by name descending
         *
         * @since 04/04/2013
         * @return advertCampaign collection
         */

        public PartialViewResult ActiveCampaigns(int? top)
        {
            if (!top.HasValue)
                top = 5;
            var oUser = (User)Session["User"];
            var campaigns = _db.AdvertCampaigns.Where(c => c.CustomerId == oUser.CustomerId).OrderByDescending(i => i.Name).Include(a => a.AdvertCampaignDetails).Take(top.Value);
            return (campaigns.Any()) ? PartialView("AdvertCampaignListSlimPartial", campaigns.ToList()) : PartialView("AdvertCampaignListSlimPartial");
        }

        /**
         * method Index
         *
         * Show main page of advertCampaigns
         *
         * @since 04/04/2013
         * @return view
         */

        public ActionResult Index()
        {
            return View();
        }

        /**
         * method Details
         *
         * Show advertCampaign details
         *
         * @since 04/04/2013
         * @return viewmodel
         */

        public ViewResult Details(int id)
        {
            var oUser = (User)Session["User"];
            //@TODO customerId check
            AdvertCampaign advertcampaign = _db.AdvertCampaigns.Find(id);
            return View(advertcampaign);
        }

        /**
         * method Create
         *
         * Show advertCampaign create form
         *
         * @since 04/04/2013
         * @return view form
         */

        public ActionResult Create()
        {
            ViewBag.CampaignTypeId = new SelectList(_db.CampaignTypes.OrderBy(c => c.Name), "CampaignTypeId", "Name", 1);
            var oCampaign = new AdvertCampaign
            {
                StartDatetime = DateTime.Now,
                EndDatetime = DateTime.Now.AddDays(30),
                NetworkId = 1 //@TODO do better
            };
            return View(oCampaign);
        }

        /**
        * method Create (post)
        *
        * Manage advertCampaign create form
        *
        * @since 04/04/2013
        * @return view form
        */

        [HttpPost]
        public ActionResult Create(AdvertCampaign advertcampaign)
        {
            ViewBag.CampaignTypeId = new SelectList(_db.CampaignTypes.OrderBy(c => c.Name), "CampaignTypeId", "Name");
            if (ModelState.IsValid)
            {
                advertcampaign.CreatedDate = DateTime.Now;
                advertcampaign.StartDatetime = DateTime.Now;
                advertcampaign.CampaignState = _db.CampaignStates.Find(1); //@TODO do better
                advertcampaign.LastUpdate = DateTime.Now; //sacar esta bazofia
                var oUser = (User)Session["User"];
                if (oUser != null)
                {
                    advertcampaign.CustomerId = oUser.CustomerId;
                    _db.AdvertCampaigns.Add(advertcampaign);
                    try
                    {
                        _db.SaveChanges();
                        ViewBag.Success = "La campaña fue registrada satisfactoriamente.";
                        return RedirectToAction("Edit", "AdvertCampaign", new { id = advertcampaign.AdvertCampaignId });
                    }
                    catch (Exception)
                    {
                        ViewBag.Error = "Lo sentimos, ocurrió un error mientras se procesaba la solicitud.";
                        return View(advertcampaign);
                    }
                }
            }
            return View(advertcampaign);
        }

        /**
        * method Edit
        *
        * Manage advertCampaign edit form
        *
        * @since 04/04/2013
        * @return view form
        */

        public ActionResult Edit(int id)
        {
            AdvertCampaign advertcampaign = _db.AdvertCampaigns.Find(id); //@TODO customer check
            ViewBag.CampaignTypeId = new SelectList(_db.CampaignTypes.OrderBy(c => c.Name), "CampaignTypeId", "Name", advertcampaign.CampaignTypeId);

            return View(advertcampaign);
        }

        /**
        * method Edit (post)
        *
        * Manage advertCampaign edit form
        *
        * @since 04/04/2013
        * @return view form
        */

        [HttpPost]
        public ActionResult Edit(AdvertCampaign advertcampaign)
        {
            ViewBag.CampaignTypeId = new SelectList(_db.CampaignTypes.OrderBy(c => c.Name), "CampaignTypeId", "Name");
            if (ModelState.IsValid)
            {
                advertcampaign.LastUpdate = DateTime.Now;
                _db.Entry(advertcampaign).State = EntityState.Modified;
                try
                {
                    _db.SaveChanges();
                    ViewBag.Success = "La campaña fue editada satisfactoriamente.";
                    return View(advertcampaign);
                }
                catch (Exception)
                {
                    ViewBag.Error = "Lo sentimos, ocurrió un error mientras se procesaba la solicitud.";
                    return View(advertcampaign);
                }
            }
            return View(advertcampaign);
        }

        /**
       * method Delete
       *
       * Manage advertCampaign delete form
       *
       * @since 04/04/2013
       * @return view form
       */

        public ActionResult Delete(int id)
        {
            AdvertCampaign advertcampaign = _db.AdvertCampaigns.Find(id); //@TODO customer check
            return View(advertcampaign);
        }

        /**
        * method Delete (post)
        *
        * Manage advertCampaign delete confirm form
        *
        * @since 04/04/2013
        * @return view form
        */

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            AdvertCampaign advertcampaign = _db.AdvertCampaigns.Find(id);
            _db.AdvertCampaigns.Remove(advertcampaign);
            try
            {
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View();
            }
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}